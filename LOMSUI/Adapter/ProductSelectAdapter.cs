using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using LOMSUI.Models; 

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

        public override long GetItemId(int position) => _products[position].ProductID.GetHashCode(); // Hoặc một ID duy nhất khác

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.item_selectproductforsaleslist, parent, false);
            var product = _products[position];

            var productNameTextView = view.FindViewById<TextView>(Resource.Id.productNameTextView);
            var productPriceTextView = view.FindViewById<TextView>(Resource.Id.productPriceTextView);
            var productImageView = view.FindViewById<ImageView>(Resource.Id.productImageView);
            var productCheckBox = view.FindViewById<CheckBox>(Resource.Id.productCheckBox);

            productNameTextView.Text = product.Name;
            productPriceTextView.Text = $"Giá: {product.Price:N0} VNĐ"; // Định dạng giá

            // TODO: Load ảnh sản phẩm vào productImageView (sử dụng thư viện như Picasso hoặc Glide)

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