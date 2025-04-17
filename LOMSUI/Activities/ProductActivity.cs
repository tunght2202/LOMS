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
        private ApiService _apiService;
        private ProductAdapter _adapter;
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

            _apiService = ApiServiceProvider.Instance;
       
            _addProductButton.Click += (s, e) =>
            {
                Toast.MakeText(this, "Chuyển đến màn thêm sản phẩm", ToastLength.Short).Show();
                // StartActivity(typeof(AddProductActivity));
            };

            await LoadProductDataAsync();
        }

        private async Task LoadProductDataAsync()
        {
            try
            {
                var products = await _apiService.GetAllproduct();

                RunOnUiThread(() =>
                {
                    if (products != null && products.Count > 0)
                    {
                        _noProductsTextView.Visibility = ViewStates.Gone;

                        _adapter = new ProductAdapter(this, products);
                        _productListView.Adapter = _adapter;

                        _adapter.OnViewDetailClick += product =>
                        {
                            /* var intent = new Intent(this, typeof(ProductDetailActivity));
                             intent.PutExtra("ProductID", product.ProductID);
                             StartActivity(intent);*/
                        };
                    }
                    else
                    {
                        _noProductsTextView.Visibility = ViewStates.Visible;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading product data: {ex.Message}");
            }
        }
    }
}
