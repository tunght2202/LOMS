using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.Lang;
using LOMSUI.Models;
using System;
using System.Collections.Generic;

namespace LOMSUI.Adapter
{
    public class CustomerAdapter : RecyclerView.Adapter
    {
        private readonly List<CustomerModel> _customers;
        private readonly Context _context;
        private readonly Action<CustomerModel> _onDetailClick;

        public CustomerAdapter(List<CustomerModel> customers, Context context, Action<CustomerModel> onDetailClick)
        {
            _customers = customers;
            _context = context;
            _onDetailClick = onDetailClick;
        }

        public override int ItemCount => _customers.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CustomerViewHolder customerHolder)
            {
                var customer = _customers[position];

                Glide.With(customerHolder.ItemView.Context)
                     .Load(customer.ImageURL)
                     .Placeholder(Resource.Drawable.logos)
                     .Into(customerHolder.ImgAvatar);

                customerHolder.TxtCustomerName.Text = string.IsNullOrEmpty(customer.FullName) ? customer.FacebookName : customer.FullName;

                customerHolder.BtnViewDetail.Click += (s, e) => _onDetailClick?.Invoke(customer);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_customer, parent, false);
            return new CustomerViewHolder(itemView);
        }

        public class CustomerViewHolder : RecyclerView.ViewHolder
        {
            public ImageView ImgAvatar { get; }
            public TextView TxtCustomerName { get; }
            public Button BtnViewDetail { get; }

            public CustomerViewHolder(View itemView) : base(itemView)
            {
                ImgAvatar = itemView.FindViewById<ImageView>(Resource.Id.imgAvatar);
                TxtCustomerName = itemView.FindViewById<TextView>(Resource.Id.txtCustomerName);
                BtnViewDetail = itemView.FindViewById<Button>(Resource.Id.btnViewDetail);
            }
        }
    }
}
