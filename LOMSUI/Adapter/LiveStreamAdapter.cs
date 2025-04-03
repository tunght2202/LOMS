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
            viewHolder.Status.Text = $"{item.Status}";
            viewHolder.StartTime.Text = $"Bắt đầu: {item.StartTime}";

            // Mở livestream trên Facebook khi click vào item
            viewHolder.ItemView.Click += (sender, e) =>
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(item.StreamURL));
                _context.StartActivity(intent);
            };

            // Xóa livestream khi bấm nút
            viewHolder.BtnDelete.Click += async (sender, e) =>
            {
                bool success = await _apiService.DeleteLiveStreamAsync(item.LivestreamID);
                if (success)
                {
                    Toast.MakeText(_context, "Đã xóa livestream", ToastLength.Short).Show();
                    _liveStreams.RemoveAt(position);
                    NotifyItemRemoved(position);
                }
                else
                {
                    Toast.MakeText(_context, "Xóa thất bại!", ToastLength.Short).Show();
                }
            };
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

            public LiveStreamViewHolder(View itemView) : base(itemView)
            {
                Title = itemView.FindViewById<TextView>(Resource.Id.txtStreamTitle);
                Status = itemView.FindViewById<TextView>(Resource.Id.txtStreamStatus);
                StartTime = itemView.FindViewById<TextView>(Resource.Id.txtStreamStartTime);
                BtnDelete = itemView.FindViewById<Button>(Resource.Id.btnDeleteLiveStream);
            }
        }
    }

}

