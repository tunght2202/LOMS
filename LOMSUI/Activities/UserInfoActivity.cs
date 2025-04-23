using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Provider;
using Android.Graphics;
using LOMSUI.Models;
using LOMSUI.Services;
using System.IO;
using System.Net.Http;
using Bumptech.Glide;
using Android.Views;

namespace LOMSUI.Activities
{
    [Activity(Label = "User Update")]
    public class UserInfoActivity : BaseActivity
    {
        private EditText _userNameEditText, _phoneEditText, 
                         _emailEditText, _addressEditText, 
                         _passwordEditText, _confirmPasswordEditText;
        private Spinner _genderSpinner;
        private Button _updateButton;
        private ApiService _apiService;

        protected override async void OnCreate(Bundle savedInstanceState)   
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_user_info);

            _apiService = ApiServiceProvider.Instance;

            InitViews();

            await LoadUserInfo();

            _updateButton.Click += async (s, e) => await UpdateUserInfo();
        }

        private void InitViews()
        {
            _userNameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            _phoneEditText = FindViewById<EditText>(Resource.Id.phoneEditText);
            _emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            _addressEditText = FindViewById<EditText>(Resource.Id.addressEditText);
            _genderSpinner = FindViewById<Spinner>(Resource.Id.genderSpinner);
            _passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            _confirmPasswordEditText = FindViewById<EditText>(Resource.Id.confirmPasswordEditText);
            _updateButton = FindViewById<Button>(Resource.Id.updateButton);

            var genderAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.gender_options, Android.Resource.Layout.SimpleSpinnerItem);
            genderAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _genderSpinner.Adapter = genderAdapter;
        }

        private async Task LoadUserInfo()
        {
            try
            {
                var user = await _apiService.GetUserProfileAsync();
                if (user != null)
                {
                    _userNameEditText.Text = user.UserName;
                    _phoneEditText.Text = user.PhoneNumber;
                    _emailEditText.Text = user.Email;
                    _addressEditText.Text = user.Address;
                    _genderSpinner.SetSelection(user.Gender == "Female" ? 0 : 1);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error loading information: " + ex.Message, ToastLength.Long).Show();
            }
        }

        private async Task UpdateUserInfo()
        {
            string name = _userNameEditText.Text.Trim();
            string phone = _phoneEditText.Text.Trim();
            string email = _emailEditText.Text.Trim();
            string address = _addressEditText.Text.Trim();
            string gender = _genderSpinner.SelectedItem.ToString();
            string password = _passwordEditText.Text.Trim();
            string confirm = _confirmPasswordEditText.Text.Trim();

            if (!string.IsNullOrEmpty(password) && password != confirm)
            {
                Toast.MakeText(this, "Re-entered password does not match", ToastLength.Short).Show();
                return;
            }

            try
            {
                var result = await _apiService.UpdateUserProfileRequestAsync(new UserModels
                {
                    UserName = name,
                    PhoneNumber = phone,
                    Email = email,
                    Address = address,
                    Gender = gender,
                    Password = password
                });

                if (result.Errors != null && result.Errors.Count > 0)
                {
                    if (result.Errors.ContainsKey("Email"))
                        _emailEditText.Error = string.Join("\n", result.Errors["Email"]);

                    if (result.Errors.ContainsKey("Password"))
                        _passwordEditText.Error = string.Join("\n", result.Errors["Password"]);

                    if (result.Errors.ContainsKey("PhoneNumber"))
                        _phoneEditText.Error = string.Join("\n", result.Errors["PhoneNumber"]);

                    return;
                }

                if (result.Message == "Please enter email verification code to change email.")
                {
                    Toast.MakeText(this, "OTP code sent to email, please verify.", ToastLength.Long).Show();
                    var intent = new Intent(this, typeof(VerifyOtpUpdateUserActivity));
                    intent.PutExtra("email", email);
                    StartActivity(intent);
                }
                else if (result.Message == "Information edited successfully.")
                {
                    Toast.MakeText(this, "Information updated successfully!", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, result.Message ?? "Unknown response", ToastLength.Long).Show();
                }

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Update error: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }

}
