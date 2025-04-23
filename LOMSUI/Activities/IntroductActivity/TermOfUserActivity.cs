namespace LOMSUI.Activities.IntroductActivity;

[Activity(Label = "TermOfUser")]
public class TermOfUserActivity : BaseActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_termofuse);
    }
}