using Android.Content;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "Verify Otp")]
    public class VerifyOtpUpdateUserActivity : Activity
    {
        private EditText _otpEditText;
        private Button _verifyOtpButton;
        private string _email;
        private string _token;
        private readonly ApiService _apiService = new ApiService();

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_verify_otp);

            InitViews();

            _email = Intent.GetStringExtra("email");
            if (string.IsNullOrEmpty(_email))
            {
                Toast.MakeText(this, "Email not found, please try again.", ToastLength.Long).Show();
                Finish();
                return;
            }

            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            _token = prefs.GetString("token", null);

            if (string.IsNullOrEmpty(_token))
            {
                Toast.MakeText(this, "Token not found, please login again.", ToastLength.Long).Show();
                Finish();
                return;
            }

            _verifyOtpButton.Click += async (s, e) => await VerifyOtp();
        }

        private void InitViews()
        {
            _otpEditText = FindViewById<EditText>(Resource.Id.etOtp);
            _verifyOtpButton = FindViewById<Button>(Resource.Id.btnVerifyOtp);
        }

        private async Task VerifyOtp()
        {
            string otpCode = _otpEditText.Text.Trim();

            if (string.IsNullOrEmpty(otpCode))
            {
                Toast.MakeText(this, "Please enter OTP.", ToastLength.Short).Show();
                return;
            }

            try
            {
                var success = await _apiService.VerifyOtpAndUpdateProfileAsync(new VerifyOtpModel
                {
                    Email = _email, 
                    OtpCode = otpCode
                }, _token);
                if (success)
                {
                    Toast.MakeText(this, "Email verified and profile updated successfully.", ToastLength.Long).Show();
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Invalid or expired OTP.", ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error verifying OTP: " + ex.Message, ToastLength.Long).Show();
            }
        }

    }

}
