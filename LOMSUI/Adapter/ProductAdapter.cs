﻿using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace LOMSUI.Adapter
{
    public class ProductAdapter : RecyclerView.Adapter
    {
        private readonly Context _context;
        private readonly List<ProductModel> _products;
        public event Action<ProductModel> OnViewDetailClick;
        public event Action<ProductModel> OnDeleteClick; 

        public ProductAdapter(Context context, List<ProductModel> products)
        {
            _context = context;
            _products = products;
        }

        public override int ItemCount => _products.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ProductViewHolder;
            var product = _products[position];

            viewHolder.TvName.Text = product.Name;
            viewHolder.TvDescription.Text = product.Description;
            viewHolder.TvPrice.Text = $"Price : {product.Price:N0}đ";

            viewHolder.BtnDetail.Click -= viewHolder.DetailClickHandler;
            viewHolder.DetailClickHandler = (s, e) => OnViewDetailClick?.Invoke(product);
            viewHolder.BtnDetail.Click += viewHolder.DetailClickHandler;

            viewHolder.BtnDelete.Click -= viewHolder.DeleteClickHandler;
            viewHolder.DeleteClickHandler = (s, e) => OnDeleteClick?.Invoke(product);
            viewHolder.BtnDelete.Click += viewHolder.DeleteClickHandler;
        }
        


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.item_product, parent, false);
            return new ProductViewHolder(itemView);
        }

        public class ProductViewHolder : RecyclerView.ViewHolder
        {
            public TextView TvName { get; }
            public TextView TvDescription { get; }
            public TextView TvPrice { get; }
            public Button BtnDetail { get; }
            public Button BtnDelete { get; }

            public EventHandler DetailClickHandler { get; set; }
            public EventHandler DeleteClickHandler { get; set; } 

            public ProductViewHolder(View itemView) : base(itemView)
            {
                TvName = itemView.FindViewById<TextView>(Resource.Id.tvNameProduct);
                TvDescription = itemView.FindViewById<TextView>(Resource.Id.tvDescription);
                TvPrice = itemView.FindViewById<TextView>(Resource.Id.tvPrice);
                BtnDetail = itemView.FindViewById<Button>(Resource.Id.btDetail);
                BtnDelete = itemView.FindViewById<Button>(Resource.Id.btDelete);
            }
        }
    }


}
