using Android.App;
using Android.Views;
using Android.Widget;
using LOMSUI.Models;
using System.Collections.Generic;

namespace LOMSUI.Adapter
{
    public class CurrentListAdapter : BaseAdapter<ListProductModel>
    {
        private readonly Activity _context;
        private readonly List<ListProductModel> _currentLists;

        public CurrentListAdapter(Activity context, List<ListProductModel> currentLists) : base()
        {
            _context = context;
            _currentLists = currentLists;
        }

        public override ListProductModel this[int position] => _currentLists[position];

        public override int Count => _currentLists.Count;

        public override long GetItemId(int position) => _currentLists[position].ListProductId.GetHashCode();

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var listItem = _currentLists[position];
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.item_currentlistview, null);

            var textViewListName = view.FindViewById<TextView>(Resource.Id.textViewListName);
            textViewListName.Text = listItem.ListProductName;

            // Bạn có thể thêm logic để hiển thị số lượng sản phẩm nếu cần

            return view;
        }
    }
}