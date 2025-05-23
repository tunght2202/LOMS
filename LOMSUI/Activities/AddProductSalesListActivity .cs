using Android.Views;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "Add Product Sale List")]
    public class AddProductSalesListActivity : BaseActivity
    {
        private RecyclerView _recyclerView;
        private Button _addButton, _btnSearchByProductName;
        private EditText _etProductName;
        private TextView _noProductsTextView;
        private AddProductSalesListAdapter _adapter;
        private ApiService _apiService;
        private List<ProductModel> _originalProducts;
        private int _listProductId;

        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_product_saleslist);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.productRecyclerView);
            _addButton = FindViewById<Button>(Resource.Id.addToSalesListButton);
            _btnSearchByProductName = FindViewById<Button>(Resource.Id.btnSearchByProductName);
            _etProductName = FindViewById<EditText>(Resource.Id.etProductName);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);

            _apiService = ApiServiceProvider.Instance;

            _listProductId = Intent.GetIntExtra("ListProductId", -1);

            await LoadProducts();

            _btnSearchByProductName.Click += (s, e) =>
            {
                FilterProducts(_etProductName.Text);
            };

            _etProductName.TextChanged += (s, e) =>
            {
                FilterProducts(_etProductName.Text);
            };

            _addButton.Click += async (s, e) =>
            {
                var selectedProductIds = _adapter.GetSelectedProductIds();
                if (!selectedProductIds.Any())
                {
                    Toast.MakeText(this, "Please select at least one product", ToastLength.Short).Show();
                    return;
                }
            
                var success = await _apiService.AddMoreProductIntoListProductAsync(_listProductId, selectedProductIds);
                if (success)
                {
                    Toast.MakeText(this, "Product added to list", ToastLength.Short).Show();
                    SetResult(Result.Ok);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Add failures", ToastLength.Long).Show();
                }
            };
        }

        private async Task LoadProducts()
        {
            var allProducts = await _apiService.GetAllProductsByUserAsync();
            var existingProducts = await _apiService.GetProductsFromListProductAsync(_listProductId);

            var filteredProducts = allProducts
                .Where(p => !existingProducts.Any(ep => ep.ProductID == p.ProductID))
                .ToList();

            _originalProducts = filteredProducts;

            if (filteredProducts == null || !filteredProducts.Any())
            {
                _noProductsTextView.Visibility = ViewStates.Visible;
                _addButton.Enabled = false; 
                return;
            }

            _noProductsTextView.Visibility = ViewStates.Gone;
            _addButton.Enabled=true;

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            _adapter = new AddProductSalesListAdapter(_originalProducts);
            _recyclerView.SetAdapter(_adapter);
        }

        private void FilterProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                _adapter.UpdateData(_originalProducts); 
                return;
            }

            var filtered = _originalProducts
                .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            _adapter.UpdateData(filtered);
        }


    }
}

