using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "Add Sales List")]
    public class AddSalesListActivity : BaseActivity
    {
        private EditText _etAddListName;
        private Button _btnAdd;
        private ApiService _apiService;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_add_sales_list);

            _apiService = ApiServiceProvider.Instance;

            _etAddListName = FindViewById<EditText>(Resource.Id.etAddListName);
            _btnAdd = FindViewById<Button>(Resource.Id.buttonAdd);

            _btnAdd.Click += async (s, e) => await AddListProduct();
        }

        private async Task AddListProduct()
        { 
            string listName = _etAddListName.Text.Trim();

            if (string.IsNullOrEmpty(listName))
            {
                Toast.MakeText(this, "Please enter a valid name!", ToastLength.Short).Show();
                return;
            }

            bool success = await _apiService.AddNewListProductAsync(listName);

            if (success)
            {
                Toast.MakeText(this, "List Product Add Successfully!", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Failed to add List Product!", ToastLength.Short).Show();
            }
        }

    }
}

