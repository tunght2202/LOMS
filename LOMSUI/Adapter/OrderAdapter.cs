using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
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
            Glide.With(viewHolder.ItemView.Context)
                    .Load(order.Product.ImageURL)
                    .Placeholder(Resource.Drawable.logos)
                    .Into(viewHolder.imgNameProduct);

            viewHolder.txtCustomerName.Text = $"Customer :{order.FacebookName}";
            viewHolder.txtProductName.Text = $"{order.Product.Name}";
            viewHolder.txtOrderQuantity.Text = $"Quantity: {order.Quantity}";
            viewHolder.TxtTotalPrice.Text = $"TotalPrice: {order.Quantity * order.CurrentPrice:n0}đ"; 
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
            public TextView txtCustomerName, txtProductName, TxtTotalPrice, TxtOrderStatus, txtOrderQuantity;
            public ImageView imgNameProduct;
            public Button BtnViewDetail;

            public OrderViewHolder(View itemView) : base(itemView)
            {
                imgNameProduct = itemView.FindViewById<ImageView>(Resource.Id.imgNameProduct);
                txtCustomerName = itemView.FindViewById<TextView>(Resource.Id.txtCustomerName);
                txtProductName = itemView.FindViewById<TextView>(Resource.Id.txtProductName);
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
