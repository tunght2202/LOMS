using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using LOMSUI.Adapter; // Đảm bảo namespace này đúng
using LOMSUI.Services; // Đảm bảo namespace này đúng
using LOMSUI.Models; // Đảm bảo namespace này đúng
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Views; // Để serialize/deserialize dữ liệu

namespace LOMSUI.Activities
{
    [Activity(Label = "Chọn sản phẩm")]
    public class SelectProductForSalesListActivity : Activity
    {
        private ListView _productListView;
        private Button _addToSalesListButton;
        private TextView _noProductsTextView;
        private ApiService _apiService;
        private ProductSelectAdapter _adapter; 

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_selectproductforsaleslist); // Sử dụng lại layout đã chỉnh sửa

            _productListView = FindViewById<ListView>(Resource.Id.productListView);
            _addToSalesListButton = FindViewById<Button>(Resource.Id.addToSalesListButton);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _apiService = new ApiService();

            BottomNavHelper.SetupFooterNavigation(this);

            await LoadProductDataAsync();

            _addToSalesListButton.Click += OnAddToSalesListButtonClick;
        }

        private async Task LoadProductDataAsync()
        {
            var products = await _apiService.GetAllproduct();

            RunOnUiThread(() =>
            {
                if (products != null && products.Count > 0)
                {
                    _noProductsTextView.Visibility = ViewStates.Gone;
                    // Khởi tạo adapter tùy chỉnh của bạn (ProductSelectAdapter)
                    _adapter = new ProductSelectAdapter(this, products);
                    _productListView.Adapter = _adapter;
                }
                else
                {
                    _noProductsTextView.Visibility = ViewStates.Visible;
                }
            });
        }

        private void OnAddToSalesListButtonClick(object sender, EventArgs e)
        {
            // Lấy danh sách các sản phẩm đã được chọn từ adapter
            List<ProductModel> selectedProducts = _adapter.GetSelectedProducts();

            if (selectedProducts != null && selectedProducts.Count > 0)
            {
                // Tạo Intent để chuyển đến CreateNewSalesListActivity
                Intent intent = new Intent(this, typeof(CreateNewSalesListActivity));

                intent.PutExtra("selectedProductsJson", JsonConvert.SerializeObject(selectedProducts));

                // Khởi chạy CreateNewSalesListActivity
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Vui lòng chọn ít nhất một sản phẩm.", ToastLength.Short).Show();
            }
        }
    }
}