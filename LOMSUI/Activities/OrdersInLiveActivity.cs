using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Services;
using System.Linq;
using System.Threading.Tasks;

namespace LOMSUI.Activities
{
    [Activity(Label = "Live Orders")]
    public class OrdersInLiveActivity : Activity
    {
        private RecyclerView _recyclerView;
        private TextView _txtNoOrders;
        private OrderHistoryAdapter _adapter;
        private ApiService _apiService;
        private string _liveStreamId;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_order_history);

            _liveStreamId = Intent.GetStringExtra("LiveStreamID");
            _apiService = new ApiService();
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewOrders);
            _txtNoOrders = FindViewById<TextView>(Resource.Id.txtNoOrders);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            await LoadOrders();
        }

        private async Task LoadOrders()
        {
            var orders = await _apiService.GetOrdersByLiveStreamIdAsync(_liveStreamId);
            if (orders == null || !orders.Any())
            {
                _txtNoOrders.Visibility = ViewStates.Visible;
                return;
            }

            _adapter = new OrderHistoryAdapter(this, orders);
            _adapter.OnViewDetailClick += order =>
            {
                // var intent = new Intent(this, typeof(OrderDetailActivity));
                // intent.PutExtra("orderId", order.OrderID);
                // StartActivity(intent);
            };

            _recyclerView.SetAdapter(_adapter);
        }
    }
}
