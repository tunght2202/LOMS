using Android.Views;
using LOMSUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Adapter
{
    public class ProductAdapter : BaseAdapter<ProductModel>
    {
        private readonly Activity _context;
        private readonly List<ProductModel> _products;

        public ProductAdapter(Activity context, List<ProductModel> products)
        {
            _context = context;
            _products = products;
        }

        public override ProductModel this[int position] => _products[position];
        public override int Count => _products.Count;
        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _products[position];
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.item_product, null);
            }

            convertView.FindViewById<TextView>(Resource.Id.txtProductCode).Text = item.ProductCode;
            convertView.FindViewById<TextView>(Resource.Id.txtName).Text = item.Name;
            convertView.FindViewById<TextView>(Resource.Id.txtDescription).Text = item.Description;

            return convertView;
        }
    }

}
