using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System.Threading.Tasks;
using LOMSUI.Services;
using LOMSUI.Models;

namespace LOMSUI
{
    [Activity(Label = "Reset Password")]
    public class ResetPasswordActivity : Activity
    {
        private EditText _newPasswordEditText;
        private Button _resetPasswordButton;
        private string _email, _otp;
        private readonly ApiService _apiService = new ApiService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_reset_password);

            _email = Intent.GetStringExtra("email");
            _otp = Intent.GetStringExtra("otp");
            if (!ValidateInput(_email, "Error: Email is missing.") || !ValidateInput(_otp, "Error: OTP is missing.")) return;

            _newPasswordEditText = FindViewById<EditText>(Resource.Id.etNewPassword);
            _resetPasswordButton = FindViewById<Button>(Resource.Id.btnResetPassword);
            _resetPasswordButton.Click += async (sender, e) => await ResetPasswordAsync();
        }

        private async Task ResetPasswordAsync()
        {
            string newPassword = _newPasswordEditText.Text.Trim();
            if (!ValidateInput(newPassword, "Please enter a new password!")) return;

            if (newPassword.Length < 6)
            {
                _newPasswordEditText.Error = "Password must be at least 6 characters long!";
                return;
            }

            var request = new ResetPasswordModel { Email = _email, NewPassword = newPassword };
            if (await _apiService.ResetPasswordAsync(request))
            {
                ShowToast("Password reset successful!");
                Finish();
            }
            else
            {
                ShowToast("Failed to reset password!");
            }
        }

        private bool ValidateInput(string input, string errorMessage)
        {
            if (string.IsNullOrEmpty(input))
            {
                ShowToast(errorMessage);
                Finish();
                return false;
            }
            return true;
        }

        private void ShowToast(string message) => RunOnUiThread(() => Toast.MakeText(this, message, ToastLength.Short).Show());
    }
}
