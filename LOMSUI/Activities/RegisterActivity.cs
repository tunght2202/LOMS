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
        private Button _backButton;
        private Button _registerButton;
        private readonly ApiService _apiService = new ApiService();

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
            _genderSpinner = FindViewById<Spinner>(Resource.Id.genderSpinner);
            _backButton = FindViewById<Button>(Resource.Id.backButton);
            _registerButton = FindViewById<Button>(Resource.Id.registerButton);

            if (_usernameEditText == null || _phoneEditText == null || _emailEditText == null ||
                _passwordEditText == null || _confirmPasswordEditText == null || _genderSpinner == null ||
                _backButton == null || _registerButton == null)
            {
                Log.Error("RegisterActivity", "Error: One or more UI elements not found!");
                Toast.MakeText(this, "Error loading UI components!", ToastLength.Long).Show();
                return;
            }

            // Attach event listeners
            _registerButton.Click += RegisterButton_Click;
            _backButton.Click += BackButton_Click;

            // Populate gender spinner
            ArrayAdapter<string> genderAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Resources.GetStringArray(Resource.Array.gender_options));
            _genderSpinner.Adapter = genderAdapter;
        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            string username = _usernameEditText.Text?.Trim();
            string phone = _phoneEditText.Text?.Trim();
            string email = _emailEditText.Text?.Trim();
            string password = _passwordEditText.Text.Trim();
            string confirmPassword = _confirmPasswordEditText.Text.Trim();
            string gender = _genderSpinner.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(gender))
            {
                Toast.MakeText(this, "Please fill in all fields", ToastLength.Short).Show();
                return;
            }

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
                    Gender = gender
                };

                var result = await _apiService.RegisterAsync(registerModel);

                if (result)
                {
                    Toast.MakeText(this, "Registration successful!", ToastLength.Short).Show();

                    Intent intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "Registration failed!", ToastLength.Short).Show();
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