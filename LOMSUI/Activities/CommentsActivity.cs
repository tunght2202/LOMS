using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Models;
using LOMSUI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOMSUI.Activities
{
    [Activity(Label = "Comments")]
    public class CommentsActivity : Activity
    {
        private EditText txtLiveStreamId, txtProductCode;
        private Button btnFetchComments, btnFilterByProduct;
        private RecyclerView recyclerViewComments;
        private TextView txtNoComments;
        private CommentAdapter _commentAdapter;
        private ApiService _apiService;
        private List<CommentModel> _allComments;
        private bool _isPolling = false;
        private string _currentLiveStreamId = "";

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_comments);

            BottomNavHelper.SetupFooterNavigation(this);

            _apiService = ApiServiceProvider.Instance;

            _allComments = new List<CommentModel>();

            txtLiveStreamId = FindViewById<EditText>(Resource.Id.txtLiveStreamId);
            txtProductCode = FindViewById<EditText>(Resource.Id.txtProductCode);
            btnFetchComments = FindViewById<Button>(Resource.Id.btnFetchComments);
            btnFilterByProduct = FindViewById<Button>(Resource.Id.btnFilterByProductCode);
            recyclerViewComments = FindViewById<RecyclerView>(Resource.Id.recyclerViewComments);
            txtNoComments = FindViewById<TextView>(Resource.Id.txtNoComments);

            recyclerViewComments.SetLayoutManager(new LinearLayoutManager(this));

            _currentLiveStreamId = Intent.GetStringExtra("LivestreamID");
            if (!string.IsNullOrEmpty(_currentLiveStreamId))
            {
                txtLiveStreamId.Text = _currentLiveStreamId;

                _ = StartPollingComments();
            }


            btnFetchComments.Click += async (s, e) =>
            {
                _isPolling = false;
                await Task.Delay(100);
                await StartPollingComments();
            };

            btnFilterByProduct.Click += (s, e) => FilterCommentsByProduct();
        }


        private async Task StartPollingComments()
        {
            _currentLiveStreamId = txtLiveStreamId.Text.Trim();

            if (string.IsNullOrEmpty(_currentLiveStreamId))
            {
                Toast.MakeText(this, "Please enter Livestream ID", ToastLength.Short).Show();
                return;
            }

            try
            {
                var initialComments = await _apiService.GetComments(_currentLiveStreamId);
                if (initialComments != null)
                {
                    _allComments = initialComments;
                    UpdateCommentList(_allComments);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Error loading initial comment: {ex.Message}", ToastLength.Short).Show();
            }

            _isPolling = true;

            while (_isPolling)
            {
                try
                {
                    var latestComments = await _apiService.GetComments(_currentLiveStreamId);

                    if (latestComments != null &&
                        (latestComments.Count != _allComments.Count ||
                         latestComments.Any(c => !_allComments.Any(a => a.CommentID == c.CommentID))))
                    {
                        _allComments = latestComments;
                        UpdateCommentList(_allComments);
                    }
                }
                catch (Exception ex)
                {
                    RunOnUiThread(() => Toast.MakeText(this, $"Error loading comments: {ex.Message}", ToastLength.Short).Show());
                }

                await Task.Delay(4000);
            }
        }


        private void FilterCommentsByProduct()
        {
            string productCode = txtProductCode.Text.Trim();
            if (string.IsNullOrEmpty(productCode))
            {
                UpdateCommentList(_allComments);
            }
            else
            {
                var filteredComments = _allComments
                    .Where(c => c.Content.Contains(productCode, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                UpdateCommentList(filteredComments);
            }
        }

        private void UpdateCommentList(List<CommentModel> comments)
        {
            RunOnUiThread(() =>
            {
                if (comments == null || comments.Count == 0)
                {
                    recyclerViewComments.Visibility = Android.Views.ViewStates.Visible;
                    txtNoComments.Visibility = Android.Views.ViewStates.Visible;
                    txtNoComments.Text = "No comments!";

                    txtProductCode.Visibility = Android.Views.ViewStates.Gone;
                    btnFilterByProduct.Visibility = Android.Views.ViewStates.Gone;

                    Toast.MakeText(this, "No comments!", ToastLength.Short).Show();
                }
                else
                {
                    recyclerViewComments.Visibility = Android.Views.ViewStates.Visible;
                    txtNoComments.Visibility = Android.Views.ViewStates.Gone;

                    _commentAdapter = new CommentAdapter(comments);
                    recyclerViewComments.SetAdapter(_commentAdapter);

                    txtProductCode.Visibility = Android.Views.ViewStates.Visible;
                    btnFilterByProduct.Visibility = Android.Views.ViewStates.Visible;

                    Toast.MakeText(this, $"Total comments: {comments.Count}", ToastLength.Short).Show();

                    _commentAdapter.OnCreateOrder += async comment =>
                    {
                        var result = await _apiService.CreateOrderFromCommentAsync(comment.CommentID);
                        if (result)
                        {
                            Toast.MakeText(this, "Order created successfully!", ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(this, "Order creation failed!", ToastLength.Short).Show();
                        }
                    };

                    _commentAdapter.OnViewInfo += comment =>
                    {
                        if (!string.IsNullOrEmpty(comment.CustomerID))
                        {
                            Intent intent = new Intent(this, typeof(CustomerInfoActivity));
                            intent.PutExtra("CustomerID", comment.CustomerID);
                            StartActivity(intent);
                        }
                        else
                        {
                            Toast.MakeText(this, "CustomerID not found!", ToastLength.Short).Show();
                        }
                    };

                }
            });
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _isPolling = false;
        }


    }
}
