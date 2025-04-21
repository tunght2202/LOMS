using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LOMSUI.Adapter;
using LOMSUI.Models;
using LOMSUI.Services;
namespace LOMSUI.Activities
{
    [Activity(Label = "Sales List")]
    public class SalesListActivity : BaseActivity
    {

        private RecyclerView _listProductRecyclerView;
        private SalesListAdapter _adapter;
        private List<ListProductModel> _salesLists = new List<ListProductModel>();
        private ApiService _apiService;
        private TextView _txtNoProducts;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_list_product);

            _listProductRecyclerView = FindViewById<RecyclerView>(Resource.Id.listProductRecyclerView);
            _txtNoProducts = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _listProductRecyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _apiService = ApiServiceProvider.Instance;

            await LoadListProducts();

        }

        private async Task LoadListProducts()
        {
            try
            {
                _salesLists = await _apiService.GetListProductsAsync();

                if (_salesLists == null || !_salesLists.Any())
                {
                    _txtNoProducts.Visibility = ViewStates.Visible;
                    return;
                }

                _adapter = new SalesListAdapter(this, _salesLists);
                _listProductRecyclerView.SetAdapter(_adapter);

                _adapter.OnViewProductClick += listProduct =>
                {
                    var intent = new Intent(this, typeof(ProductInSalesListActivity));
                    intent.PutExtra("ListProductId", listProduct.ListProductId);
                    intent.PutExtra("ListProductName", listProduct.ListProductName);
                    StartActivity(intent);
                };

                _adapter.OnDeleteClick +=  listProduct =>
                {
                    ShowDeleteConfirmationDialog(listProduct);
                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error loading ListProducts: " + ex.Message, ToastLength.Long).Show();
            }
        }

        private void ShowDeleteConfirmationDialog(ListProductModel listProduct)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Confirm deletion");
            builder.SetMessage($"Are you sure you want to delete {listProduct.ListProductName}?");

            builder.SetPositiveButton("Yes", async (sender, args) =>
            {
                bool success = await _apiService.DeleteListProductAsync(listProduct.ListProductId);
                Toast.MakeText(this, success ? "Deleted successfully!" : "Delete failure!", ToastLength.Short).Show();

                if (success)
                {
                    _salesLists.Remove(listProduct);
                    _adapter.NotifyDataSetChanged();
                }
            });

            builder.SetNegativeButton("No", (sender, args) => { });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }


    }
}