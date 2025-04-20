using Android.App;
using Android.Views;
using Android.Widget;
using LOMSUI.Models;
using System.Collections.Generic;
using Bumptech.Glide;

namespace LOMSUI.Adapter
{
    public class ListProductAdapter : BaseAdapter<ProductModel>
    {
        private readonly Activity _context;
        private readonly List<ProductModel> _products;
        private readonly List<ProductModel> _checkedItems = new List<ProductModel>();

        public ListProductAdapter(Activity context, List<ProductModel> products) : base()
        {
            _context = context;
            _products = products;
        }

        public override ProductModel this[int position] => _products[position];

        public override int Count => _products.Count;

        public override long GetItemId(int position) => _products[position].ProductID.GetHashCode();

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var product = _products[position];
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.item_listproduct, null);

            var textViewSTT = view.FindViewById<TextView>(Resource.Id.textViewSTT);
            var textViewProductName = view.FindViewById<TextView>(Resource.Id.textViewProductName);
            var imageViewProduct = view.FindViewById<ImageView>(Resource.Id.imageViewProduct);
            var textViewPrice = view.FindViewById<TextView>(Resource.Id.textViewPrice);
            var textViewQuantity = view.FindViewById<TextView>(Resource.Id.textViewQuantity);
            var checkBoxAdd = view.FindViewById<CheckBox>(Resource.Id.checkBoxAdd);

            textViewSTT.Text = (position + 1).ToString();
            textViewProductName.Text = product.Name;
            textViewPrice.Text = product.Price.ToString("N0");
            textViewQuantity.Text = product.Stock.ToString();

            // Load ảnh sản phẩm bằng Glide
            if (!string.IsNullOrEmpty(product.ImageURL))
            {
                Glide.With(_context)
                    .Load(product.ImageURL)
                    .Placeholder(Resource.Drawable.logo_loms)
                    .Error(Resource.Drawable.mtrl_ic_error)
                    .Into(imageViewProduct);
            }
            else
            {
                imageViewProduct.SetImageResource(Resource.Drawable.logo_loms);
            }

            checkBoxAdd.Checked = _checkedItems.Contains(product);
            checkBoxAdd.Tag = position;

            checkBoxAdd.CheckedChange += (sender, e) =>
            {
                var cb = sender as CheckBox;
                if (cb?.Tag != null)
                {
                    int pos = (int)cb.Tag; 
                    var selectedProduct = _products[pos];
                    if (e.IsChecked)
                    {
                        if (!_checkedItems.Contains(selectedProduct))
                        {
                            _checkedItems.Add(selectedProduct);
                        }
                    }
                    else
                    {
                        if (_checkedItems.Contains(selectedProduct))
                        {
                            _checkedItems.Remove(selectedProduct);
                        }
                    }
                }
            };

            return view;
        }

        public List<ProductModel> GetCheckedItems()
        {
            return _checkedItems;
        }
    }
}
