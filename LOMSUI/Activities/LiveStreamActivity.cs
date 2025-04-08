using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using LOMSUI.Services;
using LOMSUI.Adapter;
using LOMSUI.Models;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using System.Linq;

namespace LOMSUI
{
    [Activity(Label = "Live Streams")]
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

            await apiService.GetLiveStreamsFromFaceBook();

            _liveStreams = await apiService.GetAllLiveStreamsAsync();

            RunOnUiThread(() =>
            {
                if (_liveStreams.Any())
                {
                    _adapter = new LiveStreamAdapter(
                        _liveStreams,
                        this,
                        onViewClick: livestream =>
                        {
                            Intent intent = new Intent(this, typeof(LOMSUI.Activities.CommentsActivity));
                            intent.PutExtra("LivestreamID", livestream.LivestreamID);
                            StartActivity(intent);
                        },
                        onDeleteClick: async (livestream, position) =>
                        {
                            var deleteService = new ApiService();
                            bool success = await deleteService.DeleteLiveStreamAsync(livestream.LivestreamID);

                            if (success)
                            {
                                Toast.MakeText(this, "Đã xóa livestream", ToastLength.Short).Show();
                                _liveStreams.RemoveAt(position);
                                _adapter.NotifyItemRemoved(position);
                            }
                            else
                            {
                                Toast.MakeText(this, "Xóa thất bại!", ToastLength.Short).Show();
                            }
                        });

                    _recyclerView.SetAdapter(_adapter);
                    _txtNoLiveStreams.Visibility = ViewStates.Gone;
                }
                else
                {
                    _txtNoLiveStreams.Visibility = ViewStates.Visible;
                }
            });
        }
    }
}
