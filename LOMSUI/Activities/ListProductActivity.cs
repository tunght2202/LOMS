using Android.App;
using Android.OS;
using Android.Widget;
using LOMSUI.Services;
using LOMSUI.Models;
using System.Collections.Generic;
using Android.Content;
using LOMSUI.Adapter;
using System.Linq;
using Newtonsoft.Json;
using Android.Views;
using Android.Glide;


namespace LOMSUI.Activities
{
    [Activity(Label = "ListProductActivity")]
    public class ListProductActivity : Activity
    {
        private ApiService _apiService = new ApiService();
        private ListView _listProductListView;
        private TextView _noProductsTextView;
        private Button _buttonCancel;
        private Button _buttonAddToList;
        private int _listProductId;
        private string _listProductName;
        private ListProductAdapter _adapter;
        private List<ProductModel> _allProducts = new List<ProductModel>();

        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_listproduct);

            BottomNavHelper.SetupFooterNavigation(this);

            _listProductListView = FindViewById<ListView>(Resource.Id.listProductListView);
            _noProductsTextView = FindViewById<TextView>(Resource.Id.noProductsTextView);
            _buttonCancel = FindViewById<Button>(Resource.Id.buttonCancel);
            _buttonAddToList = FindViewById<Button>(Resource.Id.buttonAddToList);

            _apiService = new ApiService();
            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            string token = prefs.GetString("token", null);

            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetToken(token);
            }

            _listProductId = Intent.GetIntExtra("ListProductId", -1);
            _listProductName = Intent.GetStringExtra("ListName");

            if (_listProductId == -1)
            {
                Toast.MakeText(this, "Lỗi: Không tìm thấy ID danh sách.", ToastLength.Long).Show();
                Finish();
                return;
            }

            if (ActionBar != null)
            {
                ActionBar.Title = $"Sản phẩm trong {_listProductName}";
            }

            _buttonCancel.Click += (sender, e) =>
            {
                Finish();
            };

            _buttonAddToList.Click += OnButtonAddToListClick;

            await LoadAllProductDataAsync(); 
        }
        private async Task LoadAllProductDataAsync()
        {
            var userProductsTask = _apiService.GetAllProductsByUserAsync();
            var allProductsTask = _apiService.GetAllproduct();

            await Task.WhenAll(userProductsTask, allProductsTask);

            var userProducts = await userProductsTask;
            var allProducts = await allProductsTask;

            if (allProducts != null)
            {
                _allProducts.AddRange(allProducts);
                _allProducts = _allProducts.DistinctBy(p => p.ProductID).ToList(); 
            }

            RunOnUiThread(() =>
            {
                if (_allProducts.Count > 0)
                {
                    _adapter = new ListProductAdapter(this, _allProducts);
                    _listProductListView.Adapter = _adapter;
                    _noProductsTextView.Visibility = ViewStates.Gone;
                }
                else
                {
                    _noProductsTextView.Visibility = ViewStates.Visible;
                    _noProductsTextView.Text = "Không có sản phẩm nào.";
                }
            });
        }
        //private async Task LoadAllProductDataAsync()
        //{
        //    var products = await _apiService.GetAllProductsByUserAsync(); 

        //    RunOnUiThread(() =>
        //    {
        //        if (products != null && products.Count > 0)
        //        {
        //            _allProducts = products;
        //            _adapter = new ListProductAdapter(this, _allProducts);
        //            _listProductListView.Adapter = _adapter;
        //            _noProductsTextView.Visibility = ViewStates.Gone;
        //        }
        //        else
        //        {
        //            _noProductsTextView.Visibility = ViewStates.Visible;
        //            _noProductsTextView.Text = "Không có sản phẩm nào.";
        //        }
        //    });
        //}

        private async void OnButtonAddToListClick(object sender, EventArgs e)
        {
            var selectedProducts = _adapter.GetCheckedItems();

            if (selectedProducts.Count > 0)
            {
                // Gọi API để thêm các sản phẩm đã chọn vào danh sách có _listProductId
                var response = await _apiService.AddMoreProductIntoListProductAsync(_listProductId, selectedProducts.Select(p => p.ProductID).ToList());

                if (response != null && response.IsSuccessStatusCode)
                {
                    Toast.MakeText(this, "Đã thêm sản phẩm vào danh sách.", ToastLength.Short).Show();
                    Finish(); // Quay lại màn hình danh sách hiện tại
                }
                else
                {
                    string errorMessage = "Lỗi khi thêm sản phẩm vào danh sách.";
                    if (response != null)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        errorMessage += $" Chi tiết: {errorContent}";
                        System.Diagnostics.Debug.WriteLine($"Lỗi thêm sản phẩm: {errorContent}");
                        try
                        {
                            var errorObject = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                            if (errorObject?.Message != null)
                            {
                                errorMessage = errorObject.Message;
                            }
                        }
                        catch (JsonException ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lỗi parse JSON lỗi: {ex.Message} - Content: {errorContent}");
                        }
                    }
                    Toast.MakeText(this, errorMessage, ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Vui lòng chọn ít nhất một sản phẩm.", ToastLength.Short).Show();
            }
        }
    }
}