using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using LOMSUI.Models;

namespace LOMSUI.Adapter
{
    public class OrderAdapter : BaseAdapter<OrderModel>
    {
        private readonly Activity _context;
        private readonly List<OrderModel> _orders;

        public OrderAdapter(Activity context, List<OrderModel> orders)
        {
            _context = context;
            _orders = orders;
        }

        public override int Count => _orders.Count;

        public override OrderModel this[int position] => _orders[position];

        public override long GetItemId(int position) => _orders[position].OrderID;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            OrderViewHolder holder = null;

            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.item_list_order, null);
                holder = new OrderViewHolder(view);
                view.Tag = holder;
            }
            else
            {
                holder = (OrderViewHolder)view.Tag;
            }

            var order = _orders[position];
            if (order.Product != null)
            {
                holder.ProductNameTextView.Text = order.Product.Name;
                holder.ProductQuantityTextView.Text = $"x{order.Quantity}";
                holder.TotalAmountTextView.Text = order.Product.Price.ToString("N0"); // Hiển thị giá sản phẩm
                // TODO: Load ảnh sản phẩm từ order.Product.ImageUrl (nếu có)
            }
            else
            {
                // Xử lý trường hợp Product là null (có thể hiển thị thông báo lỗi hoặc giá trị mặc định)
                holder.ProductNameTextView.Text = "Không có thông tin sản phẩm";
                holder.ProductQuantityTextView.Text = "";
                holder.TotalAmountTextView.Text = "N/A";
            }

            // Thông tin đơn hàng chung
            holder.OrderStatusTextView.Text = order.Status;
            // Chúng ta không có CustomerName trong OrderModel hiện tại.
            // Bạn có thể cần thêm thuộc tính này hoặc lấy nó từ một nguồn khác nếu cần.
            holder.CustomerNameTextView.Text = "Tên khách hàng (cần bổ sung)";

            return view;
        }
    }

    public class OrderViewHolder : Java.Lang.Object
    {
        public TextView CustomerNameTextView { get; set; }
        public TextView OrderStatusTextView { get; set; }
        public TextView ProductNameTextView { get; set; }
        public TextView ProductQuantityTextView { get; set; }
        public TextView TotalAmountTextView { get; set; }
        public ImageView ProductImageView { get; set; }

        public OrderViewHolder(View view)
        {
            CustomerNameTextView = view.FindViewById<TextView>(Resource.Id.customerNameTextView);
            OrderStatusTextView = view.FindViewById<TextView>(Resource.Id.orderStatusTextView);
            ProductNameTextView = view.FindViewById<TextView>(Resource.Id.productNameTextView);
            ProductQuantityTextView = view.FindViewById<TextView>(Resource.Id.productQuantityTextView);
            TotalAmountTextView = view.FindViewById<TextView>(Resource.Id.totalAmountTextView);
            ProductImageView = view.FindViewById<ImageView>(Resource.Id.productImageView);
            // OrderIdTextView = view.FindViewById<TextView>(Resource.Id.orderIdTextView);
        }
    }
}