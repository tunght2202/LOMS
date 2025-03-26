using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using LOMSUI.Services;
using LOMSUI.Adapter;
using LOMSUI.Models;
using static LOMSUI.Services.ApiService;

namespace LOMSUI
{
    [Activity(Label = "Facebook Live Streams")]
    public class LiveStreamActivity : Activity
    {
        private ListView _listView;
        private FacebookLiveService _facebookLiveService = new FacebookLiveService();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_live_stream);

            _listView = FindViewById<ListView>(Resource.Id.listView);
            await LoadLiveStreamsAsync();
        }

        private async Task LoadLiveStreamsAsync()   
        {
            List<LiveVideo> liveStreams = await _facebookLiveService.GetLiveStreamsAsync();
            if (liveStreams.Count == 0)
            {
                Toast.MakeText(this, "No live streams found.", ToastLength.Short).Show();
                return;
            }

            var adapter = new LiveStreamAdapter(this, liveStreams);
            _listView.Adapter = adapter;
            _listView.ItemClick += (sender, e) =>
            {
                var stream = liveStreams[e.Position];
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(stream.PermalinkUrl));
                StartActivity(intent);
            };
        }
    }
}
