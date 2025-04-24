<<<<<<< HEAD
﻿using Android.App;
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
=======
﻿using Android.Content;
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
    public class OrderAdapter : RecyclerView.Adapter
    {
        private List<OrderModel> _orders;
        private readonly Context _context;
        public event Action<OrderModel> OnViewDetailClick;

        public OrderAdapter(Context context, List<OrderModel> orders)
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
        {
            _context = context;
            _orders = orders;
        }

<<<<<<< HEAD
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
=======
        public override int ItemCount => _orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as OrderViewHolder;
            var order = _orders[position];

            viewHolder.TxtOrderCode.Text = $"Order code: ORD{order.OrderID}";
            viewHolder.TxtOrderDate.Text = $"Order date: {order.OrderDate:dd/MM/yyyy}";
            viewHolder.txtOrderQuantity.Text = $"Quantity: {order.Quantity}";
            viewHolder.TxtTotalPrice.Text = $"Price: {order.Quantity * order.Product.Price:n0}đ"; 
            viewHolder.TxtOrderStatus.Text = $"Status: {order.Status}";

            viewHolder.BtnViewDetail.Tag = new OrderModelWrapper(order);
            viewHolder.BtnViewDetail.Click -= BtnViewDetail_Click;
            viewHolder.BtnViewDetail.Click += BtnViewDetail_Click;

        }
        void BtnViewDetail_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is OrderModelWrapper wrapper)
            {
                var order = wrapper.Order;
                OnViewDetailClick?.Invoke(order);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.item_order_history, parent, false);
            return new OrderViewHolder(itemView);
        }

        public class OrderViewHolder : RecyclerView.ViewHolder
        {
            public TextView TxtOrderCode, TxtOrderDate, TxtTotalPrice, TxtOrderStatus, txtOrderQuantity;
            public Button BtnViewDetail;

            public OrderViewHolder(View itemView) : base(itemView)
            {
                TxtOrderCode = itemView.FindViewById<TextView>(Resource.Id.txtOrderCode);
                TxtOrderDate = itemView.FindViewById<TextView>(Resource.Id.txtOrderDate);
                TxtTotalPrice = itemView.FindViewById<TextView>(Resource.Id.txtTotalPrice);
                txtOrderQuantity = itemView.FindViewById<TextView>(Resource.Id.txtOrderQuantity);
                TxtOrderStatus = itemView.FindViewById<TextView>(Resource.Id.txtOrderStatus);
                BtnViewDetail = itemView.FindViewById<Button>(Resource.Id.btnViewOrderDetails);
            }
        }

        public void UpdateData(List<OrderModel> newOrders)
        {
            _orders = newOrders;
            NotifyDataSetChanged();
        }

    }

}
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
