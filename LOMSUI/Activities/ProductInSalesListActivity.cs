using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using LOMSUI.Adapter;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{

    [Activity(Label = "Product In SalesList")]
    public class ProductInSalesListActivity : BaseActivity
    {
        private ApiService _apiService;
        private ProductAdapter _adapter;
        private RecyclerView _productRecyclerView;
        private TextView _noProductsTextView, _tvListName;
        private Button _addProductButton, _addListProductButton, _viewListProductButton;
        private SwipeRefreshLayout _swipeRefreshLayout;
        private int _listproductId;
        private string _listproductName;
        private List<ProductModel> _products = new List<ProductModel>();
        private List<ListProductModel> _salesLists = new List<ListProductModel>();


        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_product);

            BottomNavHelper.SetupFooterNavigation(this, "products");

            _productRecyclerView = FindViewById<RecyclerView>(Resource.Id.productRecyclerView);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _tvListName = FindViewById<TextView>(Resource.Id.tvListProductName);
            _addProductButton = FindViewById<Button>(Resource.Id.addProductButton);
            _addListProductButton = FindViewById<Button>(Resource.Id.addListProductButton);
            _viewListProductButton = FindViewById<Button>(Resource.Id.viewListProductButton);
            _swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);

            _productRecyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _addListProductButton.Visibility = ViewStates.Gone;
            _viewListProductButton.Visibility = ViewStates.Gone;
            _tvListName.Visibility = ViewStates.Visible;



            _apiService = ApiServiceProvider.Instance;
            _listproductId = Intent.GetIntExtra("ListProductId", -1);
            _listproductName = Intent.GetStringExtra("ListProductName");



            _addProductButton.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(AddProductSalesListActivity));
                intent.PutExtra("ListProductId", _listproductId);
                StartActivityForResult(intent, 100);
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
            _tvListName.Text = _listproductName;

            try
            {
                var products = await _apiService.GetProductsFromListProductAsync(_listproductId);

                if (products != null && products.Count > 0)
                {
                    _noProductsTextView.Visibility = ViewStates.Gone;

                    _products = products;
                    _adapter = new ProductAdapter(this, products);
                    _productRecyclerView.SetAdapter(_adapter);

                    _adapter.OnDeleteClick += product =>
                    {
                        ShowDeleteConfirmationDialog(_listproductId, new List<int> { product.ProductID });
                    };

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
        private void ShowDeleteConfirmationDialog(int listProductId, List<int> listProductIds)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Confirm deletion");
            builder.SetMessage($"Are you sure you want to delete selected products from the list?");

            builder.SetPositiveButton("Yes", async (sender, args) =>
            {
                bool success = await _apiService.DeleteProductsInListAsync(listProductId, listProductIds);
                Toast.MakeText(this, success ? "Deleted successfully!" : "Delete failed!", ToastLength.Short).Show();

                if (success)
                {
                    _products.RemoveAll(p => listProductIds.Contains(p.ProductID));
                    _adapter?.NotifyDataSetChanged();
                }
            });

            builder.SetNegativeButton("No", (sender, args) => { });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 100 && resultCode == Result.Ok)
            {
                    _ = LoadProductDataAsync();
            }
        }

    }
}
