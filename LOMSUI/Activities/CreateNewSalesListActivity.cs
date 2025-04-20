using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;

namespace LOMSUI.Activities
{
    [Activity(Label = "CreateNewSalesListActivity")]
    public class CreateNewSalesListActivity : Activity
    {
        private EditText _editTextListName;
        private EditText _editTextDescription;
        private Button _buttonCancel;
        private Button _buttonCreate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_create_new_sales_list_product);

            _editTextListName = FindViewById<EditText>(Resource.Id.editTextListName);
            _editTextDescription = FindViewById<EditText>(Resource.Id.editTextDescription);
            _buttonCancel = FindViewById<Button>(Resource.Id.buttonCancel);
            _buttonCreate = FindViewById<Button>(Resource.Id.buttonCreate);

            _buttonCancel.Click += (sender, e) =>
            {
                Finish();
            };

            _buttonCreate.Click += (sender, e) =>
            {
                string listName = _editTextListName.Text;
                string description = _editTextDescription.Text;

               
                string message = $"Đã yêu cầu tạo danh sách: {listName}";
                if (!string.IsNullOrEmpty(description))
                {
                    message += $"\nMô tả: {description}";
                }
                Toast.MakeText(this, message, ToastLength.Long).Show();

                Finish(); 
                
            };

            BottomNavHelper.SetupFooterNavigation(this);
        }
    }
}