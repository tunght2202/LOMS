using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "OrderDetail")]
    public class OrderDetailActivity : Activity
    {
        private TextView _txtOrderCode, _txtOrderDate, 
                         _txtTotalPrice, _txtOrderStatus,
                         _txtProductName, _txtProductPrice, _txtOrderQuantity;
        private ApiService _apiService = new ApiService();
        private int _orderId;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_order_detail);

            _orderId = Intent.GetIntExtra("OrderId", -1);
            if (_orderId == -1)
            {
                Toast.MakeText(this, "Invalid Order ID", ToastLength.Long).Show();
                Finish();
                return;
            }

            _txtOrderCode = FindViewById<TextView>(Resource.Id.txtOrderCode);
            _txtOrderDate = FindViewById<TextView>(Resource.Id.txtOrderDate);
            _txtOrderQuantity = FindViewById<TextView>(Resource.Id.txtOrderQuantity);
            _txtTotalPrice = FindViewById<TextView>(Resource.Id.txtTotalPrice);
            _txtOrderStatus = FindViewById<TextView>(Resource.Id.txtOrderStatus);
            _txtProductName = FindViewById<TextView>(Resource.Id.txtProductName);
            _txtProductPrice = FindViewById<TextView>(Resource.Id.txtProductPrice);

            await LoadOrderDetails();
        }

        private async Task LoadOrderDetails()
        {
            var order = await _apiService.GetOrderByIdAsync(_orderId);
            if (order == null)
            {
                Toast.MakeText(this, "Failed to fetch order details", ToastLength.Long).Show();
                return;
            }
            _txtOrderCode.Text = "Order code: " + order.OrderID;
            _txtOrderDate.Text = "Order date: " + order.OrderDate;
            _txtOrderQuantity.Text =  "Quantity: " + order.Quantity;
            _txtTotalPrice.Text =$"Price: {order.Quantity * order.Product.Price:n0}đ";
            _txtOrderStatus.Text = "Status: " + order.Status;
            _txtProductName.Text = "Product Name: " + order.Product.Name;
            _txtTotalPrice.Text =$"Price: {order.Quantity * order.Product.Price:n0}đ";
        }
    }
}
