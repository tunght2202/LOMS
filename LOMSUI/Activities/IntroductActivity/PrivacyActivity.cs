namespace LOMSUI.Activities.IntroductActivity;

[Activity(Label = "Privacy Policy")]
public class PrivacyActivity : BaseActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_privacypolicy);
    }
}