using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using LOMSUI.Models;

namespace LOMSUI.Adapter
{
    public class OrderDetailAdapter : RecyclerView.Adapter
    {
        private readonly Context _context;
        private readonly List<OrderByProductCodeModel> _orders;

        public OrderDetailAdapter(Context context, List<OrderByProductCodeModel> orders)
        {
            _context = context;
            _orders = orders;
        }

        public override int ItemCount => _orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is ProductViewHolder viewHolder)
            {
                var orders = _orders[position];

                //viewHolder.TxtProductName.Text = orders.;
                viewHolder.TxtOrderQuantity.Text = $"Quantity: {orders.Quantity}";
                viewHolder.TxtTotalPrice.Text = $"Price: {orders.TotalPrice:#,##0}đ";

             /*   if (!string.IsNullOrEmpty(orders.ImageUrl))
                {
                    Glide.With(_context)
                         .Load(orders.ImageUrl)
                         .Into(viewHolder.ImgProduct);
                }
                else
                {
                    viewHolder.ImgProduct.SetImageResource(Resource.Drawable.logos);
                }*/
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.item_order_detail, parent, false);
            return new ProductViewHolder(itemView);
        }

        private class ProductViewHolder : RecyclerView.ViewHolder
        {
            public TextView TxtProductName { get; }
            public TextView TxtOrderQuantity { get; }
            public TextView TxtTotalPrice { get; }
            public ImageView ImgProduct { get; }

            public ProductViewHolder(View itemView) : base(itemView)
            {
                TxtProductName = itemView.FindViewById<TextView>(Resource.Id.txtProductName);
                TxtOrderQuantity = itemView.FindViewById<TextView>(Resource.Id.txtOrderQuantity);
                TxtTotalPrice = itemView.FindViewById<TextView>(Resource.Id.txtTotalPrice);
                ImgProduct = itemView.FindViewById<ImageView>(Resource.Id.imgNameProduct);
            }
        }
    }

}
