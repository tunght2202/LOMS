using LOMSUI.Services;

namespace LOMSUI.Activities;

[Activity(Label = "FaceBookPageActivity")]
public class FaceBookPageActivity : Activity
{
    private EditText etPageCode;
    private Button updatePageButton;
    private ApiService _apiService;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_facebook_page);

        etPageCode = FindViewById<EditText>(Resource.Id.etPageId);
        updatePageButton = FindViewById<Button>(Resource.Id.updatePageButton);

        _apiService = ApiServiceProvider.Instance;


        updatePageButton.Click += async (s, e) =>
        {
            
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
}