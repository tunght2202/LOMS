using Android.Views;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "OrderDetail")]
    public class OrderDetailActivity : BaseActivity
    {
        private TextView _txtOrderCode, _txtOrderDate, 
                         _txtTotalPrice, _txtOrderStatus,
                         _txtProductName, _txtProductPrice,
                         _txtOrderQuantity, _txtCustomerName,
                          _txtAddress, _txPhoneNumber;
        private CheckBox _cbCheck;
        private EditText _edtTrackingNumber, _edtNote;
        private Button _btnStatusCancel, _btnStatusConfirmed, _btnStatusCancell,
                   _btnStatusShipped, _btnStatusReturn, _btnStatusDelivered,
                   _btnSetStatusCheck, _btnUpdateOrder;
        private LinearLayout _layoutPending, _layoutConfirm, _layoutShipped,
                              _layoutTracking, _layoutNote;
        private OrderModel _currentOrder;
        private ApiService _apiService;
        private int _orderId;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_order_detail);

            _apiService = ApiServiceProvider.Instance;

            _orderId = Intent.GetIntExtra("OrderId", -1);
            if (_orderId == -1)
            {
                Toast.MakeText(this, "Invalid Order ID", ToastLength.Long).Show();
                Finish();
                return;
            }
            InitViews();
            InitButtonEvents();
            await LoadOrderDetails();

            _btnSetStatusCheck.Click += async (s, e) =>
            {
                bool newStatusCheck = _cbCheck.Checked;

                var success = await _apiService.UpdateStatusCheckOrderAsync(_currentOrder.OrderID, newStatusCheck);
                if (success)
                {
                    Toast.MakeText(this, "Updated check status successfully", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Failed to update check status", ToastLength.Short).Show();
                }
            };
            _btnUpdateOrder.Click += async (sender, e) =>
            {
                await UpdateOrderTrackingAndNote();
            };

        }

        private void InitViews()
        {
            _txtCustomerName = FindViewById<TextView>(Resource.Id.txtCustomerName);
            _txtAddress = FindViewById<TextView>(Resource.Id.txtAddress);
            _txPhoneNumber = FindViewById<TextView>(Resource.Id.txPhoneNumber);

            _txtOrderCode = FindViewById<TextView>(Resource.Id.txtOrderCode);
            _txtOrderDate = FindViewById<TextView>(Resource.Id.txtOrderDate);
            _txtOrderQuantity = FindViewById<TextView>(Resource.Id.txtOrderQuantity);
            _txtTotalPrice = FindViewById<TextView>(Resource.Id.txtTotalPrice);
            _txtOrderStatus = FindViewById<TextView>(Resource.Id.txtOrderStatus);
            _txtProductName = FindViewById<TextView>(Resource.Id.txtProductName);
            _txtProductPrice = FindViewById<TextView>(Resource.Id.txtProductPrice);

            _edtTrackingNumber = FindViewById<EditText>(Resource.Id.edtTrackingNumber);
            _edtNote = FindViewById<EditText>(Resource.Id.edtNote);
            _cbCheck =  FindViewById<CheckBox>(Resource.Id.cbCheck);
            _btnSetStatusCheck = FindViewById<Button>(Resource.Id.btnSetStatusCheck);
            _btnUpdateOrder =  FindViewById<Button>(Resource.Id.btnUpdateOrder);
            _layoutTracking = FindViewById<LinearLayout>(Resource.Id.layoutTracking);
            _layoutNote = FindViewById<LinearLayout>(Resource.Id.layoutNote);

            _layoutPending = FindViewById<LinearLayout>(Resource.Id.layoutPending);
            _layoutConfirm = FindViewById<LinearLayout>(Resource.Id.layoutConfirm);
            _layoutShipped = FindViewById<LinearLayout>(Resource.Id.layoutShipped);

            _btnStatusCancel = FindViewById<Button>(Resource.Id.btnStatusCancel);
            _btnStatusConfirmed = FindViewById<Button>(Resource.Id.btnStatusComfimed);
            _btnStatusCancell = FindViewById<Button>(Resource.Id.btnStatusCancell);
            _btnStatusShipped = FindViewById<Button>(Resource.Id.btnStatusShipped);
            _btnStatusReturn = FindViewById<Button>(Resource.Id.btnStatusReturn);
            _btnStatusDelivered = FindViewById<Button>(Resource.Id.btnStatusDelivered);
        }

        private void InitButtonEvents()
        {
            _btnStatusCancel.Click += async (s, e) => await UpdateOrderStatus(OrderStatus.Canceled);
            _btnStatusConfirmed.Click += async (s, e) => await UpdateOrderStatus(OrderStatus.Confirmed);

            _btnStatusCancell.Click += async (s, e) => await UpdateOrderStatus(OrderStatus.Canceled);
            _btnStatusShipped.Click += async (s, e) => await UpdateOrderStatus(OrderStatus.Shipped);

            _btnStatusReturn.Click += async (s, e) => await UpdateOrderStatus(OrderStatus.Returned);
            _btnStatusDelivered.Click += async (s, e) => await UpdateOrderStatus(OrderStatus.Delivered);
        }

        private async Task LoadOrderDetails()
        {
            var order = await _apiService.GetOrderByIdAsync(_orderId);
            if (order == null)
            {
                Toast.MakeText(this, "Failed to fetch order details", ToastLength.Long).Show();
                return;
            }

            _currentOrder = order;

            _txtCustomerName.Text = "Customer : " + order.FacebookName;
            _txtAddress.Text = "Address : " + order.Address;
            _txPhoneNumber.Text = "Phone : " + order.PhoneNumber;
            _cbCheck.Checked = order.StatusCheck;
            _edtTrackingNumber.Text = order.TrackingNumber;
            _edtNote.Text = order.Note;
            _txtOrderCode.Text = "Order code: " + order.OrderID;
            _txtOrderDate.Text = "Order date: " + order.OrderDate;
            _txtOrderQuantity.Text =  "Quantity: " + order.Quantity;
            _txtTotalPrice.Text =$"TotalPrice: {order.Quantity * order.CurrentPrice:n0}đ";
            _txtOrderStatus.Text = "Status: " + order.Status;
            _txtProductName.Text = "Product Name: " + order.Product.Name;
            _txtProductPrice.Text =$"Price: {order.CurrentPrice:n0}đ";

            UpdateStatusLayout(order.Status); 

        }

        private void UpdateStatusLayout(string status)
        {
            _layoutPending.Visibility = ViewStates.Gone;
            _layoutConfirm.Visibility = ViewStates.Gone;
            _layoutShipped.Visibility = ViewStates.Gone;

            switch (status)
            {
                case "Pending":
                    _layoutPending.Visibility = ViewStates.Visible;
                    _layoutTracking.Visibility = ViewStates.Gone;
                    _layoutNote.Visibility = ViewStates.Gone;
                    _btnSetStatusCheck.Visibility = ViewStates.Visible;
                    _btnSetStatusCheck.Enabled = true;
                    _cbCheck.Enabled = true;
                    break;
                case "Confirmed":
                    _layoutConfirm.Visibility = ViewStates.Visible;
                    _btnUpdateOrder.Visibility = ViewStates.Visible;
                    _btnUpdateOrder.Enabled = true;
                    _edtNote.Enabled = true;
                    _edtTrackingNumber.Enabled = true;
                    break;
                case "Shipped":
                    _layoutShipped.Visibility = ViewStates.Visible;
                    break;
            }
        }

        private async Task UpdateOrderStatus(OrderStatus newStatus)
        {
            try
            {
                var success = await _apiService.UpdateOrderStatusAsync(_orderId, newStatus);
                if (success)
                {
                    Toast.MakeText(this, "Order status updated", ToastLength.Short).Show();
                    SetResult(Result.Ok);
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        private async Task UpdateOrderTrackingAndNote()
        {
            var tracking = _edtTrackingNumber.Text?.Trim();
            var note = _edtNote.Text?.Trim();

            var model = new OrderModelRequest
            {
                OrderID = _orderId,
                TrackingNumber = tracking,
                Note = note
            };

            try
            {
                var success = await _apiService.UpdateOrderTrackingAndNoteAsync(_orderId,model);
                if (success)
                {
                    Toast.MakeText(this, "Order infomation updated", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

    }
}
