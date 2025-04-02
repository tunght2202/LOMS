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

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_live_stream);

            _listView = FindViewById<ListView>(Resource.Id.listView);
            await LoadLiveStreamsAsync();
        }

        private async Task LoadLiveStreamsAsync()
        {
            var apiService = new ApiService();
            List<LiveVideo> liveStreams = await apiService.GetLiveStreamsAsync();


            if (liveStreams.Count == 0)
            {
                Toast.MakeText(this, "Không có livestream nào.", ToastLength.Short).Show();
                return;
            }

            var adapter = new LiveStreamAdapter(this, liveStreams);
            _listView.Adapter = adapter;
        }


    }
}
