using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Util;
using System;
using System.Threading.Tasks;
using LOMSUI.Models;
using LOMSUI.Services;
using Android.Provider;
using Android.Graphics;
using System.IO;
using Android.Views;

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

            // Find UI elements
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

            // Kiểm tra null cho các view
            if (_usernameEditText == null || _phoneEditText == null || _emailEditText == null ||
                _passwordEditText == null || _confirmPasswordEditText == null || _genderSpinner == null ||
                _backButton == null || _registerButton == null || _avatarImageView == null || _fullNameEditText == null)
            {
                Log.Error("RegisterActivity", "Error: One or more UI elements not found!");
                Toast.MakeText(this, "Error loading UI components!", ToastLength.Long).Show();
                return;
            }

            // Attach event listeners
            _registerButton.Click += RegisterButton_Click;
            _backButton.Click += BackButton_Click;
            _avatarImageView.Click += AvatarImageView_Click;

            // Populate gender spinner
            ArrayAdapter<string> genderAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Resources.GetStringArray(Resource.Array.gender_options));
            _genderSpinner.Adapter = genderAdapter;
        }

        private void AvatarImageView_Click(object sender, EventArgs e)
        {
            Intent imageIntent = new Intent(Intent.ActionPick);
            imageIntent.SetType("image/*");
            StartActivityForResult(imageIntent, PickImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PickImageId && resultCode == Result.Ok && data != null)
            {
                _selectedImageUri = data.Data;
                _avatarImageView.SetImageURI(_selectedImageUri);

                try
                {
                    using (Stream inputStream = ContentResolver.OpenInputStream(_selectedImageUri))
                    {
                        if (inputStream != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                inputStream.CopyTo(memoryStream);
                                _avatarImageData = memoryStream.ToArray();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("RegisterActivity", "Lỗi chuyển đổi ảnh thành byte array: " + ex.Message);
                    Toast.MakeText(this, "Lỗi đọc ảnh!", ToastLength.Short).Show();
                    _avatarImageData = null;
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
                Toast.MakeText(this, "Vui lòng điền đầy đủ thông tin", ToastLength.Short).Show();
                return;
            }

            if (password != confirmPassword)
            {
                Toast.MakeText(this, "Mật khẩu không khớp", ToastLength.Short).Show();
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
                    Toast.MakeText(this, "Yêu cầu đăng ký thành công. Vui lòng kiểm tra email để lấy mã OTP.", ToastLength.Long).Show();
                    Intent intent = new Intent(this, typeof(VerifyOtpRegisterActivity));
                    intent.PutExtra("email", email);
                    StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "Đăng ký thất bại. Vui lòng thử lại.", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Log.Error("RegisterActivity", "Lỗi trong quá trình đăng ký: " + ex.Message);
                Toast.MakeText(this, "Lỗi: " + ex.Message, ToastLength.Long).Show();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}