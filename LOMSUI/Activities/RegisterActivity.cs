using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Util;
using System;
using System.Threading.Tasks;
using LOMSUI.Models;
using LOMSUI.Services;
using LOMSUI.Activities;
using Android.Graphics;

namespace LOMSUI.Activities
{
    [Activity(Label = "Register")]
    public class RegisterActivity : BaseActivity
    {
        private EditText _usernameEditText, _phoneEditText, _emailEditText, _passwordEditText,
                         _confirmPasswordEditText, _addressEditText, _fullNameEditText;
        private Spinner _genderSpinner;
        private Button _backButton, _registerButton;
        private ImageView _avatarImageView;
        private readonly ApiService _apiService = new ApiService();
        private Android.Net.Uri _selectedImageUri;
        private byte[] _avatarImageData;

        private const int PickImageId = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register);

            _usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            _phoneEditText = FindViewById<EditText>(Resource.Id.phoneEditText);
            _emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            _passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            _confirmPasswordEditText = FindViewById<EditText>(Resource.Id.confirmPasswordEditText);
            _addressEditText = FindViewById<EditText>(Resource.Id.addressEditText);
            _genderSpinner = FindViewById<Spinner>(Resource.Id.genderSpinner);
            _fullNameEditText = FindViewById<EditText>(Resource.Id.fullNameEditText);
            _backButton = FindViewById<Button>(Resource.Id.backButton);
            _registerButton = FindViewById<Button>(Resource.Id.registerButton);
            _avatarImageView = FindViewById<ImageView>(Resource.Id.avatarImageView);

            ArrayAdapter<string> genderAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Resources.GetStringArray(Resource.Array.gender_options));
            _genderSpinner.Adapter = genderAdapter;

            _registerButton.Click += RegisterButton_Click;
            _backButton.Click += BackButton_Click;
            _avatarImageView.Click += AvatarImageView_Click;
        }

        private void AvatarImageView_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            intent.AddCategory(Intent.CategoryOpenable);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Image"), 101);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 101 && resultCode == Result.Ok && data != null)
            {
                _selectedImageUri = data.Data;
                _avatarImageData = ProcessAvatarImage(_selectedImageUri);
                _avatarImageView.SetImageBitmap(BitmapFactory.DecodeByteArray(_avatarImageData, 0, _avatarImageData.Length));
            }
        }
        private byte[] ProcessAvatarImage(Android.Net.Uri imageUri)
        {
            using (var input = ContentResolver.OpenInputStream(imageUri))
            {
                var options = new BitmapFactory.Options { InJustDecodeBounds = true };
                BitmapFactory.DecodeStream(input, null, options);

                int inSampleSize = Math.Max(options.OutHeight / 800, options.OutWidth / 800);
                options.InSampleSize = inSampleSize > 0 ? inSampleSize : 1;
                options.InJustDecodeBounds = false;

                input.Close();

                using (var resizedStream = ContentResolver.OpenInputStream(imageUri))
                {
                    Bitmap bitmap = BitmapFactory.DecodeStream(resizedStream, null, options);
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Jpeg, 20, stream); 
                        stream.Seek(0, SeekOrigin.Begin);
                        return stream.ToArray();
                    }
                }
            }
        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            if (!ValidateFields(out string username, out string phone, out string email,
                                 out string password, out string confirmPassword, out string address,
                                 out string gender, out string fullName))
                return;

            if (password != confirmPassword)
            {
                Toast.MakeText(this, "Passwords do not match", ToastLength.Short).Show();
                return;
            }

            try
            {
                var registerModel = new RegisterModel
                {
                    Username = username,
                    PhoneNumber = phone,
                    Email = email,
                    Password = password,
                    Gender = gender,
                    Address = address,
                    FullName = fullName,
                    AvatarData = _avatarImageData
                };

                bool registrationSuccessful = _avatarImageData != null && _selectedImageUri != null
                    ? await RegisterWithAvatar(registerModel)
                    : await _apiService.RegisterAsync(registerModel);

                if (registrationSuccessful)
                {
                    Toast.MakeText(this, "Registration successful. Please check your email for OTP code.", ToastLength.Long).Show();
                    StartActivity(new Intent(this, typeof(VerifyOtpRegisterActivity)).PutExtra("email", email));
                }
                else
                {
                    Toast.MakeText(this, "Registration failed. Please try again.", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Log.Error("RegisterActivity", "Error during registration: " + ex.Message);
                Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }

        private bool ValidateFields(out string username, out string phone, out string email,
                                     out string password, out string confirmPassword, out string address,
                                     out string gender, out string fullName)
        {
            username = _usernameEditText.Text?.Trim();
            phone = _phoneEditText.Text?.Trim();
            email = _emailEditText.Text?.Trim();
            password = _passwordEditText.Text?.Trim();
            confirmPassword = _confirmPasswordEditText.Text?.Trim();
            address = _addressEditText.Text?.Trim();
            gender = _genderSpinner.SelectedItem?.ToString();
            fullName = _fullNameEditText.Text?.Trim();

            var missingFields = new List<string>();
            if (string.IsNullOrEmpty(username)) missingFields.Add("Username");
            if (string.IsNullOrEmpty(phone)) missingFields.Add("Phone number");
            if (string.IsNullOrEmpty(email)) missingFields.Add("Email");
            if (string.IsNullOrEmpty(password)) missingFields.Add("Password");
            if (string.IsNullOrEmpty(confirmPassword)) missingFields.Add("Confirm Password");
            if (string.IsNullOrEmpty(gender)) missingFields.Add("Gender");
            if (string.IsNullOrEmpty(address)) missingFields.Add("Address");
            if (string.IsNullOrEmpty(fullName)) missingFields.Add("Full Name");

            if (missingFields.Any())
            {
                Toast.MakeText(this, $"{string.Join(", ", missingFields)} are required", ToastLength.Short).Show();
                return false;
            }

            return true;
        }
        private async Task<bool> RegisterWithAvatar(RegisterModel registerModel)
        {
            using (Stream imageStream = ContentResolver.OpenInputStream(_selectedImageUri))
            {
                return await _apiService.RegisterWithAvatarAsync(registerModel, imageStream);
            }
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }

}