using Android.Content;
using LOMSUI.Services;

namespace LOMSUI.Activities;

[Activity(Label = "FacebookToken")]
public class FacebookTokenActivity : BaseActivity
{
        private EditText etTokenCode, etPageCode;
        private Button updateTokenButton, updatePageButton;
        private ApiService _apiService;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_facebook_token);


        etTokenCode = FindViewById<EditText>(Resource.Id.etTokenCode);
        updateTokenButton = FindViewById<Button>(Resource.Id.updateTokenButton);

        etPageCode = FindViewById<EditText>(Resource.Id.etPageId);
        updatePageButton = FindViewById<Button>(Resource.Id.updatePageButton);

        _apiService = ApiServiceProvider.Instance;


        updatePageButton.Click += async (s, e) =>
        {
            string pageId = etPageCode.Text.Trim();
            await UpdatePage(pageId);
        };

        updateTokenButton.Click += async (s, e) =>
        {
            string token = etTokenCode.Text.Trim();
            await UpdateToken(token);
        };


    }

    public async Task UpdatePage(string pageId)
    {
        var success = await _apiService.UpdateFacebookPageAsync(pageId);
        if (success)
        {
            Toast.MakeText(this, "Page update successful!", ToastLength.Short).Show();
        }
        else
        {
            Toast.MakeText(this, "Error while updating Page.", ToastLength.Short).Show();
        }

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