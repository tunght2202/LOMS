using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using LOMSAPI.Models;
using LOMSUI.Activities;
using LOMSUI.Models;
using LOMSUI.Services;
using System;

namespace LOMSUI
{
    [Activity(Label = "Livestream Details")]
    public class LiveStreamDetailActivity : BaseActivity
    {
        private TextView _txtTitle, _txtStatus, _txtStartTime;
        private Button _btnViewComments, _btnViewCustomers,
                       _btnViewOrders, _btnSetupListProduct, _tvRevenusLive;
        private ToggleButton _toggleAutoCreateOrder;
        private bool _isAutoCreating = false;
        private CancellationTokenSource _cancellationTokenSource;

        private Spinner _spinnerListProduct;
        private string _liveStreamId;
        private string _title;
        private string _status;
        private string _startTime;
        private  ApiService _apiService = new ApiService();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_livestream_detail);

            //BottomNavHelper.SetupFooterNavigation(this);

            _txtTitle = FindViewById<TextView>(Resource.Id.txtLiveTitle);
            _txtStatus = FindViewById<TextView>(Resource.Id.txtLiveStatus);
            _txtStartTime = FindViewById<TextView>(Resource.Id.txtLiveStartTime);
            _tvRevenusLive = FindViewById<Button>(Resource.Id.tvRevenueLive);
            _btnSetupListProduct = FindViewById<Button>(Resource.Id.btnSetupListProduct);
            _toggleAutoCreateOrder = FindViewById<ToggleButton>(Resource.Id.toggleAutoOrder);
            _spinnerListProduct = FindViewById<Spinner>(Resource.Id.spinnerListProduct);
            _btnViewComments = FindViewById<Button>(Resource.Id.btnViewComments);
            _btnViewCustomers = FindViewById<Button>(Resource.Id.btnViewCustomers);
            _btnViewOrders = FindViewById<Button>(Resource.Id.btnViewOrders);


            _apiService = ApiServiceProvider.Instance;


            _liveStreamId = Intent.GetStringExtra("LiveStreamID");
            _title = Intent.GetStringExtra("Title");
            _status = Intent.GetStringExtra("Status");
            _startTime = Intent.GetStringExtra("StartTime");

            _txtTitle.Text = _title;
            _txtStatus.Text = $"Status: {_status}";
            _txtStartTime.Text = $"Start: {_startTime}";

            await CheckLiveStreamStatusAndUpdateUIAsync();
            await LoadListProducts();

            _btnSetupListProduct.Click += async (s, e) => await SetupListProduct();

            _toggleAutoCreateOrder.CheckedChange += async (s, e) =>
            {
                bool hasListProduct = await _apiService.CheckListProductExistsAsync(_liveStreamId);

                if (!hasListProduct)
                {
                    Toast.MakeText(this, "Product list not set up for livestream!", ToastLength.Long).Show();
                    _toggleAutoCreateOrder.Checked = false;
                    return;
                }

                if (e.IsChecked)        
                {
                    _isAutoCreating = true;
                    _cancellationTokenSource = new CancellationTokenSource();
                    StartAutoCreateLoop(_cancellationTokenSource.Token);
                    Toast.MakeText(this, "Started automatic order creation!", ToastLength.Short).Show();
                }
                else
                {
                    _isAutoCreating = false;
                    _cancellationTokenSource?.Cancel();
                    Toast.MakeText(this, "Stopped automatic order creation.", ToastLength.Short).Show();
                }
            };

            _tvRevenusLive.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(LiveStreamRevenueActivity));
                intent.PutExtra("LiveStreamID", _liveStreamId);
                StartActivity(intent);
            };
            
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
                var intent = new Intent(this, typeof(OrderListActivity));
                intent.PutExtra("Type", "ByLive");  
                intent.PutExtra("LiveStreamID", _liveStreamId);
                StartActivity(intent);
            };

        }

        private async Task LoadListProducts()
        {
            try
            {
                var listProducts = await _apiService.GetListProductsAsync();

                var displayList = new List<string> { "Not Select" };
                displayList.AddRange(listProducts.Select(lp => lp.ListProductName));

                var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, displayList);
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
                int listProductId = 0;
                    
                if (selectedIndex > 0)
                {
                    var listProducts = await _apiService.GetListProductsAsync();
                    listProductId = listProducts[selectedIndex - 1].ListProductId;
                }

                var success = await _apiService.SetupListProductAsync(_liveStreamId, listProductId);

                Toast.MakeText(this, success ? "ListProduct setup successfully!" : "Failed to setup ListProduct.", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error setting up ListProduct: " + ex.Message, ToastLength.Long).Show();
            }
        }


        private async void StartAutoCreateLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {

                    bool isLive = await _apiService.IsLiveStreamStillLiveAsync(_liveStreamId);

                    if (!isLive)
                    {
                        RunOnUiThread(() =>
                        {
                            _isAutoCreating = false;
                            _cancellationTokenSource?.Cancel();
                            _toggleAutoCreateOrder.Checked = false;
                            _toggleAutoCreateOrder.Visibility = ViewStates.Gone;
                        });
                        break;
                    }

                    var (isSuccess, message) = await _apiService.CreateOrdersFromCommentsAsync(_liveStreamId);

                    if (isSuccess)
                    {
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, message, ToastLength.Short).Show();
                        });
                    }

                    await Task.Delay(5000, token); 
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Auto creation stopped."); 
            }
            catch (Exception ex)
            {
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Long).Show();
                });
            }
        }

        private async Task CheckLiveStreamStatusAndUpdateUIAsync()
        {
            bool isLive = await _apiService.IsLiveStreamStillLiveAsync(_liveStreamId);

            RunOnUiThread(() =>
            {
                if (isLive)
                {
                    _toggleAutoCreateOrder.Visibility = ViewStates.Visible;
                    _toggleAutoCreateOrder.Enabled = true;
                }
                else
                {
                    if (_isAutoCreating)
                    {
                        _isAutoCreating = false;
                        _cancellationTokenSource?.Cancel();
                        _toggleAutoCreateOrder.Checked = false;
                    }
                    _toggleAutoCreateOrder.Visibility = ViewStates.Gone;
                }
            });
        }

    }
}
