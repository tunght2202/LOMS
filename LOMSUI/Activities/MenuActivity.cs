using Android.Content;
using Bumptech.Glide;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "Menu")]
    public class MenuActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu);

            BottomNavHelper.SetupFooterNavigation(this);

            TextView userNameTextView = FindViewById<TextView>(Resource.Id.userNameTextView);
            TextView emailTextView = FindViewById<TextView>(Resource.Id.emailTextView);
            ImageView ImageView = FindViewById<ImageView>(Resource.Id.profileImageView);
            LinearLayout userInfoSection = FindViewById<LinearLayout>(Resource.Id.userInfoSection);
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

            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            string token = prefs.GetString("token", null);

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var apiService = new ApiService();
                    var user = await apiService.GetUserProfileAsync(token); 

                    if (user != null)
                    {
                        userNameTextView.Text = user.UserName;
                        emailTextView.Text = user.Email;

                        if (!string.IsNullOrEmpty(user.ImageUrl))
                        {
                            Glide.With(this)
                                 .Load(user.ImageUrl)
                                 .Into(ImageView);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Error loading user information: " + ex.Message, ToastLength.Long).Show();
                }
            }

            userInfoSection.Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(UserInfoActivity));
                StartActivity(intent);
            };

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
                Intent intent = new Intent(this, typeof(FacebookTokenActivity));
                StartActivity(intent);
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

        }
    }
}