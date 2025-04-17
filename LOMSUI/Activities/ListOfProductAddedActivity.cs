using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using LOMSUI.Models;

namespace LOMSUI.Activities
{
    [Activity(Label = "ListOfProductAddedActivity", MainLauncher =true)]
    public class ListOfProductAddedActivity : Activity
    {
        private ListView productsListView;
        private Button currentListButton;
        private Button createSaleListButton;
        private ImageView backButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.listofproductsadded);

            productsListView = FindViewById<ListView>(Resource.Id.productsListView);
            currentListButton = FindViewById<Button>(Resource.Id.currentListButton);
            createSaleListButton = FindViewById<Button>(Resource.Id.createSaleListButton);
            backButton = FindViewById<ImageView>(Resource.Id.backButton);

            // Tạo dữ liệu mẫu cho ListView
            List<ListOfProductAddedModel> productList = new List<ListOfProductAddedModel>
            {
                new ListOfProductAddedModel { STT = 1, Name = "bé 3", ImagePath = "image1", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 2, Name = "bé 3", ImagePath = "image2", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 3, Name = "bé 3", ImagePath = "image3", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 4, Name = "bé 3", ImagePath = "image4", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 5, Name = "bé 3", ImagePath = "image5", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 6, Name = "bé 3", ImagePath = "image6", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 7, Name = "bé 3", ImagePath = "image7", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 8, Name = "bé 3", ImagePath = "image8", Price = 1000000, Quantity = 10000 },
                new ListOfProductAddedModel { STT = 9, Name = "bé 3", ImagePath = "image9", Price = 1000000, Quantity = 10000 },
            };

            // Tạo Adapter cho ListView
            ProductListAdapter adapter = new ProductListAdapter(this, productList);
            productsListView.Adapter = adapter;

            // Xử lý sự kiện click cho nút "Danh sách hiện tại"
            currentListButton.Click += (sender, e) =>
            {
                // Xử lý sự kiện click cho nút "Danh sách hiện tại"
                Toast.MakeText(this, "Danh sách hiện tại", ToastLength.Short).Show();
            };

            // Xử lý sự kiện click cho nút "Tạo danh sách bán hàng"
            createSaleListButton.Click += (sender, e) =>
            {
                // Xử lý sự kiện click cho nút "Tạo danh sách bán hàng"
                Toast.MakeText(this, "Tạo danh sách bán hàng", ToastLength.Short).Show();
            }; 

            // Xử lý sự kiện click cho nút back
            backButton.Click += (sender, e) =>
            {
                Finish(); // Đóng Activity
            };
        }

        // Adapter cho ListView
        private class ProductListAdapter : BaseAdapter<ListOfProductAddedModel>
        {
            private List<ListOfProductAddedModel> productList;
            private Context context;

            public ProductListAdapter(Context context, List<ListOfProductAddedModel> productList)
            {
                this.context = context;
                this.productList = productList;
            }

            public override ListOfProductAddedModel this[int position] => productList[position];

            public override int Count => productList.Count;

            public override long GetItemId(int position) => position;

            public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
            {
                Android.Views.View view = convertView;
                if (view == null)
                {
                    view = Android.Views.LayoutInflater.From(context).Inflate(Resource.Layout.activity_product, null);
                }

                ListOfProductAddedModel product = productList[position];

                TextView sttTextView = view.FindViewById<TextView>(Resource.Id.sttHeaderTextView);
                TextView nameTextView = view.FindViewById<TextView>(Resource.Id.productNameHeaderTextView);
                ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageHeaderTextView);
                TextView priceTextView = view.FindViewById<TextView>(Resource.Id.priceHeaderTextView);
                TextView quantityTextView = view.FindViewById<TextView>(Resource.Id.quantityHeaderTextView);
                //Button addButton = view.FindViewById<Button>(Resource.Id.addButton);
                //Button editButton = view.FindViewById<Button>(Resource.Id.editButton);
                //Button deleteButton = view.FindViewById<Button>(Resource.Id.deleteButton);

                sttTextView.Text = product.STT.ToString();
                nameTextView.Text = product.Name;
                // Load ảnh từ đường dẫn product.ImagePath vào imageView
                priceTextView.Text = product.Price.ToString();
                quantityTextView.Text = product.Quantity.ToString();

                //addButton.Click += (sender, e) =>
                //{
                //};

                //editButton.Click += (sender, e) =>
                //{
                //};

                //deleteButton.Click += (sender, e) =>
                //{
                //};

                return view;
            }
        }
    }
}