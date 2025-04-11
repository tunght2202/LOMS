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
    public class OrderHistoryAdapter : RecyclerView.Adapter
    {
        private readonly List<OrderModel> _orders;
        private readonly Context _context;
        public event Action<OrderModel> OnViewDetailClick;

        public OrderHistoryAdapter(Context context, List<OrderModel> orders)
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
            viewHolder.TxtTotalPrice.Text = $"Price: {order.Quantity * 100000:n0}đ"; 
            viewHolder.TxtOrderStatus.Text = $"Status: {order.Status}";

            viewHolder.BtnViewDetail.Click += (s, e) => OnViewDetailClick?.Invoke(order);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.item_order_history, parent, false);
            return new OrderViewHolder(itemView);
        }

        public class OrderViewHolder : RecyclerView.ViewHolder
        {
            public TextView TxtOrderCode, TxtOrderDate, TxtTotalPrice, TxtOrderStatus;
            public Button BtnViewDetail;

            public OrderViewHolder(View itemView) : base(itemView)
            {
                TxtOrderCode = itemView.FindViewById<TextView>(Resource.Id.txtOrderCode);
                TxtOrderDate = itemView.FindViewById<TextView>(Resource.Id.txtOrderDate);
                TxtTotalPrice = itemView.FindViewById<TextView>(Resource.Id.txtTotalPrice);
                TxtOrderStatus = itemView.FindViewById<TextView>(Resource.Id.txtOrderStatus);
                BtnViewDetail = itemView.FindViewById<Button>(Resource.Id.btnViewOrderDetails);
            }
        }
    }

}
