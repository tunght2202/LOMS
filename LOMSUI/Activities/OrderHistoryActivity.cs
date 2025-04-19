using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "OrderHistory")]
    public class OrderHistoryActivity : BaseActivity
    {
        private RecyclerView _recyclerView;
        private TextView _txtNoOrders;
        private OrderHistoryAdapter _adapter;
        private ApiService _apiService;
        private string _customerId;

        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_order_history);

            _apiService = ApiServiceProvider.Instance;

            _customerId = Intent.GetStringExtra("customerId");

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewOrders);
            _txtNoOrders = FindViewById<TextView>(Resource.Id.txtNoOrders);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            await LoadOrders();

        }
        private async Task LoadOrders()
        {
            var orders = await _apiService.GetOrdersByCustomerIdAsync(_customerId);
            if (orders == null || !orders.Any())
            {
                _txtNoOrders.Visibility = ViewStates.Visible;
                return;
            }

            _adapter = new OrderHistoryAdapter(this, orders);
            _adapter.OnViewDetailClick += order =>
            {
                var intent = new Intent(this, typeof(OrderDetailActivity));
                intent.PutExtra("OrderId", order.OrderID);
                StartActivity(intent);
            };

            _recyclerView.SetAdapter(_adapter);
        }
    }
}

