namespace LOMSUI.Activities;

[Activity(Label = "About Us")]
public class AboutActivity : BaseActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_aboutt);
    }
}