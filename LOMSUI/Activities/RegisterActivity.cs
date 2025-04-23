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
            string username = _usernameEditText.Text?.Trim();
            string phone = _phoneEditText.Text?.Trim();
            string email = _emailEditText.Text?.Trim();
            string password = _passwordEditText.Text?.Trim();
            string confirmPassword = _confirmPasswordEditText.Text?.Trim();
            string address = _addressEditText.Text?.Trim();
            string gender = _genderSpinner.SelectedItem?.ToString();
            string fullName = _fullNameEditText.Text?.Trim();

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
                    Address = address,
                    Gender = gender,
                    FullName = fullName,
                    AvatarData = _avatarImageData
                };

                var result = await _apiService.RegisterAsync(registerModel, _selectedImageUri);

                if (result.IsSuccess)
                {
                    Toast.MakeText(this, "Registration successful. Please check your email for OTP code.", ToastLength.Long).Show();
                    StartActivity(new Intent(this, typeof(VerifyOtpRegisterActivity)).PutExtra("email", email));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Toast.MakeText(this, error, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }

}