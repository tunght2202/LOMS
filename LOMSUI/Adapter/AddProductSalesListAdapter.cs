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
    public class AddProductSalesListAdapter : RecyclerView.Adapter
    {
        private readonly List<ProductModel> _products;
        private readonly Dictionary<int, bool> _selected = new();

        public AddProductSalesListAdapter(List<ProductModel> products)
        {
            _products = products;
            foreach (var p in _products)
            {
                _selected[p.ProductID] = false;
            }
        }

        public List<int> GetSelectedProductIds()
        {
            return _selected.Where(kv => kv.Value).Select(kv => kv.Key).ToList();
        }

        public override int ItemCount => _products.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ProductViewHolder;
            var product = _products[position];

            viewHolder.NameTextView.Text = product.Name;
            viewHolder.PriceTextView.Text = $"Giá: {product.Price:N0} VNĐ";
            viewHolder.CheckBox.Checked = _selected[product.ProductID];

            Glide.With(viewHolder.ImageView.Context)
                .Load(product.ImageURL)
                .Into(viewHolder.ImageView);

            viewHolder.CheckBox.CheckedChange += (s, e) =>
            {
                _selected[product.ProductID] = e.IsChecked;
            };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.item_add_product_saleslist, parent, false);

            return new ProductViewHolder(itemView);
        }

        private class ProductViewHolder : RecyclerView.ViewHolder
        {
            public ImageView ImageView { get; }
            public TextView NameTextView { get; }
            public TextView PriceTextView { get; }
            public CheckBox CheckBox { get; }

            public ProductViewHolder(View itemView) : base(itemView)
            {
                ImageView = itemView.FindViewById<ImageView>(Resource.Id.productImageView);
                NameTextView = itemView.FindViewById<TextView>(Resource.Id.productNameTextView);
                PriceTextView = itemView.FindViewById<TextView>(Resource.Id.productPriceTextView);
                CheckBox = itemView.FindViewById<CheckBox>(Resource.Id.productCheckBox);
            }
        }
    }

}
