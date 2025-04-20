using Android.App;
using Android.OS;
using Android.Widget;
using LOMSUI.Adapter;
using LOMSUI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using LOMSUI.Models;
using Android.Views;
using Android.Content;

namespace LOMSUI.Activities
{
    [Activity(Label = "OrderActivity")]
    public class OrderActivity : Activity
    {
        private ListView _orderListView;
        private ApiService _apiService;
        private List<OrderModel> _orderList;
        private TextView _statusPendingTextView;
        private TextView _statusPickingTextView;
        private TextView _statusShippingTextView;
        private TextView _statusDeliveredTextView;
        private TextView _statusCancelledTextView;
        private TextView _statusReturnedTextView;

        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_list_order);

            _orderListView = FindViewById<ListView>(Resource.Id.orderListView);
            _apiService = new ApiService();

            _statusPendingTextView = FindViewById<TextView>(Resource.Id.statusPendingTextView);
            _statusPickingTextView = FindViewById<TextView>(Resource.Id.statusPickingTextView);
            _statusShippingTextView = FindViewById<TextView>(Resource.Id.statusShippingTextView);
            _statusDeliveredTextView = FindViewById<TextView>(Resource.Id.statusDeliveredTextView);
            _statusCancelledTextView = FindViewById<TextView>(Resource.Id.statusCancelledTextView);
            _statusReturnedTextView = FindViewById<TextView>(Resource.Id.statusReturnedTextView);

            BottomNavHelper.SetupFooterNavigation(this);

            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            string token = prefs.GetString("token", null);
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetToken(token);
            }

            // Load all orders 
            await LoadOrdersAsync();

            _statusPendingTextView.Click += (sender, e) => FilterOrders("Pending");
            _statusPickingTextView.Click += (sender, e) => FilterOrders("Confirmed"); 
            _statusShippingTextView.Click += (sender, e) => FilterOrders("Shipped");
            _statusDeliveredTextView.Click += (sender, e) => FilterOrders("Delivered");
            _statusCancelledTextView.Click += (sender, e) => FilterOrders("Canceled");
            _statusReturnedTextView.Click += (sender, e) => FilterOrders("Returned");
        }

        private async Task LoadOrdersAsync(string status = null)
        {
            List<OrderModel> orders = null;
            if (string.IsNullOrEmpty(status))
            {
                orders = await _apiService.GetUserOrdersAsync();
            }

            if (orders != null)
            {
                _orderList = orders;
                _orderListView.Adapter = new OrderAdapter(this, _orderList);
            }
            else
            {
                // no orders are returned
                Toast.MakeText(this, "Không có đơn hàng.", ToastLength.Short).Show();
                _orderListView.Adapter = null;
            }
        }

        private void FilterOrders(string status)
        {
            if (_orderList != null)
            {
                var filteredOrders = _orderList.Where(o => o.Status.ToLower() == status.ToLower()).ToList();
                _orderListView.Adapter = new OrderAdapter(this, filteredOrders);
            }
        }
    }
}