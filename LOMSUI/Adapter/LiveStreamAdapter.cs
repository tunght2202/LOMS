using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Models;
using LOMSUI.Services;
using System;
using System.Collections.Generic;
using Android.Widget;
using LOMSUI.Activities;

namespace LOMSUI.Adapter
{
    public class LiveStreamAdapter : RecyclerView.Adapter
    {
        private List<LiveStreamModel> _liveStreams;
        private Context _context;
        private ApiService _apiService;
        private readonly Action<LiveStreamModel> _onViewClick;
        private readonly Action<LiveStreamModel, int> _onDeleteClick;

        public LiveStreamAdapter(List<LiveStreamModel> liveStreams, Context context,
            Action<LiveStreamModel> onViewClick,
            Action<LiveStreamModel, int> onDeleteClick)
        {
            _liveStreams = liveStreams;
            _context = context;
            _apiService = new ApiService();
            _onViewClick = onViewClick;
            _onDeleteClick = onDeleteClick;
        }

        public override int ItemCount => _liveStreams.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as LiveStreamViewHolder;
            var item = _liveStreams[position];

            viewHolder.Title.Text = item.StreamTitle;
            viewHolder.Status.Text = $"{item.Status}";
            viewHolder.StartTime.Text = $"Start: {item.GetFormattedTime()}";

            viewHolder.ItemView.Click += (sender, e) =>
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(item.StreamURL));
                _context.StartActivity(intent);
            };

            viewHolder.BtnView.Click += (sender, e) =>
            {
                Intent intent = new Intent(_context, typeof(CommentsActivity));
                intent.PutExtra("LivestreamID", item.LivestreamID);
                _context.StartActivity(intent);
            };

            viewHolder.BtnDelete.Click += (sender, e) => _onDeleteClick?.Invoke(item, position);
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
            public Button BtnDelete { get; private set; }
            public Button BtnView { get; private set; }

            public LiveStreamViewHolder(View itemView) : base(itemView)
            {
                Title = itemView.FindViewById<TextView>(Resource.Id.txtStreamTitle);
                Status = itemView.FindViewById<TextView>(Resource.Id.txtStreamStatus);
                StartTime = itemView.FindViewById<TextView>(Resource.Id.txtStreamStartTime);
                BtnDelete = itemView.FindViewById<Button>(Resource.Id.btnDeleteLiveStream);
                BtnView = itemView.FindViewById<Button>(Resource.Id.viewURLStream);
            }
        }
    }
}
