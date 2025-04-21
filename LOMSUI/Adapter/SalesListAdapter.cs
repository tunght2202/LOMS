using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Models;
using System;
using System.Collections.Generic;

namespace LOMSUI.Adapters
{
    public class SalesListAdapter : RecyclerView.Adapter
    {
        private readonly List<ListProductModel> _items;
        private readonly Context _context;

        public event Action<ListProductModel> OnProductClick;
        public event Action<ListProductModel> OnAddClick;

        public SalesListAdapter(Context context, List<ListProductModel> items)
        {
            _context = context;
            _items = items;
        }

        public override int ItemCount => _items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is SalesListViewHolder viewHolder)
            {
                var item = _items[position];
                viewHolder.TxtNameList.Text = item.ListProductName;

                viewHolder.BtnProduct.Click += (s, e) => OnProductClick?.Invoke(item);
                viewHolder.BtnAdd.Click += (s, e) => OnAddClick?.Invoke(item);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_list_product, parent, false);
            return new SalesListViewHolder(itemView);
        }

        public void UpdateData(List<ListProductModel> newList)
        {
            _items.Clear();
            _items.AddRange(newList);
            NotifyDataSetChanged();
        }

        class SalesListViewHolder : RecyclerView.ViewHolder
        {
            public TextView TxtNameList { get; }
            public Button BtnProduct { get; }
            public Button BtnAdd { get; }

            public SalesListViewHolder(View itemView) : base(itemView)
            {
                TxtNameList = itemView.FindViewById<TextView>(Resource.Id.txtNameList);
                BtnProduct = itemView.FindViewById<Button>(Resource.Id.productInListProduct);
                BtnAdd = itemView.FindViewById<Button>(Resource.Id.btnAddProductInLProduct);
            }
        }
    }
}
