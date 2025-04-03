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
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace LOMSUI
{
    [Activity(Label = "Facebook Live Streams")]
    public class LiveStreamActivity : Activity
    {
        private RecyclerView _recyclerView;
        private TextView _txtNoLiveStreams;
        private LiveStreamAdapter _adapter;
        private List<LiveStreamModel> _liveStreams = new List<LiveStreamModel>();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_live_stream);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewLiveStreams);
            _txtNoLiveStreams = FindViewById<TextView>(Resource.Id.txtNoLiveStreams);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            await LoadLiveStreams();
        }

        private async Task LoadLiveStreams()
        {
            var apiService = new ApiService();
            _liveStreams = await apiService.GetAllLiveStreamsAsync();

            if (_liveStreams.Any())
            {
                _adapter = new LiveStreamAdapter(_liveStreams, this);
                _recyclerView.SetAdapter(_adapter);
                _txtNoLiveStreams.Visibility = ViewStates.Gone;

                ItemTouchHelper itemTouchHelper = new ItemTouchHelper(new SwipeToDeleteCallback(_adapter, this));
                itemTouchHelper.AttachToRecyclerView(_recyclerView);
            }
            else
            {
                _txtNoLiveStreams.Visibility = ViewStates.Visible;
            }
        }
    }

}
