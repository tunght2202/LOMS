using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using LOMSUI.Adapter;
using LOMSUI.Services;
using System.Threading.Tasks;
using LOMSUI.Models;
using Android.Content;

namespace LOMSUI.Activities
{
    [Activity(Label = "ProductActivity")]
    public class ProductActivity : Activity
    {
        private ApiService _apiService = new ApiService();
        private ListView _productListView;
        private TextView _noProductsTextView;
        private Button _addProductButton;

        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listproduct); 

            BottomNavHelper.SetupFooterNavigation(this);

            _productListView = FindViewById<ListView>(Resource.Id.productListView);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _addProductButton = FindViewById<Button>(Resource.Id.addProductButton);

            _apiService = new ApiService();
            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            string token = prefs.GetString("token", null);

            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetToken(token);
            }

            _addProductButton.Click += (s, e) =>
            {
                Toast.MakeText(this, "Chuyển đến màn thêm sản phẩm", ToastLength.Short).Show();
                // StartActivity(typeof(AddProductActivity));
            };

            await LoadProductDataAsync();
        }

        private async Task LoadProductDataAsync()
        {
            var products = await _apiService.GetAllproduct();

            RunOnUiThread(() =>
            {
                if (products != null && products.Count > 0)
                {
                    _noProductsTextView.Visibility = ViewStates.Gone;
                    _productListView.Adapter = new ProductAdapter(this, products);
                }
                else
                {
                    _noProductsTextView.Visibility = ViewStates.Visible;
                }
            });
        }
    }
}
