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
<<<<<<< HEAD
        private Button _addProductButton;
        private Button _createNewListButton;
        private Button _viewCurrentListsButton;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_productxml);
=======
        private Button _addProductButton, _addListProductButton, _viewListProductButton;
        private SwipeRefreshLayout _swipeRefreshLayout;
        private string _userId;
        private List<ProductModel> _products = new List<ProductModel>();


        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_product); 
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2

            BottomNavHelper.SetupFooterNavigation(this, "products");

            _productRecyclerView = FindViewById<RecyclerView>(Resource.Id.productRecyclerView);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _addProductButton = FindViewById<Button>(Resource.Id.addProductButton);
<<<<<<< HEAD
            _createNewListButton = FindViewById<Button>(Resource.Id.createNewListButton);
            _viewCurrentListsButton = FindViewById<Button>(Resource.Id.viewCurrentListsButton);
=======
            _addListProductButton = FindViewById<Button>(Resource.Id.addListProductButton);
            _viewListProductButton = FindViewById<Button>(Resource.Id.viewListProductButton);
            _swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2

            _productRecyclerView.SetLayoutManager(new LinearLayoutManager(this));

              _userId = Preferences.Get("userID", "");

            _apiService = ApiServiceProvider.Instance;
       
            _addProductButton.Click += (s, e) =>
            {
<<<<<<< HEAD
                Toast.MakeText(this, "Chuyển đến màn thêm sản phẩm", ToastLength.Short).Show();
                //StartActivity(typeof(AddNewProductActivity));
=======
                var intent = new Intent(this, typeof(AddNewProductActivity));
                StartActivity(intent);
            };
            _addListProductButton.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(AddSalesListActivity));
                StartActivity(intent);
            };
            _viewListProductButton.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(SalesListActivity));
                StartActivity(intent);
>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
            };
            _createNewListButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(CreateNewSalesListActivity));
                StartActivity(intent);
            };
            _viewCurrentListsButton.Click += OnViewCurrentListsButtonClick;

            _swipeRefreshLayout.Refresh += async (s, e) =>
            {
                 LoadProductDataAsync();
                _swipeRefreshLayout.Refreshing = false;
            };
            await LoadProductDataAsync();
        }
        private void OnViewCurrentListsButtonClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(CurrentListViewActivity));
            StartActivity(intent);
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