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
using AndroidX.SwipeRefreshLayout.Widget;
using LOMSUI.Activities;

namespace LOMSUI
{
    [Activity(Label = "Live Streams")]
    public class LiveStreamActivity : BaseActivity
    {
        private SwipeRefreshLayout _swipeRefreshLayout;
        private RecyclerView _recyclerView;
        private TextView _txtNoLiveStreams;
        private LiveStreamAdapter _adapter;
        private List<LiveStreamModel> _liveStreams = new List<LiveStreamModel>();
        private ApiService _apiService;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_live_stream);

            BottomNavHelper.SetupFooterNavigation(this, "sell");

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewLiveStreams);
            _txtNoLiveStreams = FindViewById<TextView>(Resource.Id.txtNoLiveStreams);
            _swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _apiService = ApiServiceProvider.Instance;

            _swipeRefreshLayout.Refresh += async (s, e) =>
            {
                await LoadLiveStreams();
                _swipeRefreshLayout.Refreshing = false;
            };

            await LoadLiveStreams();
        }

        private async Task LoadLiveStreams()
        {
            _liveStreams = await _apiService.GetAllLiveStreams();

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
                            ShowDeleteLiveStreamConfirmationDialog(livestream, position);

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

        private void ShowDeleteLiveStreamConfirmationDialog(LiveStreamModel livestream, int position)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Confirm deletion");
            builder.SetMessage("Are you sure you want to delete this livestream??");
            builder.SetPositiveButton("Yes", async (sender, args) =>
            {
                bool success = await _apiService.DeleteLiveStreamAsync(livestream.LivestreamID);
                Toast.MakeText(this, success ? "Delete successful!" : "Delete failed!", ToastLength.Short).Show();

                if (success)
                {
                    _liveStreams.RemoveAt(position);
                    _adapter.NotifyItemRemoved(position);
                }
            });
            builder.SetNegativeButton("No", (sender, args) => { });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

    }
}
