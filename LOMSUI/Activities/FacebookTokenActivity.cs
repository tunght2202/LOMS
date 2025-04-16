namespace LOMSUI.Activities;

[Activity(Label = "FacebookTokenActivity")]
public class FacebookTokenActivity : Activity
{
    private EditText etTokenCode;
    private Button updateTokenButton;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_facebook_token);

        BottomNavHelper.SetupFooterNavigation(this);

        etTokenCode = FindViewById<EditText>(Resource.Id.etTokenCode);
        updateTokenButton = FindViewById<Button>(Resource.Id.updateTokenButton);

        updateTokenButton.Click += (s, e) =>
        {

        };


    }
}