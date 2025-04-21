using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapters;
using LOMSUI.Models;
using LOMSUI.Services;
namespace LOMSUI.Activities
{
    [Activity(Label = "ListProduct")]
    public class ListProductActivity : BaseActivity
    {
        private RecyclerView _recyclerView;
        private TextView _noProductsTextView;
        private SalesListAdapter _adapter;
        private ApiService _apiService;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_list_product);

            _apiService = ApiServiceProvider.Instance;

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.listProductRecyclerView);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _adapter = new SalesListAdapter(this, new List<ListProductModel>());
            _adapter.OnProductClick += OnViewProductClicked;
            _adapter.OnAddClick += OnAddProductClicked;

            _recyclerView.SetAdapter(_adapter);

            await LoadListProducts();
        }

        private async Task LoadListProducts()
        {
            var list = await _apiService.GetListProductsAsync(); 
            if (list != null && list.Any())
            {
                _adapter.UpdateData(list);
                _noProductsTextView.Visibility = ViewStates.Gone;
            }
            else
            {
                _noProductsTextView.Visibility = ViewStates.Visible;
            }
        }

        private void OnViewProductClicked(ListProductModel model)
        {
            Toast.MakeText(this, $"Viewing products in: {model.ListProductName}", ToastLength.Short).Show();
        }

        private void OnAddProductClicked(ListProductModel model)
        {
            Toast.MakeText(this, $"Adding to: {model.ListProductName}", ToastLength.Short).Show();
        }
    }
}