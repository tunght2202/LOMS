using Android.Views;
using LOMSUI.Adapter;
using LOMSUI.Services;
using LOMSUI.Models;
using Android.Content;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Xamarin.Essentials;

namespace LOMSUI.Activities
{
    [Activity(Label = "Product")]
    public class ProductActivity : BaseActivity
    {
        private ApiService _apiService;
        private ProductAdapter _adapter;
        private RecyclerView _productRecyclerView;
        private TextView _noProductsTextView;
        private Button _addProductButton;
        private SwipeRefreshLayout _swipeRefreshLayout;
        private string _userId;
        private List<ProductModel> _products = new List<ProductModel>();


        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_product); 

            BottomNavHelper.SetupFooterNavigation(this, "products");

            _productRecyclerView = FindViewById<RecyclerView>(Resource.Id.productRecyclerView);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _addProductButton = FindViewById<Button>(Resource.Id.addProductButton);
            _swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);

            _productRecyclerView.SetLayoutManager(new LinearLayoutManager(this));

              _userId = Preferences.Get("userID", "");

            _apiService = ApiServiceProvider.Instance;
       
            _addProductButton.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(AddNewProductActivity));
                StartActivity(intent);
            };

            _swipeRefreshLayout.Refresh += async (s, e) =>
            {
                 LoadProductDataAsync();
                _swipeRefreshLayout.Refreshing = false;
            };
            await LoadProductDataAsync();
        }

        private async Task LoadProductDataAsync()
        {
            try
            {
                var products = await _apiService.GetAllProductsByUserAsync();

                    if (products != null && products.Count > 0)
                    {
                        _noProductsTextView.Visibility = ViewStates.Gone;

                        _products = products;
                        _adapter = new ProductAdapter(this, products);
                        _productRecyclerView.SetAdapter(_adapter);

                        _adapter.OnDeleteClick += product => ShowDeleteConfirmationDialog(product);

                        _adapter.OnViewDetailClick += product =>
                        {
                            var intent = new Intent(this, typeof(ProductDetailActivity));
                            intent.PutExtra("ProductID", product.ProductID);
                            StartActivity(intent);
                        };
                    }
                    else
                    {
                        _noProductsTextView.Visibility = ViewStates.Visible;
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading product data: {ex.Message}");
            }
        }
        private void ShowDeleteConfirmationDialog(ProductModel product)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Confirm deletion");
            builder.SetMessage($"Are you sure you want to delete the product?");
            builder.SetPositiveButton("Yes", async (sender, args) =>
            {
                bool success = await _apiService.DeleteProductAsync(product.ProductID);
                Toast.MakeText(this, success ? "Deleted successfully!" : "Delete failure!", ToastLength.Short).Show();

                if (success)
                {
                    _products.Remove(product); 
                    _adapter.NotifyDataSetChanged();
                }
            });

                builder.SetNegativeButton("No", (sender, args) => { });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }
    }
}
