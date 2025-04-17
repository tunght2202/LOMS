using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
namespace LOMSUI.Activities
{
    [Activity(Label = "ListProductActivity")]
    public class ListProductActivity : Activity
    {
        private Button addProductButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_product);

            addProductButton = FindViewById<Button>(Resource.Id.addProductButton);

            addProductButton.Click += (sender, e) =>
            {
                // Navigate to the AddNewProductActivity Activity
                Intent intent = new Intent(this, typeof(AddNewProductActivity));
                StartActivity(intent);
            };
        }
    }
}