namespace LOMSUI
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
<<<<<<< Updated upstream
            SetContentView(Resource.Layout.activity_login);
=======
            SetContentView(Resource.Layout.activity_comments);
>>>>>>> Stashed changes
        }
    }
}