﻿using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System.Threading.Tasks;
using LOMSUI.Services;
using LOMSUI.Models;

namespace LOMSUI
{
    [Activity(Label = "Verify OTP")]
    public class VerifyOtpActivity : Activity
    {
        private EditText _otpEditText;
        private Button _verifyOtpButton;
        private string _email;
        private readonly ApiService _apiService = new ApiService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_verify_otp);

            _email = Intent.GetStringExtra("email");
            if (!ValidateInput(_email, "Error: Email is missing.")) return;

            _otpEditText = FindViewById<EditText>(Resource.Id.etOtp);
            _verifyOtpButton = FindViewById<Button>(Resource.Id.btnVerifyOtp);
            _verifyOtpButton.Click += async (sender, e) => await VerifyOtpAsync();
        }

        private async Task VerifyOtpAsync()
        {
            string otp = _otpEditText.Text.Trim();

            // Validate OTP format
            if (otp.Length != 6)
            {
                ShowToast("OTP code must be 6 digits.");
                return;
            }

            var request = new VerifyOtpModel
            {
                Email = _email,
                OtpCode = otp
            };

            Console.WriteLine($"Sending OTP Verification for Email: {_email}");

            bool isOtpValid = await _apiService.VerifyOtpAsync(request);

            RunOnUiThread(() =>
            {
                if (isOtpValid)
                {
                    ShowToast("Valid OTP! Please enter a new password.");
                    NavigateToActivity<ResetPasswordActivity>("email", _email, "otp", otp);
                }
                else
                {
                    ShowToast("OTP code is invalid or expired.");
                }
            });
        }

        
        private bool ValidateInput(string input, string errorMessage)
        {
            if (string.IsNullOrEmpty(input))
            {
                ShowToast(errorMessage);
                return false;
            }
            return true;
        }

        private void ShowToast(string message) => RunOnUiThread(() => Toast.MakeText(this, message, ToastLength.Short).Show());

        private void NavigateToActivity<T>(string key1, string value1, string key2 = null, string value2 = null) where T : Activity
        {
            var intent = new Intent(this, typeof(T));
            intent.PutExtra(key1, value1);
            if (!string.IsNullOrEmpty(key2) && !string.IsNullOrEmpty(value2))
                intent.PutExtra(key2, value2);
            StartActivity(intent);
            Finish();
        }
    }
}
