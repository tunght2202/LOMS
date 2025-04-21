using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Adapter
{
    public class SalesListAdapter : RecyclerView.Adapter
    {
        private readonly List<ListProductModel> _items;
        private readonly Context _context;

        public event Action<ListProductModel> OnViewProductClick;
        public event Action<ListProductModel> OnDeleteClick;

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

                viewHolder.BtnViewProduct.Click += (s, e) => OnViewProductClick?.Invoke(item);
                viewHolder.BtnDelete.Click += (s, e) => OnDeleteClick?.Invoke(item);
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
            public Button BtnViewProduct { get; }
            public Button BtnDelete { get; }

            public SalesListViewHolder(View itemView) : base(itemView)
            {
                TxtNameList = itemView.FindViewById<TextView>(Resource.Id.txtNameList);
                BtnViewProduct = itemView.FindViewById<Button>(Resource.Id.productInListProduct);
                BtnDelete = itemView.FindViewById<Button>(Resource.Id.btnDeleteListProduct);
            }
        }
    }
}
