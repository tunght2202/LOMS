using Android.App;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_comments);

            _apiService = new ApiService();
            _allComments = new List<CommentModel>();

            txtLiveStreamId = FindViewById<EditText>(Resource.Id.txtLiveStreamId);
            txtProductCode = FindViewById<EditText>(Resource.Id.txtProductCode);
            btnFetchComments = FindViewById<Button>(Resource.Id.btnFetchComments);
            btnFilterByProduct = FindViewById<Button>(Resource.Id.btnFilterByProductCode);
            recyclerViewComments = FindViewById<RecyclerView>(Resource.Id.recyclerViewComments);
            txtNoComments = FindViewById<TextView>(Resource.Id.txtNoComments);

            recyclerViewComments.SetLayoutManager(new LinearLayoutManager(this));

            btnFetchComments.Click += async (s, e) => await LoadComments();
            btnFilterByProduct.Click += (s, e) => FilterCommentsByProduct();
        }

        private async Task LoadComments()
        {
            string liveStreamId = txtLiveStreamId.Text.Trim();
            if (string.IsNullOrEmpty(liveStreamId))
            {
                Toast.MakeText(this, "Vui lòng nhập ID Livestream", ToastLength.Short).Show();
                return;
            }

            _allComments = await _apiService.GetComments(liveStreamId);
            UpdateCommentList(_allComments);
        }

        private void FilterCommentsByProduct()
        {
            string productCode = txtProductCode.Text.Trim();
            if (string.IsNullOrEmpty(productCode))
            {
                UpdateCommentList(_allComments); // Hiển thị lại toàn bộ nếu không nhập mã
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
                    recyclerViewComments.Visibility = Android.Views.ViewStates.Gone;
                    txtNoComments.Visibility = Android.Views.ViewStates.Visible;
                    txtNoComments.Text = "Không có bình luận nào!";

                    txtProductCode.Visibility = Android.Views.ViewStates.Gone;
                    btnFilterByProduct.Visibility = Android.Views.ViewStates.Gone;

                    Toast.MakeText(this, "Không có bình luận nào!", ToastLength.Short).Show();
                }
                else
                {
                    recyclerViewComments.Visibility = Android.Views.ViewStates.Visible;
                    txtNoComments.Visibility = Android.Views.ViewStates.Gone;

                    _commentAdapter = new CommentAdapter(comments);
                    recyclerViewComments.SetAdapter(_commentAdapter);

                    txtProductCode.Visibility = Android.Views.ViewStates.Visible;
                    btnFilterByProduct.Visibility = Android.Views.ViewStates.Visible;

                    Toast.MakeText(this, $"Tổng số bình luận: {comments.Count}", ToastLength.Short).Show();

                    _commentAdapter.OnCreateOrder += comment =>
                        Toast.MakeText(this, $"Tạo đơn cho: {comment.CustomerName}", ToastLength.Short).Show();

                    _commentAdapter.OnViewInfo += comment =>
                        Toast.MakeText(this, $"Xem thông tin: {comment.CustomerName}", ToastLength.Short).Show();
                }
            });
        }



    }
}
