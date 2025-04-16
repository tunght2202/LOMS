using Android.Graphics;
using Android.Views;
using LOMSUI.Services;
using System.Globalization;

namespace LOMSUI.Activities
{
    [Activity(Label = "HomePage", MainLauncher = true)]
    public class HomePageActivity : Activity
    {
      /*  LinearLayout bottomNavLayout;
        LinearLayout thongKeLayout;
        LinearLayout banHangLayout;
        LinearLayout sanPhamLayout;
        LinearLayout khachHangLayout;
        LinearLayout menuLayout;*/
        private TextView _txtTotalRevenue, _txtTotalOrders;
        private ApiService _apiService;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.homepage);

          /*  bottomNavLayout = FindViewById<LinearLayout>(Resource.Id.bottomNavLayout);
            thongKeLayout = FindViewById<LinearLayout>(Resource.Id.thongKeLayout);
            banHangLayout = FindViewById<LinearLayout>(Resource.Id.banHangLayout);
            sanPhamLayout = FindViewById<LinearLayout>(Resource.Id.sanPhamLayout);
            khachHangLayout = FindViewById<LinearLayout>(Resource.Id.khachHangLayout);
            menuLayout = FindViewById<LinearLayout>(Resource.Id.menuLayout);
*/
            _txtTotalRevenue = FindViewById<TextView>(Resource.Id.txtTotalRevenue);
            _txtTotalOrders = FindViewById<TextView>(Resource.Id.txtTotalOrders);

            _apiService = ApiServiceProvider.Instance;

             LoadRevenueData();
            LoadProductData();

          /*  thongKeLayout.Click += (sender, e) =>
            {
                
            };

            banHangLayout.Click += (sender, e) =>
            {
                
            };*/

            /*      sanPhamLayout.Click += (sender, e) =>
                  {
                      Intent intent = new Intent(this, typeof(ProductActivity));
                      StartActivity(intent);
                  };

                  khachHangLayout.Click += (sender, e) =>
                  {
                      Intent intent = new Intent(this, typeof(CustomerListActivity));
                      StartActivity(intent);
                  };

                  menuLayout.Click += (sender, e) =>
                  {
                      Intent intent = new Intent(this, typeof(MenuActivity));
                      StartActivity(intent);
                  };
      */
        }
        private async Task LoadRevenueData()
        {
            var revenueData = await _apiService.GetRevenueDataAsync();
            var totalOrders = await _apiService.GetTotalOrdersAsync();

            if (revenueData != null)
            {
                _txtTotalRevenue.Text = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:C0}", revenueData.TotalRevenue);

            }

            if (totalOrders >= 0)
            {
                _txtTotalOrders.Text = $"{totalOrders:N0} đơn hàng";
            }
        }

        private async Task LoadProductData()
        {
            try
            {
                var products = await _apiService.GetAllproduct();
                TableLayout productTable = FindViewById<TableLayout>(Resource.Id.productListLayout);
                if (products == null || products.Count == 0) return;

                RunOnUiThread(() =>
                {
                    productTable.RemoveAllViews(); 

                    productTable.AddView(CreateTableRow(new[] { "STT", "Name", "Quantity", "Price" }, true));

                    for (int i = 0; i < products.Count; i++)
                    {
                        productTable.AddView(CreateTableRow(new[]
                        {
                    $"{i + 1}",
                    products[i].Name,
                    products[i].Stock.ToString(),
                    $"{products[i].Price:N0} VND"
                }));
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading products: {ex.Message}");
            }
        }
        private TableRow CreateTableRow(string[] values, bool isHeader = false)
        {
            TableRow row = new TableRow(this);
            row.LayoutParameters = new TableLayout.LayoutParams(
                TableLayout.LayoutParams.MatchParent,
                TableLayout.LayoutParams.WrapContent
            );

            float[] weights = { 1f, 3f, 2f, 2f }; 
            for (int i = 0; i < values.Length; i++)
            {
                TextView cell = new TextView(this) { Text = values[i] };
                cell.SetPadding(8, 8, 8, 8);
                cell.Gravity = GravityFlags.Center;
                cell.LayoutParameters = new TableRow.LayoutParams(0, TableRow.LayoutParams.WrapContent, weights[i]);

                if (isHeader) cell.SetTypeface(null, TypefaceStyle.Bold);

                row.AddView(cell);
            }
            return row;
        }



    }
}