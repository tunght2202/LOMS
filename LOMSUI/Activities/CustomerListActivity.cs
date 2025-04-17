using Android.Content;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using LOMSUI.Adapter;
using LOMSUI.Models;
using LOMSUI.Services;

namespace LOMSUI.Activities
{

    [Activity(Label = "CustomerList")]
    public class CustomerListActivity : Activity
    {
        private RecyclerView _recyclerView;
        private TextView _txtNoCustomers;
        private CustomerAdapter _adapter;
        private List<CustomerModel> _customers = new List<CustomerModel>();
        private string userId;
        private ApiService _apiService;
        private SwipeRefreshLayout _swipeRefreshLayout;

        protected override async void OnCreate(Bundle? savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_customer_list);

            BottomNavHelper.SetupFooterNavigation(this);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewCustomers);
            _txtNoCustomers = FindViewById<TextView>(Resource.Id.txtNoCustomers);
            _swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);

            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            _apiService = ApiServiceProvider.Instance;

            _swipeRefreshLayout.Refresh += async (s, e) =>
            {
                await LoadCustomers();
                _swipeRefreshLayout.Refreshing = false;
            };
            await LoadCustomers();
        }

        private async Task LoadCustomers()
        {
            try
            {
                _customers = await _apiService.GetCustomersByUserIdAsync();

                RunOnUiThread(() =>
                {
                    if (_customers.Any())
                    {
                        _adapter = new CustomerAdapter(_customers, this, customer =>
                        {
                            Intent intent = new Intent(this, typeof(CustomerInfoActivity));
                            intent.PutExtra("CustomerID", customer.CustomerID);
                            StartActivity(intent);
                        });

                        _recyclerView.SetAdapter(_adapter);
                        _txtNoCustomers.Visibility = Android.Views.ViewStates.Gone;
                    }
                    else
                    {
                        _txtNoCustomers.Visibility = Android.Views.ViewStates.Visible;
                    }
                });
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Data loading error: " + ex.Message, ToastLength.Long).Show();
            }
        }
    }

}
