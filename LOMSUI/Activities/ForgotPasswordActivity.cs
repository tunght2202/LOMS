using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System.Threading.Tasks;
using LOMSUI.Services;
using LOMSUI.Models;
using LOMSUI.Activities;

namespace LOMSUI
{
    [Activity(Label = "Forgot Password")]
    public class ForgotPasswordActivity : BaseActivity
    {
        private EditText _emailEditText;
        private Button _sendOtpButton;
        private readonly ApiService _apiService = new ApiService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_forgot_password);

            _emailEditText = FindViewById<EditText>(Resource.Id.etEmail);
            _sendOtpButton = FindViewById<Button>(Resource.Id.btnSendOtp);

            _sendOtpButton.Click += async (sender, e) => await SendOtpAsync();
        }

        private async Task SendOtpAsync()
        {
            string email = _emailEditText.Text.Trim();
            if (!ValidateInput(email, "Please enter your email!")) return;

            var request = new ForgotPasswordModel { Email = email };
            if (await _apiService.RequestOtpAsync(request))
            {
                ShowToast("OTP sent to your email!");
                NavigateToActivity<VerifyOtpActivity>("email", email);
            }
            else
            {
                ShowToast("Failed to send OTP. Please check your email and try again.");
            }
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

        private void NavigateToActivity<T>(string key, string value) where T : Activity
        {
            var intent = new Intent(this, typeof(T));
            intent.PutExtra(key, value);
            StartActivity(intent);
            Finish();
        }
    }
}
