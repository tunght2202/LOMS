using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Helpers;
using LOMSUI.Models;
using LOMSUI.Services;


namespace LOMSUI.Activities
{
    [Activity(Label = "Orders")]
    public class OrderListActivity : BaseActivity
    {

        private List<OrderModel> _allOrders = new List<OrderModel>();
        private OrderStatusFilterHelper _statusFilterHelper;
        private LinearLayout _currentSelectedLayout;
        private RecyclerView _recyclerView;
        private TextView _txtNoOrders;
        private OrderAdapter _adapter;
        private ApiService _apiService;

        private string _liveStreamId;
        private string _customerId;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_order);

            _apiService = ApiServiceProvider.Instance;

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewOrders);
            _txtNoOrders = FindViewById<TextView>(Resource.Id.txtNoOrders);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _statusFilterHelper = new OrderStatusFilterHelper(this, FilterOrdersByStatus);

            await LoadOrdersByType();

        }

        private async Task LoadOrdersByType()
        {
            var type = Intent.GetStringExtra("Type");

            switch (type)
            {
                case "ByCustomer":
                    _customerId = Intent.GetStringExtra("customerId");
                    if (!string.IsNullOrEmpty(_customerId))
                        await LoadOrdersByCustomerId(_customerId);
                    break;

                case "ByLive":
                    _liveStreamId = Intent.GetStringExtra("LiveStreamID");
                    if (!string.IsNullOrEmpty(_liveStreamId))
                        await LoadOrdersByLiveStreamId(_liveStreamId);
                    break;

                case "ByUser":
                    await LoadOrdersByUserId();
                    break;
            }
        }

        private async Task LoadOrdersByUserId()
        {
            var orders = await _apiService.GetOrdersByUserIdAsync();
            _allOrders = orders ?? new List<OrderModel>();

            SetupOrderAdapter();
            _statusFilterHelper.SelectDefaultStatus("Pending");
        }

        private async Task LoadOrdersByLiveStreamId(string livestreamId)
        {
            var orders = await _apiService.GetOrdersByLiveStreamIdAsync(livestreamId);
            _allOrders = orders ?? new List<OrderModel>();

            SetupOrderAdapter();
            _statusFilterHelper.SelectDefaultStatus("Pending");
        }

        private async Task LoadOrdersByCustomerId(string customerId)
        {
            var orders = await _apiService.GetOrdersByCustomerIdAsync(customerId);
            _allOrders = orders ?? new List<OrderModel>();

            SetupOrderAdapter();
            _statusFilterHelper.SelectDefaultStatus("Pending");
        }

        private void SetupOrderAdapter()
        {
            _adapter = new OrderAdapter(this, new List<OrderModel>());
            _adapter.OnViewDetailClick += order =>
            {
                var intent = new Intent(this, typeof(OrderDetailActivity));
                intent.PutExtra("OrderId", order.OrderID);
                StartActivityForResult(intent, 100);
            };
            _recyclerView.SetAdapter(_adapter);
        }

        private void FilterOrdersByStatus(string status, LinearLayout selectedLayout)
        {
            var filteredOrders = (_allOrders ?? new List<OrderModel>())
                                 .Where(o => o.Status == status)
                                 .ToList();

            _adapter?.UpdateData(filteredOrders);
            _txtNoOrders.Visibility = filteredOrders.Any() ? ViewStates.Gone : ViewStates.Visible;

            if (_currentSelectedLayout != null)
                _currentSelectedLayout.SetBackgroundResource(Resource.Drawable.status_tab_background);

            selectedLayout.SetBackgroundResource(Resource.Drawable.status_tab_selected);
            _currentSelectedLayout = selectedLayout;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 100 && resultCode == Result.Ok)
            {
                if (!string.IsNullOrEmpty(_customerId))
                    _ = LoadOrdersByCustomerId(_customerId);
                else if (!string.IsNullOrEmpty(_liveStreamId))
                    _ = LoadOrdersByLiveStreamId(_liveStreamId);
            }
        }


    }
}
