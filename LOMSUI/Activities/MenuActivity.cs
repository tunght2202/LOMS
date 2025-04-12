using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using LOMSUI.Models;
using System;

namespace LOMSUI.Activities
{
    [Activity(Label = "Menu")]
    public class MenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu);

            // Lấy các view từ layout
            TextView userNameTextView = FindViewById<TextView>(Resource.Id.userNameTextView);
            TextView emailTextView = FindViewById<TextView>(Resource.Id.emailTextView);
            Button logoutButton = FindViewById<Button>(Resource.Id.logout_button);
            LinearLayout productListLayout = FindViewById<LinearLayout>(Resource.Id.productListLayout);
            LinearLayout orderManagementLayout = FindViewById<LinearLayout>(Resource.Id.orderManagementLayout);
            LinearLayout liveManagement = FindViewById<LinearLayout>(Resource.Id.LiveManagement);
            LinearLayout helpLayout = FindViewById<LinearLayout>(Resource.Id.helpLinearLayout);
            LinearLayout aboutLayout = FindViewById<LinearLayout>(Resource.Id.aboutLinearLayout);
            LinearLayout fanpageLinkLayout = FindViewById<LinearLayout>(Resource.Id.fanpageLinkLayout);
            LinearLayout printerConnectionLayout = FindViewById<LinearLayout>(Resource.Id.printerConnectionLayout);
            LinearLayout privacyPolicyLayout = FindViewById<LinearLayout>(Resource.Id.privacyPolicyLayout);
            LinearLayout termsOfUseLayout = FindViewById<LinearLayout>(Resource.Id.termsOfUseLayout);
            LinearLayout statisticsLayout = FindViewById<LinearLayout>(Resource.Id.statisticsLayout);
            LinearLayout sellLayout = FindViewById<LinearLayout>(Resource.Id.sellLayout);
            LinearLayout productsLayout = FindViewById<LinearLayout>(Resource.Id.productsLayout);
            LinearLayout customersLayout = FindViewById<LinearLayout>(Resource.Id.customersLayout);

            MenuModel menuModel = new MenuModel
            {
                UserName = "Nguyễn Văn A",
                Email = "ANV1995@gmail.com"
            };

            userNameTextView.Text = menuModel.UserName;
            emailTextView.Text = menuModel.Email;

            logoutButton.Click += (sender, e) =>
            {
                var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
                prefs.Edit().Remove("token").Apply(); 

                Intent intent = new Intent(this, typeof(LoginActivity));
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                StartActivity(intent);
                Finish();
            };

            productListLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Danh sách sản phẩm", ToastLength.Short).Show();
            };

            orderManagementLayout.Click += (sender, e) =>
            {
               
                Toast.MakeText(this, "Quản lý đơn hàng", ToastLength.Short).Show();
            };

            liveManagement.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(LiveStreamActivity));
                StartActivity(intent);
            };

            helpLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Trợ giúp", ToastLength.Short).Show();
            };

            aboutLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Giới thiệu", ToastLength.Short).Show();
            };

            fanpageLinkLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Liên kết fanpage", ToastLength.Short).Show();
            };

            printerConnectionLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Kết nối máy in", ToastLength.Short).Show();
            };

            privacyPolicyLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Chính sách bảo mật", ToastLength.Short).Show();
            };

            termsOfUseLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Điều khoản sử dụng", ToastLength.Short).Show();
            };

            statisticsLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Thống kê", ToastLength.Short).Show();
            };

            sellLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Bán hàng", ToastLength.Short).Show();
            };

            productsLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Sản phẩm", ToastLength.Short).Show();
            };

            customersLayout.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Khách hàng", ToastLength.Short).Show();
            };

        }
    }
}