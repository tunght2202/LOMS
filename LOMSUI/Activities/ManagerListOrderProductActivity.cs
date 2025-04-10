using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;
using LOMSUI.Models;

namespace LOMSUI.Activities
{
    [Activity(Label = "ManagerListOrderProductActivity")]
    public class ManagerListOrderProductActivity : Activity
    {
        private List<ManagerListOrderProductModel> orderList;
        private LinearLayout orderListContainer;

        // Toolbar views
        private LinearLayout toolbar;
        private TextView toolbarTitleLoms;
        private TextView toolbarTitleOrderManagement;

        // Status tab views
        private HorizontalScrollView statusScrollView;
        private LinearLayout statusTabContainer;
        private Button btnWaitingConfirmation;
        private Button btnWaitingPickup;
        private Button btnWaitingDelivery;
        private Button btnDelivered;
        private Button btnReturned;
        private Button btnCancelled;

        // Search views
        private ImageView imgSearchIcon;
        private TextView txtSearchLabel;

        // Order list scroll view
        private ScrollView orderListScrollView;

        // Bottom navigation views
        private LinearLayout bottomNavLayout;
        private LinearLayout thongKeLayout;
        private ImageView thongKeImageView;
        private TextView thongKeTextView;
        private LinearLayout banHangLayout;
        private ImageView banHangImageView;
        private TextView banHangTextView;
        private LinearLayout sanPhamLayout;
        private ImageView sanPhamImageView;
        private TextView sanPhamTextView;
        private LinearLayout khachHangLayout;
        private ImageView khachHangImageView;
        private TextView khachHangTextView;
        private LinearLayout menuLayout;
        private ImageView menuImageView;
        private TextView menuTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the content view
            SetContentView(Resource.Layout.managerlistorderproduct);

            // Initialize views
            toolbar = FindViewById<LinearLayout>(Resource.Id.toolbar);
            toolbarTitleLoms = FindViewById<TextView>(Resource.Id.toolbar_title_loms);
            toolbarTitleOrderManagement = FindViewById<TextView>(Resource.Id.toolbar_title_order_management);

            statusScrollView = FindViewById<HorizontalScrollView>(Resource.Id.status_scroll_view);
            statusTabContainer = FindViewById<LinearLayout>(Resource.Id.status_tab_container);
            btnWaitingConfirmation = FindViewById<Button>(Resource.Id.btn_waiting_confirmation);
            btnWaitingPickup = FindViewById<Button>(Resource.Id.btn_waiting_pickup);
            btnWaitingDelivery = FindViewById<Button>(Resource.Id.btn_waiting_delivery);
            btnDelivered = FindViewById<Button>(Resource.Id.btn_delivered);
            btnReturned = FindViewById<Button>(Resource.Id.btn_returned);
            btnCancelled = FindViewById<Button>(Resource.Id.btn_cancelled);

            imgSearchIcon = FindViewById<ImageView>(Resource.Id.img_search_icon);
            txtSearchLabel = FindViewById<TextView>(Resource.Id.txt_search_label);

            orderListScrollView = FindViewById<ScrollView>(Resource.Id.order_list_scroll_view);
            orderListContainer = FindViewById<LinearLayout>(Resource.Id.order_list_container);

            bottomNavLayout = FindViewById<LinearLayout>(Resource.Id.bottomNavLayout);
            thongKeLayout = FindViewById<LinearLayout>(Resource.Id.thongKeLayout);
            thongKeImageView = FindViewById<ImageView>(Resource.Id.thongKeImageView);
            thongKeTextView = FindViewById<TextView>(Resource.Id.thongKeTextView);
            banHangLayout = FindViewById<LinearLayout>(Resource.Id.banHangLayout);
            banHangImageView = FindViewById<ImageView>(Resource.Id.banHangImageView);
            banHangTextView = FindViewById<TextView>(Resource.Id.banHangTextView);
            sanPhamLayout = FindViewById<LinearLayout>(Resource.Id.sanPhamLayout);
            sanPhamImageView = FindViewById<ImageView>(Resource.Id.sanPhamImageView);
            sanPhamTextView = FindViewById<TextView>(Resource.Id.sanPhamTextView);
            khachHangLayout = FindViewById<LinearLayout>(Resource.Id.khachHangLayout);
            khachHangImageView = FindViewById<ImageView>(Resource.Id.khachHangImageView);
            khachHangTextView = FindViewById<TextView>(Resource.Id.khachHangTextView);
            menuLayout = FindViewById<LinearLayout>(Resource.Id.menuLayout);
            menuImageView = FindViewById<ImageView>(Resource.Id.menuImageView);
            menuTextView = FindViewById<TextView>(Resource.Id.menuTextView);

            // Initialize order list (replace with your actual data loading)
            orderList = new List<ManagerListOrderProductModel>();
            orderList.Add(new ManagerListOrderProductModel("Nguyễn Văn A", "Chờ xác nhận", BitmapFactory.DecodeResource(Resources, Resource.Drawable.logo_loms), "Chuột gaming x11", "400.000", "400.000"));
            // Add more order items here

            // Populate the order list on the UI
            PopulateOrderList();

            // Set up event listeners for status buttons (example)
            btnWaitingConfirmation.Click += (sender, e) => FilterOrders("Chờ xác nhận");
            btnWaitingPickup.Click += (sender, e) => FilterOrders("Chờ lấy hàng");
            btnWaitingDelivery.Click += (sender, e) => FilterOrders("Chờ giao hàng");
            btnDelivered.Click += (sender, e) => FilterOrders("Đã giao hàng");
            btnReturned.Click += (sender, e) => FilterOrders("Trả hàng");
            btnCancelled.Click += (sender, e) => FilterOrders("Hủy đơn hàng");

            menuLayout.Click += (sender, e) => {
                Toast.MakeText(this, "Menu Clicked", ToastLength.Short).Show();
            };
        }

        private void PopulateOrderList()
        {
            orderListContainer.RemoveAllViews(); // Clear existing views

            foreach (var order in orderList)
            {
                View itemView = LayoutInflater.Inflate(Resource.Layout.managerlistorderproduct, null); 

                TextView customerNameTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_customer_name);
                TextView statusTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_status);
                ImageView productImageImageView = itemView.FindViewById<ImageView>(Resource.Id.order_item_product_image);
                TextView productNameTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_product_name);
                TextView productPriceTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_product_price);
                TextView totalPriceTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_total_price);

                customerNameTextView.Text = order.CustomerName;
                statusTextView.Text = order.Status;
                productImageImageView.SetImageBitmap(order.ProductImage);
                productNameTextView.Text = order.ProductName;
                productPriceTextView.Text = order.ProductPrice;
                totalPriceTextView.Text = $"Tổng số tiền (1 sản phẩm): {order.TotalPrice}";

                orderListContainer.AddView(itemView);

                // Add a separator between items (optional)
                View separator = new View(this);
                separator.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 1);
                separator.SetBackgroundColor(Color.ParseColor("#BDBDBD"));
                orderListContainer.AddView(separator);
            }
        }

        private void FilterOrders(string status)
        {
            var filteredList = orderList.Where(order => order.Status.ToLower() == status.ToLower()).ToList();
            // 
            orderListContainer.RemoveAllViews();
            foreach (var order in filteredList)
            {
                View itemView = LayoutInflater.Inflate(Resource.Layout.managerlistorderproduct, null); 

                TextView customerNameTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_customer_name);
                TextView statusTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_status);
                ImageView productImageImageView = itemView.FindViewById<ImageView>(Resource.Id.order_item_product_image);
                TextView productNameTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_product_name);
                TextView productPriceTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_product_price);
                TextView totalPriceTextView = itemView.FindViewById<TextView>(Resource.Id.order_item_total_price);

                customerNameTextView.Text = order.CustomerName;
                statusTextView.Text = order.Status;
                productImageImageView.SetImageBitmap(order.ProductImage);
                productNameTextView.Text = order.ProductName;
                productPriceTextView.Text = order.ProductPrice;
                totalPriceTextView.Text = $"Tổng số tiền (1 sản phẩm): {order.TotalPrice}";

                orderListContainer.AddView(itemView);

                View separator = new View(this);
                separator.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 1);
                separator.SetBackgroundColor(Color.ParseColor("#BDBDBD"));
                orderListContainer.AddView(separator);
            }
        }
    }
}