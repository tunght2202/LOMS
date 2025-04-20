using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using LOMSUI.Services;

namespace LOMSUI.Activities
{
    [Activity(Label = "CreateNewSalesListActivity")]
    public class CreateNewSalesListActivity : Activity
    {
        private EditText _editTextListName;
        private Button _buttonCancel;
        private Button _buttonCreate;
        private ApiService _apiService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_create_new_sales_list_product);

            _editTextListName = FindViewById<EditText>(Resource.Id.editTextListName);
            _buttonCancel = FindViewById<Button>(Resource.Id.buttonCancel);
            _buttonCreate = FindViewById<Button>(Resource.Id.buttonCreate);
            _apiService = new ApiService();

            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            string token = prefs.GetString("token", null);
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetToken(token);
            }

            _buttonCancel.Click += (sender, e) =>
            {
                Finish();
            };

            _buttonCreate.Click += async (sender, e) =>
            {
                string listName = _editTextListName.Text;

                if (string.IsNullOrWhiteSpace(listName))
                {
                    Toast.MakeText(this, "Vui lòng nhập tên danh sách.", ToastLength.Short).Show();
                    return;
                }

                var response = await _apiService.AddNewListProductAsync(listName);

                if (response != null && response.IsSuccessStatusCode)
                {
                    Toast.MakeText(this, $"Đã tạo danh sách: {listName}", ToastLength.Short).Show();
                    Finish();
                }
                else
                {
                    string errorMessage = "Lỗi khi tạo danh sách.";
                    if (response != null)
                    {
                        errorMessage += $" Mã lỗi: {(int)response.StatusCode}";
                        string errorContent = await response.Content.ReadAsStringAsync();
                        errorMessage += $" Chi tiết: {errorContent}";
                        System.Diagnostics.Debug.WriteLine($"Lỗi tạo danh sách: {errorContent}");
                    }
                    Toast.MakeText(this, errorMessage, ToastLength.Long).Show();
                }
            };

            BottomNavHelper.SetupFooterNavigation(this);
        }
    }
}