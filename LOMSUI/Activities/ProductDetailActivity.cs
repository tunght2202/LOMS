﻿using Bumptech.Glide;
using Java.Net;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{

    [Activity(Label = "ProductDetail")]
    public class ProductDetailActivity : BaseActivity
    {
        private ImageView _imgProduct;
        private EditText _etName, _etCode,
                       _etDescription, _etStock, _etPrice;
        private Switch _switchStatus;
        private Button _btnSaveInfo;
        private ApiService _apiService;
        private int _productId;
        private ProductModel _product;
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
            _imgProduct = FindViewById<ImageView>(Resource.Id.imgProduct);
            _etName = FindViewById<EditText>(Resource.Id.etName);
            _etCode = FindViewById<EditText>(Resource.Id.etCode);
            _etDescription = FindViewById<EditText>(Resource.Id.etDescription);
            _etStock = FindViewById<EditText>(Resource.Id.etStock);
            _etPrice = FindViewById<EditText>(Resource.Id.etPrice);
            _switchStatus = FindViewById<Switch>(Resource.Id.switchStatus);
            _btnSaveInfo = FindViewById<Button>(Resource.Id.btnSaveInfo);


            _btnSaveInfo.Click += async (s, e) => await SaveProductInfo();

        }
        private void LoadProduct(ProductModel product)
        {
            RunOnUiThread(() =>
            {
                _etName.Text = _product.Name;
                _etCode.Text = _product.ProductCode;
                _etDescription.Text = _product.Description;
                _etStock.Text = _product.Stock.ToString();
                _etPrice.Text = product.Price.ToString("0");
                _switchStatus.Checked = _product.Status;

                Glide.With(this).Load(_product.ImageURL).Into(_imgProduct);
            });


        }

        private async Task SaveProductInfo()
        {
            _product.Name = _etName.Text;
            _product.ProductCode = _etCode.Text;
            _product.Description = _etDescription.Text;
            _product.Stock = int.Parse(_etStock.Text);
            _product.Price = decimal.Parse(_etPrice.Text);
            _product.Status = _switchStatus.Checked;

            bool success = await _apiService.UpdateProductAsync(_productId, _product);
            if (success)
            {

                Toast.MakeText(this, "Update Product successful!", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Product update failed!", ToastLength.Short).Show();

            }
        }
    }
}
