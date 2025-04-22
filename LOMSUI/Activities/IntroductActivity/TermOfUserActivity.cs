namespace LOMSUI.Activities.IntroductActivity;

[Activity(Label = "TermOfUserActivity")]
public class TermOfUserActivity : BaseActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_termofuse);
    }
}