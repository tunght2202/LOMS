using Bumptech.Glide;
using Java.Net;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{

    [Activity(Label = "ProductDetail")]
    public class ProductDetailActivity : BaseActivity
    {
        private ImageView _imgProduct;
        private EditText _etImageUrl, _etName, _etCode,
                       _etDescription, _etStock, _etPrice;
        private Button _btnSaveInfo;
        private ApiService _apiService;
        private int _productId;
        private ProductModelRequest _product;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_product_detail);

            _apiService = ApiServiceProvider.Instance;
            var _token = ApiServiceProvider.Token;

            _productId = Intent.GetIntExtra("ProductID", -1);
            if (_productId == -1)
            {
                Toast.MakeText(this, "Invalid ProductID", ToastLength.Long).Show();
                Finish();
                return;
            }

            InitViews();

            _product = await _apiService.GetProductByIdAsync(_productId);
            if (_product != null)
            {
                LoadProduct(_product);
            }


        }
        private void InitViews()
        {
            _imgProduct =  FindViewById<ImageView>(Resource.Id.imgProduct);
            _etImageUrl = FindViewById<EditText>(Resource.Id.etImageUrl);
            _etName = FindViewById<EditText>(Resource.Id.etName);
            _etCode = FindViewById<EditText>(Resource.Id.etCode);
            _etDescription = FindViewById<EditText>(Resource.Id.etDescription);
            _etStock = FindViewById<EditText>(Resource.Id.etStock);
            _etPrice = FindViewById<EditText>(Resource.Id.etPrice);
            _btnSaveInfo = FindViewById<Button>(Resource.Id.btnSaveInfo);


            _btnSaveInfo.Click += async (s, e) => await SaveProductInfo();

        }
        private void LoadProduct(ProductModelRequest product)
        {
            RunOnUiThread(() =>
            {
                _etName.Text = _product.Name;
                _etCode.Text = _product.ProductCode;
                _etDescription.Text = _product.Description;
                _etStock.Text = _product.Stock;
                _etPrice.Text = _product.Price;
                _etImageUrl.Text = _product.ImageURL;

                Glide.With(this).Load(_product.ImageURL).Into(_imgProduct);
            });


        }

        private async Task SaveProductInfo()
        {
            _product.Name = _etName.Text;
            _product.ProductCode = _etCode.Text;
            _product.Description = _etDescription.Text;
            _product.Price = _etPrice.Text; 
            _product.Stock = _etStock.Text;
            _product.ImageURL = _etImageUrl.Text;

            var errorResponse = await _apiService.UpdateProductAsync(_productId, _product);
            if (errorResponse == null)
            {
                Toast.MakeText(this, "Update Product successful!", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                _etName.Error = null;
                _etPrice.Error = null;
                _etStock.Error = null;
                _etDescription.Error = null;

                if (errorResponse.Errors.TryGetValue("Name", out var nameErrs))
                    _etName.Error = string.Join("\n", nameErrs);

                if (errorResponse.Errors.TryGetValue("Price", out var priceErrs))
                    _etPrice.Error = string.Join("\n", priceErrs);

                if (errorResponse.Errors.TryGetValue("Stock", out var stockErrs))
                    _etStock.Error = string.Join("\n", stockErrs);

                if (errorResponse.Errors.TryGetValue("Description", out var descErrs))
                    _etDescription.Error = string.Join("\n", descErrs);
            }
        }

    }
}
        
    
