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

namespace LOMSUI.Activities
{
    [Activity(Label = "AddNewProductActivity", MainLauncher =true)]
    public class AddNewProductActivity : Activity
    {
        private ImageView productImageView;
        private EditText productNameEditText;
        private EditText productDescriptionEditText;
        private EditText productPriceEditText;
        private EditText productDiscountEditText;
        private EditText productQuantityEditText;
        private Button cancelButton;
        private Button saveButton;
        private ImageView backButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.addnewproduct);

            productImageView = FindViewById<ImageView>(Resource.Id.productImageView);
            productNameEditText = FindViewById<EditText>(Resource.Id.productNameEditText);
            productDescriptionEditText = FindViewById<EditText>(Resource.Id.productDescriptionEditText);
            productPriceEditText = FindViewById<EditText>(Resource.Id.productPriceEditText);
            productDiscountEditText = FindViewById<EditText>(Resource.Id.productDiscountEditText);
            productQuantityEditText = FindViewById<EditText>(Resource.Id.productQuantityEditText);
            cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            backButton = FindViewById<ImageView>(Resource.Id.backButton);
           
           

            productImageView.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Chọn ảnh sản phẩm", ToastLength.Short).Show();
            };

            cancelButton.Click += (sender, e) =>
            {
                Finish(); 
            };

            saveButton.Click += (sender, e) =>
            {
                string name = productNameEditText.Text;
                string description = productDescriptionEditText.Text;
                decimal price = decimal.Parse(productPriceEditText.Text);
                decimal discount = decimal.Parse(productDiscountEditText.Text);
                int quantity = int.Parse(productQuantityEditText.Text);

                var product = new AddNewProductModel
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Discount = discount,
                    Quantity = quantity,
                };

               

                Toast.MakeText(this, "Sản phẩm đã được lưu", ToastLength.Short).Show();
                Finish(); 
            };

            backButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(ListProductActivity));
                StartActivity(intent);
                Finish();
            };
        }
    }
}
