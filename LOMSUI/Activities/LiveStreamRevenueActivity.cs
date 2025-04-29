using LOMSUI.Services;
using System.Globalization;

namespace LOMSUI.Activities
{
    [Activity(Label = "LiveStream Revenue")]
    public class LiveStreamRevenueActivity : BaseActivity
    {
        private TextView _txtTotalRevenueLive, _txtTotalOrdersLive,
                  _txtOrderCancelLive, _txtOrderReturnLive, _txtOrderDeliveLive;
        private ApiService _apiService;
        private string _livestreamId;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_livestream_revenuen);


            _txtTotalRevenueLive = FindViewById<TextView>(Resource.Id.textRevenue);
            _txtTotalOrdersLive = FindViewById<TextView>(Resource.Id.textOrders);
            _txtOrderDeliveLive = FindViewById<TextView>(Resource.Id.textDelivered);
            _txtOrderCancelLive = FindViewById<TextView>(Resource.Id.textCancelled);
            _txtOrderReturnLive = FindViewById<TextView>(Resource.Id.textReturned);


            _apiService = ApiServiceProvider.Instance;
            _livestreamId = Intent.GetStringExtra("LiveStreamID");

            LoadRevenueData();

        }

        private async Task LoadRevenueData()
        {
            var revenueTask = _apiService.GetRevenueByLivestream(_livestreamId);
            var totalOrdersTask = _apiService.GetTotalOrdersByLivestreamIdAsync(_livestreamId);
            var cancelTask = _apiService.GetTotalOrdersCancelledByLivestreamIdAsync(_livestreamId);
            var returnTask = _apiService.GetTotalOrdersReturnedByLivestreamIdAsync(_livestreamId);
            var deliverTask = _apiService.GetTotalOrdersDeliveredByLivestreamIdAsync(_livestreamId);

            await Task.WhenAll(revenueTask, totalOrdersTask, cancelTask, returnTask, deliverTask);

            var revenueData = await revenueTask;
            var totalOrders = await totalOrdersTask;
            var totalOrderCancel = await cancelTask;
            var totalOrderReturn = await returnTask;
            var totalOrderDelive = await deliverTask;

     
            RunOnUiThread(() =>
            {
                if (revenueData != null)
                {
                    _txtTotalRevenueLive.Text = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:C0}", revenueData.LiveStreamRevenue);
                }

                _txtTotalOrdersLive.Text = $"{totalOrders:N0}";
                _txtOrderDeliveLive.Text = $"{totalOrderDelive:N0}";
                _txtOrderCancelLive.Text = $"{totalOrderCancel:N0}";
                _txtOrderReturnLive.Text = $"{totalOrderReturn:N0}";
            });
        }

    }

}

