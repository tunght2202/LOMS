using Android.Content;
using LOMSUI.Services;

namespace LOMSUI.Activities;

[Activity(Label = "FacebookToken")]
public class FacebookTokenActivity : BaseActivity
{
        private EditText etTokenCode;
        private Button updateTokenButton;
        private ApiService _apiService;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_facebook_token);


        etTokenCode = FindViewById<EditText>(Resource.Id.etTokenCode);
        updateTokenButton = FindViewById<Button>(Resource.Id.updateTokenButton);

        _apiService = ApiServiceProvider.Instance;


        updateTokenButton.Click += async (s, e) =>
        {
            string token = etTokenCode.Text.Trim();
            await UpdateToken(token);
        };


    }

    public async Task UpdateToken(string token)
    {
        var success = await _apiService.UpdateFacebookTokenAsync(token);
        if (success)
        {
            Toast.MakeText(this, "Token update successful!", ToastLength.Short).Show();
        }
        else
        {
            Toast.MakeText(this, "Error while updating token.", ToastLength.Short).Show();
        }

    }


}