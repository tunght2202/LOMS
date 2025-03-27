using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Models;
using LOMSUI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOMSUI.Activities
{
    [Activity(Label = "CommentsActivity")]
    public class CommentsActivity : Activity
    {
        /* private EditText txtLiveStreamURL;
         private Button btnFetchComments;
         private RecyclerView recyclerViewComments;
         private TextView txtNoComments; 
         private CommentAdapter _commentAdapter;
         private ApiService _apiService;
         private List<CommentModel> _comments;

         protected override void OnCreate(Bundle savedInstanceState)
         {
             base.OnCreate(savedInstanceState);
             SetContentView(Resource.Layout.activity_comments);

             _apiService = new ApiService();
             _comments = new List<CommentModel>();

             txtLiveStreamURL = FindViewById<EditText>(Resource.Id.txtLiveStreamURL);
             btnFetchComments = FindViewById<Button>(Resource.Id.btnFetchComments);
             recyclerViewComments = FindViewById<RecyclerView>(Resource.Id.recyclerViewComments);
             txtNoComments = FindViewById<TextView>(Resource.Id.txtNoComments); 

             recyclerViewComments.SetLayoutManager(new LinearLayoutManager(this));

             btnFetchComments.Click += async (s, e) => await LoadComments();
         }

         private async Task LoadComments()
         {
             string liveStreamURL = txtLiveStreamURL.Text.Trim();
             if (string.IsNullOrEmpty(liveStreamURL))
             {
                 Toast.MakeText(this, "Vui lòng nhập URL Livestream", ToastLength.Short).Show();
                 return;
             }

             _comments = await _apiService.GetComments(liveStreamURL);

             if (_comments == null || _comments.Count == 0)
             {
                 recyclerViewComments.Visibility = Android.Views.ViewStates.Gone;
                 txtNoComments.Visibility = Android.Views.ViewStates.Visible;
                 txtNoComments.Text = "Không có bình luận nào!";
             }
             else
             {
                 recyclerViewComments.Visibility = Android.Views.ViewStates.Visible;
                 txtNoComments.Visibility = Android.Views.ViewStates.Gone;

                 _commentAdapter = new CommentAdapter(_comments);
                 recyclerViewComments.SetAdapter(_commentAdapter);

                 _commentAdapter.OnCreateOrder += comment =>
                     Toast.MakeText(this, $"Tạo đơn cho: {comment.CustomerName}", ToastLength.Short).Show();

                 _commentAdapter.OnViewInfo += comment =>
                     Toast.MakeText(this, $"Xem thông tin: {comment.CustomerName}", ToastLength.Short).Show();
             }
         }
     }*/
    }
}