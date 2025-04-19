using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
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
        private EditText _edtName, _edtDescription, _edtPrice, _edtStock;
        private Switch _switchStatus;
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
            _edtPrice = FindViewById<EditText>(Resource.Id.productPriceEditText);
            _edtStock = FindViewById<EditText>(Resource.Id.edtStock);
            _switchStatus = FindViewById<Switch>(Resource.Id.switchStatus);
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
                Android.Net.Uri androidUri = data.Data;
                _imgProduct.SetImageURI(androidUri);
                _imageStream = ContentResolver.OpenInputStream(androidUri);
            }
        }

        private async Task SaveProduct()
        {
            if (_imageStream == null)
            {
                Toast.MakeText(this, "Please select an image", ToastLength.Short).Show();
                return;
            }

            var product = new ProductModel
            {
                Name = _edtName.Text,
                Description = _edtDescription.Text,
                Price = decimal.Parse(_edtPrice.Text),
                Stock = int.Parse(_edtStock.Text),
                Status = _switchStatus.Checked
            };

            var success = await _apiService.AddProductAsync(product, _imageStream, "product.jpg");

            Toast.MakeText(this, success ? "Add successful" : "Add failed", ToastLength.Short).Show();
            if (success) Finish(); 
        }
    }

}
