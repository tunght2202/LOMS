using Android.App;
using Android.OS;
using Android.Content;
using Android.Bluetooth;
using LOMSUI.Models;

namespace LOMSUI
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private ThermalPrinterService printer = new ThermalPrinterService();
        private BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);

            Button btnIn = FindViewById<Button>(Resource.Id.btnPrint);
            btnIn.Click += async (s, e) =>
            {
                var device = adapter.BondedDevices.FirstOrDefault(d => d.Name.Contains("PT"));
                if (device == null)
                {
                    Toast.MakeText(this, "Không tìm thấy máy in PT", ToastLength.Long).Show();
                    return;
                }

                await printer.ConnectAsync(device);

                var info = new PrintInfoModel
                {
                    MaDonHang = "100036478571801",
                    TenKhachHang = "Linh Phạm",
                    MaVach = "200300042",
                    NgayGio = DateTime.Now,
                    DiaChi = "48/15 Phạm Văn Xảo, Quận Tân Phú",
                    SoDienThoai = "0373389165"
                };

                await printer.PrintCustomerLabelAsync(info);
            };
        }
    }
}