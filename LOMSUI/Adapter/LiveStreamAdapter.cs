using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Models;
using LOMSUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;
using Android.Graphics;
using AndroidX.Core.Content;
using AndroidX.Core.View;
using Android.Graphics.Drawables;

namespace LOMSUI.Adapter
{
    public class LiveStreamAdapter : RecyclerView.Adapter
    {
        private List<LiveStreamModel> _liveStreams;
        private Context _context;
        private ApiService _apiService;

        public LiveStreamAdapter(List<LiveStreamModel> liveStreams, Context context)
        {
            _liveStreams = liveStreams;
            _context = context;
            _apiService = new ApiService();
        }

        public override int ItemCount => _liveStreams.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as LiveStreamViewHolder;
            var item = _liveStreams[position];

            viewHolder.Title.Text = item.StreamTitle;
            viewHolder.Status.Text = $"Trạng thái: {item.Status}";
            viewHolder.StartTime.Text = $"Bắt đầu: {item.StartTime}";

            viewHolder.ItemView.Click += (sender, e) =>
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(item.StreamURL));
                _context.StartActivity(intent);
            };
        }

        public async Task DeleteItem(int position)
        {
            bool success = await _apiService.DeleteLiveStreamAsync(_liveStreams[position].LivestreamID);
            if (success)
            {
                _liveStreams.RemoveAt(position);
                NotifyItemRemoved(position);
                Toast.MakeText(_context, "Đã xóa livestream", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(_context, "Xóa thất bại!", ToastLength.Short).Show();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_live_stream, parent, false);
            return new LiveStreamViewHolder(itemView);
        }

        public class LiveStreamViewHolder : RecyclerView.ViewHolder
        {
            public TextView Title { get; private set; }
            public TextView Status { get; private set; }
            public TextView StartTime { get; private set; }

            public LiveStreamViewHolder(View itemView) : base(itemView)
            {
                Title = itemView.FindViewById<TextView>(Resource.Id.txtStreamTitle);
                Status = itemView.FindViewById<TextView>(Resource.Id.txtStreamStatus);
                StartTime = itemView.FindViewById<TextView>(Resource.Id.txtStreamStartTime);
            }
        }
    }

    public class SwipeToDeleteCallback : ItemTouchHelper.SimpleCallback
    {
        private readonly LiveStreamAdapter _adapter;
        private readonly Drawable _deleteIcon;
        private readonly ColorDrawable _background;

        public SwipeToDeleteCallback(LiveStreamAdapter adapter, Context context) : base(0, ItemTouchHelper.Left)
        {
            _adapter = adapter;
            _deleteIcon = ContextCompat.GetDrawable(context, Android.Resource.Drawable.IcMenuDelete);
            _background = new ColorDrawable(Color.Red);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target) => false;

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int position = viewHolder.AdapterPosition;
            _adapter.DeleteItem(position);
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            View itemView = viewHolder.ItemView;
            int iconMargin = (itemView.Height - _deleteIcon.IntrinsicHeight) / 2;

            _background.SetBounds(itemView.Right + (int)dX, itemView.Top, itemView.Right, itemView.Bottom);
            _background.Draw(c);

            int iconLeft = itemView.Right - iconMargin - _deleteIcon.IntrinsicWidth;
            int iconRight = itemView.Right - iconMargin;
            int iconTop = itemView.Top + (itemView.Height - _deleteIcon.IntrinsicHeight) / 2;
            int iconBottom = iconTop + _deleteIcon.IntrinsicHeight;

            _deleteIcon.SetBounds(iconLeft, iconTop, iconRight, iconBottom);
            _deleteIcon.Draw(c);

            base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        }
    }
}
