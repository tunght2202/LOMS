namespace LOMSUI.Activities.IntroductActivity;

[Activity(Label = "HelpActivity")]
public class HelpActivity : BaseActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_help);
    }
}