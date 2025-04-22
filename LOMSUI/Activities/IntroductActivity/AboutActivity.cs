namespace LOMSUI.Activities;

[Activity(Label = "AboutActivity")]
public class AboutActivity : BaseActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_aboutt);
    }
}