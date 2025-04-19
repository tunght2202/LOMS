using Android.Views;
using AndroidX.AppCompat.App;

namespace LOMSUI.Activities;

[Activity(Label = "BaseActivity")]
public class BaseActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SupportActionBar?.SetDisplayHomeAsUpEnabled(true);
        SupportActionBar?.SetHomeButtonEnabled(true);
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        if (item.ItemId == Android.Resource.Id.Home)
        {
            Finish();
            return true;
        }
        return base.OnOptionsItemSelected(item);
    }
}
