using Android.Views;
using LOMSUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Adapter
{
    public class ProductSelectAdapter : BaseAdapter<ProductModel>
    {
        private readonly Activity _context;
        private readonly List<ProductModel> _products;
        private readonly List<ProductModel> _selectedProducts = new List<ProductModel>();

        public ProductSelectAdapter(Activity context, List<ProductModel> products) : base()
        {
            _context = context;
            _products = products;
        }

        public override ProductModel this[int position] => _products[position];

        public override int Count => _products.Count;

        public override long GetItemId(int position) => _products[position].ProductID.GetHashCode(); 

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.item_selectproductforsaleslist, parent, false);
            var product = _products[position];

            var productNameTextView = view.FindViewById<TextView>(Resource.Id.productNameTextView);
            var productPriceTextView = view.FindViewById<TextView>(Resource.Id.productPriceTextView);
            var productImageView = view.FindViewById<ImageView>(Resource.Id.productImageView);
            var productCheckBox = view.FindViewById<CheckBox>(Resource.Id.productCheckBox);

            productNameTextView.Text = product.Name;
            productPriceTextView.Text = $"Giá: {product.Price:N0} VNĐ"; 


            productCheckBox.Checked = _selectedProducts.Contains(product);

            productCheckBox.CheckedChange += (sender, e) =>
            {
                if (e.IsChecked)
                {
                    if (!_selectedProducts.Contains(product))
                    {
                        _selectedProducts.Add(product);
                    }
                }
                else
                {
                    _selectedProducts.Remove(product);
                }
            };

            return view;
        }

        public List<ProductModel> GetSelectedProducts()
        {
            return _selectedProducts;
        }
    }
}
