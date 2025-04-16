using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using LOMSUI.Models;
using System;
using System.Collections.Generic;

namespace LOMSUI.Adapter
{
    public class CommentAdapter : RecyclerView.Adapter
    {
        private List<CommentModel> _comments;
        public event Action<CommentModel> OnCreateOrder;
        public event Action<CommentModel> OnViewInfo;

        public CommentAdapter(List<CommentModel> comments)
        {
            _comments = comments ?? new List<CommentModel>();
        }

        public override int ItemCount => _comments.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CommentViewHolder commentHolder)
            {
                var comment = _comments[position];

                commentHolder.TxtCustomerName.Text = comment.CustomerName ?? "Khách hàng ẩn danh";
                commentHolder.TxtCommentContent.Text = comment.Content;
                commentHolder.TxtCommentTime.Text = comment.GetFormattedTime();

                Glide.With(commentHolder.ItemView.Context)
                     .Load(comment.AvatarUrl)
                     .Placeholder(Resource.Drawable.loms)
                     .Error(Resource.Drawable.mtrl_ic_error)
                     .Into(commentHolder.ImgCustomerAvatar);

                commentHolder.BtnCreateOrder.Click -= BtnCreateOrder_Click;
                commentHolder.BtnCreateOrder.Click += BtnCreateOrder_Click;

                commentHolder.BtnViewInfo.Click -= BtnViewInfo_Click;
                commentHolder.BtnViewInfo.Click += BtnViewInfo_Click;

                commentHolder.BtnCreateOrder.Tag = new CommentWrapper(comment);
                commentHolder.BtnViewInfo.Tag = new CommentWrapper(comment);

            }
        }

        private void BtnCreateOrder_Click(object sender, EventArgs e)
        {
            if (sender is View view && view.Tag is CommentWrapper wrapper)
            {
                OnCreateOrder?.Invoke(wrapper.Comment);
            }
        }

        private void BtnViewInfo_Click(object sender, EventArgs e)
        {
            if (sender is View view && view.Tag is CommentWrapper wrapper)
            {
                OnViewInfo?.Invoke(wrapper.Comment);
            }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_comment, parent, false);
            return new CommentViewHolder(itemView);
        }
    }

    public class CommentViewHolder : RecyclerView.ViewHolder
    {
        public ImageView ImgCustomerAvatar { get; }
        public TextView TxtCustomerName { get; }
        public TextView TxtCommentContent { get; }
        public TextView TxtCommentTime { get; }
        public Button BtnCreateOrder { get; }
        public Button BtnViewInfo { get; }

        public CommentViewHolder(View itemView) : base(itemView)
        {
            ImgCustomerAvatar = itemView.FindViewById<ImageView>(Resource.Id.imgCustomerAvatar);
            TxtCustomerName = itemView.FindViewById<TextView>(Resource.Id.txtCustomerName);
            TxtCommentContent = itemView.FindViewById<TextView>(Resource.Id.txtCommentContent);
            TxtCommentTime = itemView.FindViewById<TextView>(Resource.Id.txtCommentTime);
            BtnCreateOrder = itemView.FindViewById<Button>(Resource.Id.btnCreateOrder);
            BtnViewInfo = itemView.FindViewById<Button>(Resource.Id.btnViewInfo);
        }
    }
}
