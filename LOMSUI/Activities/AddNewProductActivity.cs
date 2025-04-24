using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "Add Product")]
    public class AddNewProductActivity : BaseActivity
    {
        private ImageView _imgProduct;
        private EditText _edtName, _edtDescription, _edtPrice, _edtStock, _edtCode;
        private Button _btnSave, _btnCancel;
        private Uri _imageUri;
        private Stream _imageStream;
        private ApiService _apiService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.addnewproduct);

            _imgProduct = FindViewById<ImageView>(Resource.Id.imgProductadd);
            _edtName = FindViewById<EditText>(Resource.Id.edtProductName);
            _edtDescription = FindViewById<EditText>(Resource.Id.edtDescripton);
            _edtCode = FindViewById<EditText>(Resource.Id.edtCode);
            _edtPrice = FindViewById<EditText>(Resource.Id.productPriceEditText);
            _edtStock = FindViewById<EditText>(Resource.Id.edtStock);
            _btnSave = FindViewById<Button>(Resource.Id.saveButton);
            _btnCancel = FindViewById<Button>(Resource.Id.cancelButton);

            _apiService = ApiServiceProvider.Instance;

            _imgProduct.Click += ChooseImage;
            _btnSave.Click += async (s, e) => await SaveProduct();
            _btnCancel.Click += (s, e) => Finish(); 
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

        private async Task SaveProduct()
        {
            if (_imageStream == null)
            {
                Toast.MakeText(this, "Please select an image", ToastLength.Short).Show();
                return;
            }

            var product = new ProductModelRequest
            {
                Name = _edtName.Text,
                ProductCode = _edtCode.Text,
                Description = _edtDescription.Text,
                Price = _edtPrice.Text,
                Stock = _edtStock.Text,
            };

            var error = await _apiService.AddProductAsync(product, _imageStream, "product.jpg");

            if (error == null)
            {
                Toast.MakeText(this, "Add successful", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                _edtName.Error = null;
                _edtPrice.Error = null;
                _edtStock.Error = null;
                _edtDescription.Error = null;

                if (error.Errors.TryGetValue("Name", out var nameErrs))
                    _edtName.Error = string.Join("\n", nameErrs);

                if (error.Errors.TryGetValue("Price", out var priceErrs))
                    _edtPrice.Error = string.Join("\n", priceErrs);

                if (error.Errors.TryGetValue("Stock", out var stockErrs))
                    _edtStock.Error = string.Join("\n", stockErrs);

                if (error.Errors.TryGetValue("Description", out var descErrs))
                    _edtDescription.Error = string.Join("\n", descErrs);

            }
        }

    }

}
