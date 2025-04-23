using Android;
using Android.Bluetooth;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using LOMSUI.Services;
namespace LOMSUI
{
    [Activity(Label = "Maiin")]
    public class MainActivity : Activity
    {

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

        }
    }
}