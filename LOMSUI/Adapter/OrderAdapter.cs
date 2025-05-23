﻿using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using LOMSUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Adapter
{
    public class OrderAdapter : RecyclerView.Adapter
    {
        private List<OrderByLiveStreamCustoemrModel> _orders;
        private readonly Context _context;
        public event Action<OrderByLiveStreamCustoemrModel> OnViewDetailClick;

        public OrderAdapter(Context context, List<OrderByLiveStreamCustoemrModel> orders)
        {
            _context = context;
            _orders = orders;
        }

        public override int ItemCount => _orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as OrderViewHolder;
            var order = _orders[position];

            Glide.With(viewHolder.ItemView.Context)
          .Load(order.ImageUrl) 
          .Placeholder(Resource.Drawable.logos)
          .Into(viewHolder.imgCustomer);

            viewHolder.txtCustomerName.Text = order.CustoemrName;
            viewHolder.txtLiveStreamName.Text = order.LiveStreamTital;
            viewHolder.txtTotalOrder.Text = $"TotalOrder: {order.TotalOrder}";
            viewHolder.TxtTotalPrice.Text = $"TotalPrice: {order.TotalPrice:n0}đ";
            viewHolder.TxtOrderStatus.Text = $"Status: {order.OrderStatus}";

            viewHolder.BtnViewDetail.Tag = new OrderModelWrapper(order);
            viewHolder.BtnViewDetail.Click -= BtnViewDetail_Click;
            viewHolder.BtnViewDetail.Click += BtnViewDetail_Click;

        }
        void BtnViewDetail_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is OrderModelWrapper wrapper)
            {
                var order = wrapper.Order;
                OnViewDetailClick?.Invoke(order);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_order, parent, false);
            return new OrderViewHolder(itemView);
        }

        public class OrderViewHolder : RecyclerView.ViewHolder
        {
            public TextView txtCustomerName, txtLiveStreamName, TxtTotalPrice, TxtOrderStatus, txtTotalOrder;
            public ImageView imgCustomer;
            public Button BtnViewDetail;

            public OrderViewHolder(View itemView) : base(itemView)
            {
                imgCustomer = itemView.FindViewById<ImageView>(Resource.Id.imgCustomer);
                txtCustomerName = itemView.FindViewById<TextView>(Resource.Id.txtCustomerName);
                TxtTotalPrice = itemView.FindViewById<TextView>(Resource.Id.txtTotalPrice);
                txtTotalOrder = itemView.FindViewById<TextView>(Resource.Id.txtOrderTotal);
                TxtOrderStatus = itemView.FindViewById<TextView>(Resource.Id.txtOrderStatus);
                txtLiveStreamName = itemView.FindViewById<TextView>(Resource.Id.txtLiveStreamName);
                BtnViewDetail = itemView.FindViewById<Button>(Resource.Id.btnViewOrderDetails);
            }
        }

        public void UpdateData(List<OrderByLiveStreamCustoemrModel> newOrders)
        {
            _orders = newOrders;
            NotifyDataSetChanged();
        }

    }

}
