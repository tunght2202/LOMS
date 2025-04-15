using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Activities
{
    public static class BottomNavHelper
    {
        public static void SetupFooterNavigation(Activity activity)
        {
            var statisticsLayout = activity.FindViewById<LinearLayout>(Resource.Id.statisticsLayout);
            var sellLayout = activity.FindViewById<LinearLayout>(Resource.Id.sellLayout);
            var productsLayout = activity.FindViewById<LinearLayout>(Resource.Id.productsLayout);
            var customersLayout = activity.FindViewById<LinearLayout>(Resource.Id.customersLayout);
            var menuLayout = activity.FindViewById<LinearLayout>(Resource.Id.menuLayout);

            statisticsLayout.Click += (sender, e) =>
            {
                
            };

            sellLayout.Click += (sender, e) =>
            {
               
            };

            productsLayout.Click += (sender, e) =>
            {
               
            };

            customersLayout.Click += (sender, e) =>
            {
                var intent = new Intent(activity, typeof(CustomerListActivity));
                activity.StartActivity(intent);
            };

            menuLayout.Click += (sender, e) =>
            {
                var intent = new Intent(activity, typeof(MenuActivity));
                activity.StartActivity(intent);
            };
        }
    }

}
