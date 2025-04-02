using Android.Views;
using LOMSUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Adapter
{
    public class LiveStreamAdapter : BaseAdapter<LiveVideo>
    {
        private readonly Activity _context;
        private readonly List<LiveVideo> _items;

        public LiveStreamAdapter(Activity context, List<LiveVideo> items)
        {
            _context = context;
            _items = items;
        }

        public override LiveVideo this[int position] => _items[position];
        public override int Count => _items.Count;
        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            var stream = _items[position];

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = stream.StreamTitle;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = $"Status: {stream.Status} - {stream.FormattedStartTime}";

            return view;
        }



    }
}
