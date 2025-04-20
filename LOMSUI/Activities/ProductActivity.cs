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
        private Button _createNewListButton;
        private Button _viewCurrentListsButton;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_productxml);

            BottomNavHelper.SetupFooterNavigation(this);

            _productListView = FindViewById<ListView>(Resource.Id.productListView);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _addProductButton = FindViewById<Button>(Resource.Id.addProductButton);
            _createNewListButton = FindViewById<Button>(Resource.Id.createNewListButton);
            _viewCurrentListsButton = FindViewById<Button>(Resource.Id.viewCurrentListsButton);

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
                //StartActivity(typeof(AddNewProductActivity));
            };
            _createNewListButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(CreateNewSalesListActivity));
                StartActivity(intent);
            };
            _viewCurrentListsButton.Click += OnViewCurrentListsButtonClick;

            await LoadProductDataAsync();
        }
        private void OnViewCurrentListsButtonClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(CurrentListViewActivity));
            StartActivity(intent);
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