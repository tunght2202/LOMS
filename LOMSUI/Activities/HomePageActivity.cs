using Android.Graphics;
using Android.Views;
using LOMSUI.Services;
using System.Globalization;
using Xamarin.Essentials;

namespace LOMSUI.Activities
{
    [Activity(Label = "HomePage", MainLauncher = true)]
    public class HomePageActivity : Activity
    {

        private TextView _txtTotalRevenue, _txtTotalOrders, 
                         _txtOrderCancel, _txtOrderReturn, _txtOrderDelive;
        private ApiService _apiService;
        private TextView _txtStartDate, _txtEndDate;
        private DateTime _startDate = DateTime.Now.AddDays(-7);
        private DateTime _endDate = DateTime.Now;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.homepage);
            BottomNavHelper.SetupFooterNavigation(this, "statistics");

            _txtTotalRevenue = FindViewById<TextView>(Resource.Id.txtTotalRevenue);
            _txtTotalOrders = FindViewById<TextView>(Resource.Id.txtTotalOrders);
            _txtOrderDelive = FindViewById<TextView>(Resource.Id.txtOrderDelivered);
            _txtOrderCancel = FindViewById<TextView>(Resource.Id.txtOrderCancel);
            _txtOrderReturn = FindViewById<TextView>(Resource.Id.txtOrderReturn);
            _txtStartDate = FindViewById<TextView>(Resource.Id.txtStartDate);
            _txtEndDate = FindViewById<TextView>(Resource.Id.txtEndDate);

            _txtStartDate.Text = _startDate.ToString("d/M/yyyy");
            _txtEndDate.Text = _endDate.ToString("d/M/yyyy");


            _apiService = ApiServiceProvider.Instance;

            FindViewById<LinearLayout>(Resource.Id.startDateLayout).Click += (s, e) => ShowDatePicker(true);
            FindViewById<LinearLayout>(Resource.Id.endDateLayout).Click += (s, e) => ShowDatePicker(false);

            LoadRevenueData();
            LoadProductData();

         
        }
        private async Task LoadRevenueData()
        {
            var revenueData = await _apiService.GetRevenueDataAsync();
            var totalOrders = await _apiService.GetTotalOrdersAsync();
            var totalOrderCancel = await _apiService.GetTotalOrdersCancelledAsync();
            var totalOrderReturn = await _apiService.GetTotalOrdersReturnedAsync();
            var totalOrderDelive = await _apiService.GetTotalOrdersDeliveredAsync();



            if (revenueData != null)
            {
                _txtTotalRevenue.Text = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:C0}", revenueData.TotalRevenue);

            }

            if (totalOrders >= 0)
            {
                _txtTotalOrders.Text = $"{totalOrders:N0}";
            }

            if (totalOrderDelive >= 0)
            {
                _txtOrderDelive.Text = $"{totalOrderDelive:N0}";
            }

            if (totalOrderCancel >= 0)
            {
                _txtOrderCancel.Text = $"{totalOrderCancel:N0}";
            }

            if (totalOrderReturn >= 0)
            {
                _txtOrderReturn.Text = $"{totalOrderReturn:N0}";
            }
        }

        private void ShowDatePicker(bool isStart)
        {
            DateTime current = isStart ? _startDate : _endDate;

            DatePickerDialog dialog = new DatePickerDialog(this, (sender, e) =>
            {
                if (isStart)
                {
                    _startDate = e.Date;
                    _txtStartDate.Text = _startDate.ToString("d/M/yyyy");
                }
                else
                {
                    _endDate = e.Date;
                    _txtEndDate.Text = _endDate.ToString("d/M/yyyy");
                }

                if (_startDate <= _endDate)
                {
                    LoadRevenueByDateRange();
                }
                else
                {
                    Toast.MakeText(this, "Start date cannot be greater than end date!", ToastLength.Short).Show();
                }

            }, current.Year, current.Month - 1, current.Day);

            dialog.Show();
        }

        private async void LoadRevenueByDateRange()
        {
            var revenue = await _apiService.GetRevenueByDateRangeAsync(_startDate, _endDate);
            if (revenue != null)
            {
                _txtTotalRevenue.Text = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:C0}", revenue.TotalRevenue);
            }
        }



        private async Task LoadProductData()
        {
            try
            {
                var products = await _apiService.GetAllProductsByUserAsync();
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
                    $"{products[i].Price:N0} đ"
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