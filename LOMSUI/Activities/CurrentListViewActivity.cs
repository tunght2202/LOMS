using Android.App;
using Android.OS;
using Android.Widget;
using LOMSUI.Services;
using LOMSUI.Models;
using System.Collections.Generic;
using Android.Content;
using LOMSUI.Adapter;

namespace LOMSUI.Activities
{
    [Activity(Label = "CurrentListViewActivity")]
    public class CurrentListViewActivity : Activity
    {
        private ListView _currentListsListView;
        private ApiService _apiService;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_currentlistview);

            BottomNavHelper.SetupFooterNavigation(this);

            _currentListsListView = FindViewById<ListView>(Resource.Id.currentListsListView);
            _apiService = new ApiService();

            var prefs = GetSharedPreferences("auth", FileCreationMode.Private);
            string token = prefs.GetString("token", null);
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetToken(token);
            }

            await LoadCurrentListsAsync();

            _currentListsListView.ItemClick += (sender, e) =>
            {
                var adapter = _currentListsListView.Adapter as CurrentListAdapter;
                if (adapter != null)
                {
                    var selectedList = adapter[e.Position];

                    Intent intent = new Intent(this, typeof(ListProductActivity));

                    intent.PutExtra("ListProductId", selectedList.ListProductId);
                    intent.PutExtra("ListName", selectedList.ListProductName); 

                    StartActivity(intent);
                }
            };
        }


        private async Task LoadCurrentListsAsync()
        {
            var currentListsObject = await _apiService.GetAllListProduct();

            RunOnUiThread(() =>
            {
                if (currentListsObject != null)
                {
                    if (currentListsObject is System.Collections.Generic.List<ListProductModel> currentLists)
                    {
                        if (currentLists.Count > 0)
                        {
                            _currentListsListView.Adapter = new CurrentListAdapter(this, currentLists);
                        }
                        else
                        {
                            Toast.MakeText(this, "Không có danh sách nào.", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "Lỗi: Dữ liệu trả về không đúng định dạng.", ToastLength.Long).Show();
                        System.Diagnostics.Debug.WriteLine($"Kiểu dữ liệu trả về: {currentListsObject.GetType()}");
                    }
                }
                else
                {
                    Toast.MakeText(this, "Lỗi khi tải danh sách.", ToastLength.Short).Show();
                }
            });
        }
    }
}