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
        public static void SetupFooterNavigation(Activity activity, string currentTab)
        {
            var statisticsLayout = activity.FindViewById<LinearLayout>(Resource.Id.statisticsLayout);
            var sellLayout = activity.FindViewById<LinearLayout>(Resource.Id.sellLayout);
            var productsLayout = activity.FindViewById<LinearLayout>(Resource.Id.productsLayout);
            var customersLayout = activity.FindViewById<LinearLayout>(Resource.Id.customersLayout);
            var menuLayout = activity.FindViewById<LinearLayout>(Resource.Id.menuLayout);

            var layouts = new Dictionary<string, LinearLayout>
    {
        { "statistics", statisticsLayout },
        { "sell", sellLayout },
        { "products", productsLayout },
        { "customers", customersLayout },
        { "menu", menuLayout }
    };

            foreach (var layout in layouts.Values)
            {
                layout.Selected = false;
            }

            if (layouts.ContainsKey(currentTab))
            {
                layouts[currentTab].Selected = true;
            }

            statisticsLayout.Click += (sender, e) =>
            {
                if (currentTab != "statistics")
                {
                    var intent = new Intent(activity, typeof(HomePageActivity));
                    activity.StartActivity(intent);
                }
            };

            sellLayout.Click += (sender, e) =>
            {
                if (currentTab != "sell")
                {
                }
            };

            productsLayout.Click += (sender, e) =>
            {
                if (currentTab != "products")
                {
                    var intent = new Intent(activity, typeof(ProductActivity));
                    activity.StartActivity(intent);
                }
            };

            customersLayout.Click += (sender, e) =>
            {
                if (currentTab != "customers")
                {
                    var intent = new Intent(activity, typeof(CustomerListActivity));
                    activity.StartActivity(intent);
                }
            };

            menuLayout.Click += (sender, e) =>
            {
                if (currentTab != "menu")
                {
                    var intent = new Intent(activity, typeof(MenuActivity));
                    activity.StartActivity(intent);
                }
            };
        }


    }

}
