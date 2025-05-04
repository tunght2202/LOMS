using Android.Graphics;
using Android.Views;
using LOMSUI.Services;
using System.Globalization;
using Xamarin.Essentials;

namespace LOMSUI.Activities
{
    [Activity(Label = "HomePage")]
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

            InitViews();

            _apiService = ApiServiceProvider.Instance;

            Task.Run(async () =>
            {
                await LoadRevenueData();
                await LoadProductData();
            });


        }

        private void InitViews()
        {
            _txtTotalRevenue = FindViewById<TextView>(Resource.Id.txtTotalRevenue);
            _txtTotalOrders = FindViewById<TextView>(Resource.Id.txtTotalOrders);
            _txtOrderDelive = FindViewById<TextView>(Resource.Id.txtOrderDelivered);
            _txtOrderCancel = FindViewById<TextView>(Resource.Id.txtOrderCancel);
            _txtOrderReturn = FindViewById<TextView>(Resource.Id.txtOrderReturn);
            _txtStartDate = FindViewById<TextView>(Resource.Id.txtStartDate);
            _txtEndDate = FindViewById<TextView>(Resource.Id.txtEndDate);

            _txtStartDate.Text = _startDate.ToString("d/M/yyyy");
            _txtEndDate.Text = _endDate.ToString("d/M/yyyy");

            FindViewById<LinearLayout>(Resource.Id.startDateLayout).Click += (s, e) => ShowDatePicker(true);
            FindViewById<LinearLayout>(Resource.Id.endDateLayout).Click += (s, e) => ShowDatePicker(false);
        }

        private async Task LoadRevenueData()
        {
            try
            {
                var revenueTask = _apiService.GetRevenueByDateRangeAsync(_startDate, _endDate);
                var totalOrdersTask = _apiService.GetTotalOrdersAsync(_startDate, _endDate);
                var cancelTask = _apiService.GetTotalOrdersCancelledAsync(_startDate, _endDate);
                var returnTask = _apiService.GetTotalOrdersReturnedAsync(_startDate, _endDate);
                var deliveredTask = _apiService.GetTotalOrdersDeliveredAsync(_startDate, _endDate);

                await Task.WhenAll(revenueTask, totalOrdersTask, cancelTask, returnTask, deliveredTask);

                var revenueData = revenueTask.Result;
                var totalOrders = totalOrdersTask.Result;
                var totalOrderCancel = cancelTask.Result;
                var totalOrderReturn = returnTask.Result;
                var totalOrderDelive = deliveredTask.Result;

                RunOnUiThread(() =>
                {
                    if (revenueData != null)
                    {
                        _txtTotalRevenue.Text = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:C0}", revenueData.TotalRevenue);
                    }

                    _txtTotalOrders.Text = $"{totalOrders:N0}";
                    _txtOrderDelive.Text = $"{totalOrderDelive:N0}";
                    _txtOrderCancel.Text = $"{totalOrderCancel:N0}";
                    _txtOrderReturn.Text = $"{totalOrderReturn:N0}";
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading revenue data: " + ex.Message);
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
                    LoadRevenueData(); 
                }
                else
                {
                    Toast.MakeText(this, "Start date cannot be greater than end date!", ToastLength.Short).Show();
                }

            }, current.Year, current.Month - 1, current.Day);

            dialog.Show();
        }


        private async Task LoadProductData()
        {
            try
            {
                var products = await _apiService.GetAllProductsByUserAsync();
                TableLayout productDataTable = FindViewById<TableLayout>(Resource.Id.productDataTable);
                if (products == null || products.Count == 0) return;

                RunOnUiThread(() =>
                {
                    productDataTable.RemoveAllViews(); 

                    for (int i = 0; i < products.Count; i++)
                    {
                        productDataTable.AddView(CreateTableRow(new[]
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