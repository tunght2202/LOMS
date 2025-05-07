using Android.Content;
using Android.Graphics;
using Android.Provider;
using Bumptech.Glide;
using Java.Net;
using LOMSUI.Models;
using LOMSUI.Services;
using System.Net;

namespace LOMSUI.Activities
{

    [Activity(Label = "ProductDetail")]
        public class ProductDetailActivity : BaseActivity
        {
            private ImageView _imgProduct;
            private EditText _etName, _etCode,
                           _etDescription, _etStock, _etPrice;
            private Button _btnSaveInfo;
            private ApiService _apiService;
            private int _productId;
            private ProductModelRequest _product;
            private Stream _imageStream;
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

        private void ChooseImage(object sender, EventArgs e)
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            intent.AddCategory(Intent.CategoryOpenable);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Image"), 101);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 101 && resultCode == Result.Ok && data != null)
            {
                var uri = data.Data;
                _imgProduct.SetImageURI(uri);

                using var input = ContentResolver.OpenInputStream(uri);
                var options = new BitmapFactory.Options { InJustDecodeBounds = true };
                BitmapFactory.DecodeStream(input, null, options);

                int inSampleSize = Math.Max(options.OutHeight / 800, options.OutWidth / 800);
                options.InSampleSize = inSampleSize > 0 ? inSampleSize : 1;
                options.InJustDecodeBounds = false;

                input.Close();
                using var resizedStream = ContentResolver.OpenInputStream(uri);
                var bitmap = BitmapFactory.DecodeStream(resizedStream, null, options);

                var stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
                stream.Seek(0, SeekOrigin.Begin);
                _imageStream = stream;
            }
        }
        private void InitViews()
            {
                _imgProduct =  FindViewById<ImageView>(Resource.Id.imgProduct);
                _etName = FindViewById<EditText>(Resource.Id.etName);
                _etCode = FindViewById<EditText>(Resource.Id.etCode);
                _etDescription = FindViewById<EditText>(Resource.Id.etDescription);
                _etStock = FindViewById<EditText>(Resource.Id.etStock);
                _etPrice = FindViewById<EditText>(Resource.Id.etPrice);
                _btnSaveInfo = FindViewById<Button>(Resource.Id.btnSaveInfo);


                _btnSaveInfo.Click += async (s, e) => await SaveProductInfo();
                _imgProduct.Click += ChooseImage;

        }
        private void LoadProduct(ProductModelRequest product)
            {
                RunOnUiThread(() =>
                {
                    _etName.Text = _product.Name;
                    _etCode.Text = _product.ProductCode;
                    _etDescription.Text = _product.Description;
                    _etStock.Text = _product.Stock;
                    string input = _product.Price;
                    string result = input.Split('.')[0];
                    _etPrice.Text = result;

                    Glide.With(this).Load(_product.ImageURL).Into(_imgProduct);
                });


            }

            private async Task SaveProductInfo()
            {

            if (_imageStream == null && !string.IsNullOrEmpty(_product.ImageURL))
            {
                try
                {
                    using var webClient = new WebClient();
                    var imageBytes = await webClient.DownloadDataTaskAsync(_product.ImageURL);
                    _imageStream = new MemoryStream(imageBytes);

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Failed to load old image: " + ex.Message, ToastLength.Short).Show();
                    return;
                }
            }

            if (_imageStream == null)
            {
                Toast.MakeText(this, "Please select an image", ToastLength.Short).Show();
                return;
            }

            _product.Name = _etName.Text;
                _product.ProductCode = _etCode.Text;
                _product.Description = _etDescription.Text;
                _product.Price = _etPrice.Text; 
                _product.Stock = _etStock.Text;

            _imageStream.Seek(0, SeekOrigin.Begin);
            var clonedStream = new MemoryStream();
            await _imageStream.CopyToAsync(clonedStream);
            clonedStream.Seek(0, SeekOrigin.Begin);

            if (!(int.TryParse(_product.Price, out var Priceint)))
            {
                Toast.MakeText(this, "Price must integer", ToastLength.Short).Show();
                return;
            }

            var errorResponse = await _apiService.UpdateProductAsync(_productId, _product, clonedStream, "product.jpg");
                if (errorResponse == null)
                {
                    Toast.MakeText(this, "Update Product successful!", ToastLength.Short).Show();
                    Finish();
                }
                else
                {
                if (errorResponse.Errors != null && errorResponse.Errors.Count > 0)
                {
                    _etName.Error = null;
                    _etPrice.Error = null;
                    _etStock.Error = null;
                    _etDescription.Error = null;

                    if (errorResponse.Errors.TryGetValue("Name", out var nameErrs))
                        _etName.Error = string.Join("\n", nameErrs);

                    if (errorResponse.Errors.TryGetValue("ProductCode", out var codeErrs))
                        _etCode.Error = string.Join("\n", codeErrs);


                    if (errorResponse.Errors.TryGetValue("Price", out var priceErrs))
                        _etPrice.Error = string.Join("\n", priceErrs);

                    if (errorResponse.Errors.TryGetValue("Stock", out var stockErrs))
                        _etStock.Error = string.Join("\n", stockErrs);

                    if (errorResponse.Errors.TryGetValue("Description", out var descErrs))
                        _etDescription.Error = string.Join("\n", descErrs);
                }
                else
                {
                    Toast.MakeText(this, errorResponse.Message ?? "Unknown error occurred", ToastLength.Long).Show();

                }
            }
            }

        }
}
        
    
