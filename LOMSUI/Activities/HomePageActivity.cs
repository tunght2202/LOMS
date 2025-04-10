using Android.App;
using Android.OS;
using Android.Widget;
using LOMSUI.Models; // Thêm namespace cho HomePageModel

namespace LOMSUI.Activities
{
    [Activity(Label = "HomePage", MainLauncher = true)]
    public class HomePageActivity : Activity
    {
        // Khai báo các biến
        TextView appNameTextView;
        LinearLayout bottomNavLayout;
        LinearLayout thongKeLayout;
        LinearLayout banHangLayout;
        LinearLayout sanPhamLayout;
        LinearLayout khachHangLayout;
        LinearLayout menuLayout;

        // Thêm các biến cho các view hiển thị dữ liệu
        TextView revenueValueTextView;
        TextView orderValueTextView;

        HomePageModel homePageModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.homepage);

            // Ánh xạ view
            appNameTextView = FindViewById<TextView>(Resource.Id.appNameTextView);
            bottomNavLayout = FindViewById<LinearLayout>(Resource.Id.bottomNavLayout);
            thongKeLayout = FindViewById<LinearLayout>(Resource.Id.thongKeLayout);
            banHangLayout = FindViewById<LinearLayout>(Resource.Id.banHangLayout);
            sanPhamLayout = FindViewById<LinearLayout>(Resource.Id.sanPhamLayout);
            khachHangLayout = FindViewById<LinearLayout>(Resource.Id.khachHangLayout);
            menuLayout = FindViewById<LinearLayout>(Resource.Id.menuLayout);

            // Ánh xạ các view hiển thị dữ liệu
            revenueValueTextView = FindViewById<TextView>(Resource.Id.revenueValueTextView);
            orderValueTextView = FindViewById<TextView>(Resource.Id.orderValueTextView);

            // Tạo instance của HomePageModel và tải dữ liệu
            homePageModel = new HomePageModel();
            homePageModel.LoadData();

            // Hiển thị dữ liệu từ HomePageModel lên view
            appNameTextView.Text = "LOMS Application";
            revenueValueTextView.Text = homePageModel.Revenue.ToString("N0") + " vnd";
            orderValueTextView.Text = homePageModel.OrderCount.ToString();

            thongKeLayout.Click += (sender, e) =>
            {
                
            };

            banHangLayout.Click += (sender, e) =>
            {
                
            };

            sanPhamLayout.Click += (sender, e) =>
            {
                
            };

            khachHangLayout.Click += (sender, e) =>
            {
                
            };

            menuLayout.Click += (sender, e) =>
            {
                
            };
        }
    }
}