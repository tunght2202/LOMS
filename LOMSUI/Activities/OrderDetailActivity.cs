using Android.Views;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Models;
using LOMSUI.Services;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace LOMSUI.Activities
{
    [Activity(Label = "OrderDetail")]
    public class OrderDetailActivity : BaseActivity
    {
        private TextView _txtOrderDate,
                         _txtTotalPrice, _txtOrderStatus,
                         _txtCustomerName,
                          _txtAddress, _txtPhoneNumber;
        private RecyclerView _recyclerView;
        private CheckBox _cbCheck;
        private EditText _edtTrackingNumber, _edtNote;
        private Button _btnStatusCancel, _btnStatusConfirmed, _btnStatusCancell,
                   _btnStatusShipped, _btnStatusReturn, _btnStatusDelivered,
                   _btnSetStatusCheck, _btnUpdateOrder;
        private LinearLayout _layoutPending, _layoutConfirm, _layoutShipped,
                              _layoutTracking, _layoutNote;
        private ApiService _apiService;
        private int _liveStreamCustomerID;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_order_detail);

            _apiService = ApiServiceProvider.Instance;

            _liveStreamCustomerID = Intent.GetIntExtra("liveStreamCustoemrID", -1);

            InitViews();
            InitButtonEvents();
            await LoadOrderDetails();


            _btnSetStatusCheck.Click += async (s, e) =>
            {
                bool newStatusCheck = _cbCheck.Checked;

                var success = await _apiService.UpdateStatusCheckOrderAsync(_liveStreamCustomerID, newStatusCheck);
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
            _txtPhoneNumber = FindViewById<TextView>(Resource.Id.txPhoneNumber);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.listOrderRecyclerView);
            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));


            _txtOrderDate = FindViewById<TextView>(Resource.Id.txtOrderDate);
            _txtTotalPrice = FindViewById<TextView>(Resource.Id.txtTotalPrice);
            _txtOrderStatus = FindViewById<TextView>(Resource.Id.txtOrderStatus);

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
            var order = await _apiService.GetOrderByLiveStreamCustomerModelAsync(_liveStreamCustomerID);
            if (order == null)
            {
                Toast.MakeText(this, "Failed to fetch order details", ToastLength.Long).Show();
                return;
            }

            _txtCustomerName.Text = order.FacebookName;
            _txtAddress.Text = "Address: " + order.Address;
            _txtPhoneNumber.Text = "Phone: " + order.PhoneNumber;
            _txtTotalPrice.Text =  $"TotalPrice: {order.TotalPrice:n0}đ";
            _txtOrderDate.Text = $"OrderDate : " + order.OrderDate;
            _cbCheck.Checked = order.StatusCheck;
            _edtTrackingNumber.Text = order.TrackingNumber;
            _edtNote.Text = order.Note;

            var adapter = new OrderDetailAdapter(this, order.orderByProductCodeModels);
            _recyclerView.SetAdapter(adapter);
            UpdateStatusLayout(order.OrderStatus); 

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
                var success = await _apiService.UpdateOrderStatusAsync(_liveStreamCustomerID, newStatus);
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
            var tracking = _edtTrackingNumber.Text?.Trim() ?? "";
            var note = _edtNote.Text?.Trim() ?? "";

            try
            {
                var success = await _apiService.UpdateOrderTrackingAndNoteAsync(_liveStreamCustomerID, tracking, note);
                if (success)
                {
                    Toast.MakeText(this, "Order information updated", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Update failed", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }


    }
}
