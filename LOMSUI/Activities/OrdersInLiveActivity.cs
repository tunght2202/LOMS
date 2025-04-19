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
    [Activity(Label = "LiveStream Orders")]
    public class OrdersInLiveActivity : BaseActivity
    {

        private List<OrderModel> _allOrders = new List<OrderModel>();
        private OrderStatusFilterHelper _statusFilterHelper;
        private LinearLayout _currentSelectedLayout;
        private RecyclerView _recyclerView;
        private TextView _txtNoOrders;
        private OrderAdapter _adapter;
        private ApiService _apiService;
        private string _liveStreamId;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_order);

            _apiService = ApiServiceProvider.Instance;

            _liveStreamId = Intent.GetStringExtra("LiveStreamID");
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewOrders);
            _txtNoOrders = FindViewById<TextView>(Resource.Id.txtNoOrders);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _statusFilterHelper = new OrderStatusFilterHelper(this, FilterOrdersByStatus);

            await LoadOrders();

        }

        private async Task LoadOrders()
        {
            var orders = await _apiService.GetOrdersByLiveStreamIdAsync(_liveStreamId);

            _allOrders = orders ?? new List<OrderModel>();

            _adapter = new OrderAdapter(this, new List<OrderModel>());
            _adapter.OnViewDetailClick += order =>
            {
                var intent = new Intent(this, typeof(OrderDetailActivity));
                intent.PutExtra("OrderId", order.OrderID);
                StartActivity(intent);
            };
            _recyclerView.SetAdapter(_adapter);

            _statusFilterHelper.SelectDefaultStatus("Pending"); 
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


    }
}
