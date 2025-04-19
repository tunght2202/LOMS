using Android.Content;
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
        {
            _context = context;
            _orders = orders;
        }

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
