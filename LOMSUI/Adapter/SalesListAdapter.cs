using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using LOMSUI.Models;

namespace LOMSUI.Adapter
{
    public class SalesListAdapter : BaseAdapter<ListProductModel>
    {
        private readonly Activity _context;
        private readonly List<ListProductModel> _salesLists;

        public SalesListAdapter(Activity context, List<ListProductModel> salesLists) : base()
        {
            _context = context;
            _salesLists = salesLists;
        }

        public override ListProductModel this[int position] => _salesLists[position];

        public override int Count => _salesLists.Count;

        public override long GetItemId(int position) => _salesLists[position].ListProductId.GetHashCode();

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var salesList = _salesLists[position];
            var view = convertView ?? _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = salesList.ListProductName;

            return view;
        }
    }
}