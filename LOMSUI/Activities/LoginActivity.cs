using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Util;
using System;
using System.Threading.Tasks;
using LOMSUI.Models;
using LOMSUI.Services;
using LOMSUI.Activities;

namespace LOMSUI
{
    [Activity(Label = "Login")]
    public class LoginActivity : Activity
    {
        private EditText _emailEditText;
        private EditText _passwordEditText;
        private Button _loginButton;
        private readonly ApiService _apiService = new ApiService();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            // Find UI elements
            _emailEditText = FindViewById<EditText>(Resource.Id.etEmail);
            _passwordEditText = FindViewById<EditText>(Resource.Id.etPassword);
            _loginButton = FindViewById<Button>(Resource.Id.btnLogin);

            if (_emailEditText == null || _passwordEditText == null || _loginButton == null)
            {
                Log.Error("LoginActivity", "Error: One or more UI elements not found!");
                Toast.MakeText(this, "Error loading UI components!", ToastLength.Long).Show();
                return;
            }

            TextView forgotPasswordTextView = FindViewById<TextView>(Resource.Id.tvForgotPassword);

            if (forgotPasswordTextView == null)
            {
                Log.Error("LoginActivity", "Error: Forgot Password UI element not found!");
            }
            else
            {
                forgotPasswordTextView.Click += (sender, e) =>
                {
                    Log.Info("LoginActivity", "Forgot Password clicked!");
                    Intent intent = new Intent(this, typeof(ForgotPasswordActivity));
                    StartActivity(intent);
                };
            }
            // Attach event listeners
            _loginButton.Click += LoginButton_Click;
            _passwordEditText.EditorAction += (sender, e) =>
            {
                if (e.ActionId == Android.Views.InputMethods.ImeAction.Done)
                {
                    _loginButton.PerformClick(); 
                }
            };


        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string email = _emailEditText.Text?.Trim();
            string password = _passwordEditText.Text.Trim();


            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Please enter email and password!", ToastLength.Short).Show();
                return;
            }

            try
            {
                var loginModel = new LoginModel { Email = email, Password = password };
                var result = await _apiService.LoginAsync(loginModel);

                if (result)
                {
                    Toast.MakeText(this, "Login successful!", ToastLength.Short).Show();

                    Intent intent = new Intent(this, typeof(CommentsActivity));
                    StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "Incorrect email or password!", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }
}