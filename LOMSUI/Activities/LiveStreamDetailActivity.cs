using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using LOMSUI.Activities;
using System;

namespace LOMSUI
{
    [Activity(Label = "Livestream Details")]
    public class LiveStreamDetailActivity : Activity
    {
        private TextView _txtTitle, _txtStatus, _txtStartTime;
        private Button _btnViewComments, _btnViewCustomers, _btnViewOrders;

        private string _liveStreamId;
        private string _title;
        private string _status;
        private string _startTime;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_livestream_detail);

            // Mapping UI
            _txtTitle = FindViewById<TextView>(Resource.Id.txtLiveTitle);
            _txtStatus = FindViewById<TextView>(Resource.Id.txtLiveStatus);
            _txtStartTime = FindViewById<TextView>(Resource.Id.txtLiveStartTime);
            _btnViewComments = FindViewById<Button>(Resource.Id.btnViewComments);
            _btnViewCustomers = FindViewById<Button>(Resource.Id.btnViewCustomers);
            _btnViewOrders = FindViewById<Button>(Resource.Id.btnViewOrders);

            _liveStreamId = Intent.GetStringExtra("LiveStreamID");
            _title = Intent.GetStringExtra("Title");
            _status = Intent.GetStringExtra("Status");
            _startTime = Intent.GetStringExtra("StartTime");

            _txtTitle.Text = _title;
            _txtStatus.Text = $"Status: {_status}";
            _txtStartTime.Text = $"Start: {_startTime}";

            _btnViewComments.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(CommentsActivity));
                intent.PutExtra("LivestreamID", _liveStreamId);
                StartActivity(intent);
            };

            _btnViewCustomers.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(CustomerListActivity));
                intent.PutExtra("LiveStreamID", _liveStreamId);
                StartActivity(intent);
            };

            _btnViewOrders.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(OrdersInLiveActivity));
                intent.PutExtra("LiveStreamID", _liveStreamId);
                StartActivity(intent);
            };
        }
    }
}
