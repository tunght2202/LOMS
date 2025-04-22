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
    public class RegisterActivity : Activity
    {
        private EditText _usernameEditText;
        private EditText _phoneEditText;
        private EditText _emailEditText;
        private EditText _passwordEditText;
        private EditText _confirmPasswordEditText;
        private EditText _addressEditText;
        private Spinner _genderSpinner;
        private EditText _fullNameEditText;
        private Button _backButton;
        private Button _registerButton;
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

            if (_usernameEditText == null || _phoneEditText == null || _emailEditText == null ||
                _passwordEditText == null || _confirmPasswordEditText == null || _genderSpinner == null ||
                _backButton == null || _registerButton == null || _avatarImageView == null || _fullNameEditText == null)
            {
                Toast.MakeText(this, "Error loading UI components!", ToastLength.Long).Show();
                return;
            }

            _registerButton.Click += RegisterButton_Click;
            _backButton.Click += BackButton_Click;
            _avatarImageView.Click += AvatarImageView_Click;

            ArrayAdapter<string> genderAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Resources.GetStringArray(Resource.Array.gender_options));
            _genderSpinner.Adapter = genderAdapter;
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

                using (var input = ContentResolver.OpenInputStream(_selectedImageUri))
                {
                    var options = new BitmapFactory.Options
                    {
                        InJustDecodeBounds = true 
                    };
                    BitmapFactory.DecodeStream(input, null, options);

                    int inSampleSize = Math.Max(options.OutHeight / 800, options.OutWidth / 800);
                    options.InSampleSize = inSampleSize > 0 ? inSampleSize : 1;
                    options.InJustDecodeBounds = false; 

                    input.Close();

                    using (var resizedStream = ContentResolver.OpenInputStream(_selectedImageUri))
                    {
                        Bitmap bitmap = BitmapFactory.DecodeStream(resizedStream, null, options);

                        using (var stream = new MemoryStream())
                        {
                            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            _avatarImageData = stream.ToArray(); 
                        }

                        _avatarImageView.SetImageBitmap(bitmap);
                    }
                }
            }
        }


        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            string username = _usernameEditText.Text?.Trim();
            string phone = _phoneEditText.Text?.Trim();
            string email = _emailEditText.Text?.Trim();
            string password = _passwordEditText.Text.Trim();
            string confirmPassword = _confirmPasswordEditText.Text.Trim();
            string address = _addressEditText.Text?.Trim();
            string gender = _genderSpinner.SelectedItem?.ToString();
            string fullName = _fullNameEditText.Text?.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(fullName))
            {
                Toast.MakeText(this, "Please fill in all information", ToastLength.Short).Show();
                return;
            }

            if (password != confirmPassword)
            {
                Toast.MakeText(this, "Passwords do not match", ToastLength.Short).Show();
                return;
            }

            try
            {
                RegisterModel registerModel = new RegisterModel
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

                bool registrationSuccessful;

                if (_avatarImageData != null && _selectedImageUri != null)
                {
                    using (Stream imageStream = ContentResolver.OpenInputStream(_selectedImageUri))
                    {
                        registrationSuccessful = await _apiService.RegisterWithAvatarAsync(registerModel, imageStream);
                    }
                }
                else
                {
                    registrationSuccessful = await _apiService.RegisterAsync(registerModel);
                }

                if (registrationSuccessful)
                {
                    Toast.MakeText(this, "Registration request successful. Please check your email for OTP code.", ToastLength.Long).Show();
                    Intent intent = new Intent(this, typeof(VerifyOtpRegisterActivity));
                    intent.PutExtra("email", email);
                    StartActivity(intent);
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

        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}