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
        private List<OrderByLiveStreamCustoemrModel> _allOrders = new List<OrderByLiveStreamCustoemrModel>();
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

            await LoadOrdersByTypeAsync();
        }

        private async Task LoadOrdersByTypeAsync()
        {
            string type = Intent.GetStringExtra("Type");

            switch (type)
            {
                case "ByCustomer":
                    _customerId = Intent.GetStringExtra("customerId");
                    if (!string.IsNullOrEmpty(_customerId))
                        await LoadOrdersAsync(() => _apiService.GetOrdersByCustomerIdAsync(_customerId));
                    break;

                case "ByLive":
                    _liveStreamId = Intent.GetStringExtra("LiveStreamID");
                    if (!string.IsNullOrEmpty(_liveStreamId))
                        await LoadOrdersAsync(() => _apiService.GetOrdersByLiveStreamIdAsync(_liveStreamId));
                    break;

                case "ByUser":
                    await LoadOrdersAsync(() => _apiService.GetListOrderByLiveStreamCustomerModelAsync());
                    break;
            }
        }

        private async Task LoadOrdersAsync(Func<Task<List<OrderByLiveStreamCustoemrModel>>> fetchOrdersFunc)
        {
            var orders = await fetchOrdersFunc.Invoke();
            _allOrders = orders ?? new List<OrderByLiveStreamCustoemrModel>();

            SetupOrderAdapter();
            _statusFilterHelper.SelectDefaultStatus("Pending");
        }

        private void SetupOrderAdapter()
        {
            _adapter = new OrderAdapter(this, _allOrders);
            _adapter.OnViewDetailClick += order =>
            {
                var intent = new Intent(this, typeof(OrderDetailActivity));
                //intent.PutExtra("OrderId", order.OrderID);
                intent.PutExtra("liveStreamCustoemrID", order.LiveStreamCustoemrID);

                StartActivityForResult(intent, 100);
            };
            _recyclerView.SetAdapter(_adapter);
        }

        private void FilterOrdersByStatus(string status, LinearLayout selectedLayout)
        {
            var filteredOrders = _allOrders
                .Where(o => o.OrderStatus == status)
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
                {
                    _ = LoadOrdersAsync(() => _apiService.GetOrdersByCustomerIdAsync(_customerId));
                }
                else if (!string.IsNullOrEmpty(_liveStreamId))
                {
                    _ = LoadOrdersAsync(() => _apiService.GetOrdersByLiveStreamIdAsync(_liveStreamId));
                }
                else
                {
                    _ = LoadOrdersAsync(() => _apiService.GetListOrderByLiveStreamCustomerModelAsync());
                }
            }
        }
    }

}
