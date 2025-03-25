using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Models;

namespace LOMSUI.Adapter
{
    public class CommentAdapter : RecyclerView.Adapter
    {
        private List<CommentModel> _comments;
        public event Action<CommentModel> OnCreateOrder;
        public event Action<CommentModel> OnViewInfo;

        public CommentAdapter(List<CommentModel> comments)
        {
            _comments = comments;
        }

        public override int ItemCount => _comments.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CommentViewHolder commentHolder)
            {
                var comment = _comments[position];
                commentHolder.TxtCustomerName.Text = comment.CustomerName;
                commentHolder.TxtCommentContent.Text = comment.Content;
                commentHolder.TxtCommentTime.Text = comment.CommentTime;

                commentHolder.BtnCreateOrder.Click += (s, e) => OnCreateOrder?.Invoke(comment);
                commentHolder.BtnViewInfo.Click += (s, e) => OnViewInfo?.Invoke(comment);
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
        public TextView TxtCustomerName { get; }
        public TextView TxtCommentContent { get; }
        public TextView TxtCommentTime { get; }
        public Button BtnCreateOrder { get; }
        public Button BtnViewInfo { get; }

        public CommentViewHolder(View itemView) : base(itemView)
        {
            TxtCustomerName = itemView.FindViewById<TextView>(Resource.Id.txtCustomerName);
            TxtCommentContent = itemView.FindViewById<TextView>(Resource.Id.txtCommentContent);
            TxtCommentTime = itemView.FindViewById<TextView>(Resource.Id.txtCommentTime);
            BtnCreateOrder = itemView.FindViewById<Button>(Resource.Id.btnCreateOrder);
            BtnViewInfo = itemView.FindViewById<Button>(Resource.Id.btnViewInfo);
        }
    }
}
