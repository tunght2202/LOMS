using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using LOMSUI.Activities;
using LOMSUI.Models;
using LOMSUI.Services;
using System;

namespace LOMSUI
{
    [Activity(Label = "Livestream Details")]
    public class LiveStreamDetailActivity : Activity
    {
        private TextView _txtTitle, _txtStatus, _txtStartTime;
        private Button _btnViewComments, _btnViewCustomers, _btnViewOrders, _btnSetupListProduct;
        private Spinner _spinnerListProduct;

        private string _liveStreamId;
        private string _title;
        private string _status;
        private string _startTime;
        private string _token;
        private  ApiService _apiService = new ApiService();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_livestream_detail);

            BottomNavHelper.SetupFooterNavigation(this);

            _txtTitle = FindViewById<TextView>(Resource.Id.txtLiveTitle);
            _txtStatus = FindViewById<TextView>(Resource.Id.txtLiveStatus);
            _txtStartTime = FindViewById<TextView>(Resource.Id.txtLiveStartTime);
            _btnSetupListProduct = FindViewById<Button>(Resource.Id.btnSetupListProduct);
            _spinnerListProduct = FindViewById<Spinner>(Resource.Id.spinnerListProduct);
            _btnViewComments = FindViewById<Button>(Resource.Id.btnViewComments);
            _btnViewCustomers = FindViewById<Button>(Resource.Id.btnViewCustomers);
            _btnViewOrders = FindViewById<Button>(Resource.Id.btnViewOrders);


            _liveStreamId = Intent.GetStringExtra("LiveStreamID");
            _apiService = ApiServiceProvider.Instance;

            _token = ApiServiceProvider.Token;


            _liveStreamId = Intent.GetStringExtra("LiveStreamID");
            _title = Intent.GetStringExtra("Title");
            _status = Intent.GetStringExtra("Status");
            _startTime = Intent.GetStringExtra("StartTime");

            _txtTitle.Text = _title;
            _txtStatus.Text = $"Status: {_status}";
            _txtStartTime.Text = $"Start: {_startTime}";

            await LoadListProducts();

            _btnSetupListProduct.Click += async (s, e) => await SetupListProduct();


            _btnViewComments.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(CommentsActivity));
                intent.PutExtra("LivestreamID", _liveStreamId);
                StartActivity(intent);
            };

            _btnViewCustomers.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(CustomerListLiveActivity));
                intent.PutExtra("LiveStreamID", _liveStreamId);
                StartActivity(intent);
            };

            _btnViewOrders.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(OrdersInLiveActivity));
                intent.PutExtra("LiveStreamID", _liveStreamId);
                StartActivity(intent);
            };

        }

        private async Task LoadListProducts()
        {
            try
            {
                var listProducts = await _apiService.GetListProductsAsync(_token);

                var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, listProducts.Select(lp => lp.ListProductName).ToList());
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                _spinnerListProduct.Adapter = adapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error loading products: " + ex.Message, ToastLength.Long).Show();
            }
        }

        private async Task SetupListProduct()
        {
            int selectedIndex = _spinnerListProduct.SelectedItemPosition;

            if (selectedIndex < 0)
            {
                Toast.MakeText(this, "Please select a List Product.", ToastLength.Short).Show();
                return;
            }

            try
            {
                var listProducts = await _apiService.GetListProductsAsync(_token);
                var selectedListProduct = listProducts[selectedIndex];

                var success = await _apiService.SetupListProductAsync(_liveStreamId, selectedListProduct.ListProductId);

                Toast.MakeText(this, success ? "ListProduct setup successfully!" : "Failed to setup ListProduct.", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error setting up ListProduct: " + ex.Message, ToastLength.Long).Show();
            }
        }

    }
}
