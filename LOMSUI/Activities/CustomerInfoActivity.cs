﻿using Android.Content;
using Android.Views;
using Bumptech.Glide;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "Customer Information")]
    public class CustomerInfoActivity : BaseActivity
    {
        private ImageView _imgAvatar;
        private TextView _txtFacebookName;
        private EditText _etFullName, _etEmail, _etPhone, _etAddress;
        private Button _btnSave;
        private TextView txtOrderHistory;
        private ApiService _apiService;
        private string _customerId;
        private CustomerModel _customer;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_customer_info);

            _apiService = ApiServiceProvider.Instance; 

            _customerId = Intent.GetStringExtra("CustomerID");
            if (string.IsNullOrEmpty(_customerId))
            {
                Toast.MakeText(this, "Invalid CustomerID!", ToastLength.Short).Show();
                Finish();
                return;
            }

            InitViews();

            _customer = await _apiService.GetCustomerByIdAsync(_customerId);
            if (_customer != null)
            {
                LoadCustomer(_customer);
            }
        }
        private void InitViews()
        {
            _imgAvatar = FindViewById<ImageView>(Resource.Id.imgAvatar);
            _txtFacebookName = FindViewById<TextView>(Resource.Id.txtFacebookName);
            _etFullName = FindViewById<EditText>(Resource.Id.etFullName);
            _etEmail = FindViewById<EditText>(Resource.Id.etEmail);
            _etPhone = FindViewById<EditText>(Resource.Id.etPhoneNumber);
            _etAddress = FindViewById<EditText>(Resource.Id.etAddress);
            _btnSave = FindViewById<Button>(Resource.Id.btnSaveInfo);
            txtOrderHistory = FindViewById<TextView>(Resource.Id.txtOrderHistory);


            _btnSave.Click += async (s, e) => await SaveCustomerInfo();

            txtOrderHistory.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(OrderListActivity));
                intent.PutExtra("Type", "ByCustomer");
                intent.PutExtra("customerId", _customer.CustomerID);
                StartActivity(intent);
            };
        }

        private void LoadCustomer(CustomerModel c)
        {
            _txtFacebookName.Text = $"{c.FacebookName}";
            _etFullName.Text = c.FullName;
            _etEmail.Text = c.Email;
            _etPhone.Text = c.PhoneNumber;
            _etAddress.Text = c.Address;

            Glide.With(this).Load(c.ImageURL).Into(_imgAvatar);
        }

        private async Task SaveCustomerInfo()
        {
            _customer.FullName = _etFullName.Text;
            _customer.Email = _etEmail.Text;
            _customer.PhoneNumber = _etPhone.Text;
            _customer.Address = _etAddress.Text;

            bool success = await _apiService.UpdateCustomerAsync(_customerId, _customer);
            Toast.MakeText(this, success ? "Update successful!" : "Update failed!", ToastLength.Short).Show();

            if (success)
            {
                Finish();
            }
        }
    }

}
