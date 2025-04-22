using Android.Content;
using Bumptech.Glide;
using LOMSUI.Activities.IntroductActivity;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "Menu")]
    public class MenuActivity : Activity
    {
        private ApiService _apiService;
        private string _token;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu);

            BottomNavHelper.SetupFooterNavigation(this, "menu");

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
            LinearLayout privacyPolicyLayout = FindViewById<LinearLayout>(Resource.Id.privacyPolicyLayout);
            LinearLayout termsOfUseLayout = FindViewById<LinearLayout>(Resource.Id.termsOfUseLayout);

            _apiService = ApiServiceProvider.Instance;

                    var user = await _apiService.GetUserProfileAsync(); 

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
        

            userInfoSection.Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(UserInfoActivity));
                StartActivity(intent);
            };

            logoutButton.Click += (sender, e) =>
            {
                var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
                var editor = prefs.Edit();
                editor.Remove("token");
                editor.Remove("email");
                editor.Remove("password");
                editor.Remove("rememberMe");
                editor.Apply();

                Intent intent = new Intent(this, typeof(LoginActivity));
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                StartActivity(intent);
                Finish();
            };


            productListLayout.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(ProductActivity));
                StartActivity(intent);
            };

            orderManagementLayout.Click += (sender, e) =>
            {

                var intent = new Intent(this, typeof(OrderListActivity));
                intent.PutExtra("Type", "ByUser");
                StartActivity(intent);
            };

            liveManagement.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(LiveStreamActivity));
                StartActivity(intent);
            };

            helpLayout.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(HelpActivity));
                StartActivity(intent);
            };

            aboutLayout.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(AboutActivity));
                StartActivity(intent);
            };

            fanpageLinkLayout.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(FacebookTokenActivity));
                StartActivity(intent);
            };
            privacyPolicyLayout.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(PrivacyActivity));
                StartActivity(intent);
            };

            termsOfUseLayout.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(TermOfUserActivity));
                StartActivity(intent);
            };

        }
    }
}