using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Util;
using System;
using System.Threading.Tasks;
using LOMSUI.Models;
using LOMSUI.Services;
<<<<<<< HEAD
using Android.Provider;
using Android.Graphics;
using System.IO;
using Android.Views;
=======
using LOMSUI.Activities;
using Android.Graphics;
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2

namespace LOMSUI.Activities
{
    [Activity(Label = "Register")]
    public class RegisterActivity : BaseActivity
    {
        private EditText _usernameEditText, _phoneEditText, _emailEditText, _passwordEditText,
                         _confirmPasswordEditText, _addressEditText, _fullNameEditText;
        private Spinner _genderSpinner;
<<<<<<< HEAD
        private EditText _fullNameEditText;
        private Button _backButton;
        private Button _registerButton;
=======
        private Button _backButton, _registerButton;
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
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

<<<<<<< HEAD
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
=======
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
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
<<<<<<< HEAD
            string password = _passwordEditText.Text.Trim();
            string confirmPassword = _confirmPasswordEditText.Text.Trim();
=======
            string password = _passwordEditText.Text?.Trim();
            string fullName = _fullNameEditText.Text?.Trim();
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
            string address = _addressEditText.Text?.Trim();
            string gender = _genderSpinner.SelectedItem?.ToString();
            string fullName = _fullNameEditText.Text?.Trim();

<<<<<<< HEAD
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(fullName))
            {
                Toast.MakeText(this, "Vui lòng điền đầy đủ thông tin", ToastLength.Short).Show();
                return;
=======
            _usernameEditText.Error = null;
            _phoneEditText.Error = null;
            _emailEditText.Error = null;
            _passwordEditText.Error = null;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(username))
            {
                _usernameEditText.Error = "Username is required";
                hasError = true;
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
<<<<<<< HEAD
                Toast.MakeText(this, "Mật khẩu không khớp", ToastLength.Short).Show();
                return;
=======
                _phoneEditText.Error = "Phone number is required";
                hasError = true;
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                _emailEditText.Error = "Email is required";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _passwordEditText.Error = "Password is required";
                hasError = true;
            }

            if (hasError) return; 

            try
            {
                RegisterModel registerModel = new RegisterModel
                {
                    Username = username,
                    PhoneNumber = phone,
                    Email = email,
                    Password = password,
<<<<<<< HEAD
                    Gender = gender,
                    Address = address,
=======
                    Address = address,
                    Gender = gender,
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
                    FullName = fullName,
                    AvatarData = _avatarImageData
                };

<<<<<<< HEAD
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
=======
                var result = await _apiService.RegisterAsync(registerModel, _selectedImageUri);

                if (result.Message.Contains("Please check email for otp code!", StringComparison.OrdinalIgnoreCase) == true)
                {
                    StartActivity(new Intent(this, typeof(VerifyOtpRegisterActivity)).PutExtra("email", email));
                }
                else if (result.Errors != null && result.Errors.Count > 0)
                {
                    if (result.Errors.ContainsKey("UserName"))
                        _usernameEditText.Error = string.Join("\n", result.Errors["UserName"]);

                    if (result.Errors.ContainsKey("PhoneNumber"))
                        _phoneEditText.Error = string.Join("\n", result.Errors["PhoneNumber"]);

                    if (result.Errors.ContainsKey("Email"))
                        _emailEditText.Error = string.Join("\n", result.Errors["Email"]);

                    if (result.Errors.ContainsKey("Password"))
                        _passwordEditText.Error = string.Join("\n", result.Errors["Password"]);
                }
                else
                {
                    Toast.MakeText(this, result.Message ?? "Unknown error occurred.", ToastLength.Long).Show();
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                Log.Error("RegisterActivity", "Lỗi trong quá trình đăng ký: " + ex.Message);
                Toast.MakeText(this, "Lỗi: " + ex.Message, ToastLength.Long).Show();
=======
                Toast.MakeText(this, $"Unexpected error: {ex.Message}", ToastLength.Long).Show();
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
            }
        }


        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }

}