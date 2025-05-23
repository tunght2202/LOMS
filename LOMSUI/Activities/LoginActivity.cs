﻿using Android.App;
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
using LOMSUI;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LOMSUI
{
    [Activity(Label = "LOMS", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        private AutoCompleteTextView _emailEditText;
        private EditText _passwordEditText;
        private Button _loginButton;
        private Button _registerButton;
        private CheckBox cbRememberMe;
        private List<string> emailList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            _emailEditText = FindViewById<AutoCompleteTextView>(Resource.Id.etEmail);
            _passwordEditText = FindViewById<EditText>(Resource.Id.etPassword);
            _loginButton = FindViewById<Button>(Resource.Id.btnLogin);
            _registerButton = FindViewById<Button>(Resource.Id.btnRegister);
            cbRememberMe = FindViewById<CheckBox>(Resource.Id.cbRememberMe);

            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            bool rememberMe = prefs.GetBoolean("rememberMe", false);
            emailList = prefs.GetStringSet("emailList", new HashSet<string>()).ToList();

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, emailList);
            _emailEditText.Adapter = adapter;

            string lastEmail = prefs.GetString("email", "");
            string lastPassword = prefs.GetString($"password_{lastEmail}", "");

            if (rememberMe && !string.IsNullOrEmpty(lastEmail))
            {
                _emailEditText.Text = lastEmail;
                _passwordEditText.Text = lastPassword;
                cbRememberMe.Checked = true;
            }


            _emailEditText.ItemClick += (s, e) =>
            {
                string selectedEmail = _emailEditText.Text?.Trim();

                var freshPrefs = GetSharedPreferences("auth", FileCreationMode.Private);
                string savedPassword = freshPrefs.GetString($"password_{selectedEmail}", "");

                _passwordEditText.Text = savedPassword;
                cbRememberMe.Checked = !string.IsNullOrEmpty(savedPassword);
            };


            TextView forgotPasswordTextView = FindViewById<TextView>(Resource.Id.tvForgotPassword);
            if (forgotPasswordTextView != null)
            {
                forgotPasswordTextView.Click += (sender, e) =>
                {
                    StartActivity(new Intent(this, typeof(ForgotPasswordActivity)));
                };
            }

            _loginButton.Click += LoginButton_Click;
            _registerButton.Click += (s, e) =>
            {
                StartActivity(new Intent(this, typeof(RegisterActivity)));
            };

            _passwordEditText.EditorAction += (s, e) =>
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
            string password = _passwordEditText.Text?.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Please enter email and password!", ToastLength.Short).Show();
                return;
            }

            try
            {
                var loginModel = new LoginModel { Email = email, Password = password };

                string token = await ApiServiceProvider.Instance.LoginAsync(loginModel);

                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    string userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                    ApiServiceProvider.SetToken(token);

                    var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
                    var editor = prefs.Edit();
                    editor.PutString("token", token);
                    editor.PutString("userID", userId);

                    // Lưu riêng password cho từng email
                    editor.PutString($"password_{email}", password);

                    if (cbRememberMe.Checked)
                    {
                        editor.PutBoolean("rememberMe", true);
                        editor.PutString("email", email);
                        editor.PutString("password", password);

                        var updatedEmailSet = prefs.GetStringSet("emailList", new HashSet<string>()).ToList();
                        if (!updatedEmailSet.Contains(email))
                            updatedEmailSet.Add(email);

                        editor.PutStringSet("emailList", updatedEmailSet.ToHashSet());
                    }
                    else
                    {
                        editor.Remove("rememberMe");
                        editor.Remove("email");
                        editor.Remove("password");
                        editor.Remove($"password_{email}");

                    }

                    editor.Apply();

                    Toast.MakeText(this, "Login successful!", ToastLength.Short).Show();
                    StartActivity(new Intent(this, typeof(HomePageActivity)));
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
