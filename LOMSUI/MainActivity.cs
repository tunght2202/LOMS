using Android.App;
using Android.OS;
using Android.Content;

namespace LOMSUI
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.listofproductsadded);
        }
    }
}