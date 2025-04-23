using Android;
using Android.Bluetooth;
using Android.Content.PM;
using Android.OS;
using LOMSUI.Services;
namespace LOMSUI
{
    [Activity(Label = "Print", MainLauncher = true)]
public class MainActivity : Activity
    {
        const int BluetoothPermissionRequestCode = 1001;
        BluetoothAdapter adapter;
        BluetoothDevice device;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitBluetooth();
            var printer = new ThermalPrinterService();

            await printer.ConnectAsync(device);

            var info = new PrintInfo
            {
                MaDonHang = "100036478571801",
                TenKhachHang = "Linh Phạm",
                MaVach = "200300042",
                NgayGio = DateTime.Now,
                DiaChi = "48/15 Phạm Văn Xảo, Quận Tân Phú",
                SoDienThoai = "0373389165"
            };

            await printer.PrintCustomerLabelAsync(info);

        }

        void InitBluetooth()
        {
            adapter = BluetoothAdapter.DefaultAdapter;

            if (adapter == null)
            {
                Toast.MakeText(this, "Thiết bị không hỗ trợ Bluetooth", ToastLength.Long).Show();
                return;
            }

            if (!adapter.IsEnabled)
            {
                var enableBtIntent = new Android.Content.Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBtIntent, 1);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                if (CheckSelfPermission(Manifest.Permission.BluetoothConnect) != Permission.Granted)
                {
                    RequestPermissions(new string[]
                    {
                        Manifest.Permission.BluetoothConnect,
                        Manifest.Permission.BluetoothScan
                    }, BluetoothPermissionRequestCode);
                    return;
                }
            }

            GetPrinterDevice();
        }

        void GetPrinterDevice()
        {
            device = adapter.BondedDevices?.FirstOrDefault(d => d.Name.Contains("PT"));

            if (device != null)
            {
                Toast.MakeText(this, $"Đã tìm thấy máy in: {device.Name}", ToastLength.Short).Show();
                // Ở đây bạn có thể gọi printer.ConnectAsync(device) nếu đã có class BluetoothPrinter
            }
            else
            {
                Toast.MakeText(this, "Không tìm thấy máy in PT đã ghép nối", ToastLength.Short).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == BluetoothPermissionRequestCode)
            {
                if (grantResults.Length > 0 && grantResults.All(r => r == Permission.Granted))
                {
                    Toast.MakeText(this, "Đã cấp quyền Bluetooth", ToastLength.Short).Show();
                    GetPrinterDevice();
                }
                else
                {
                    Toast.MakeText(this, "Không được cấp quyền Bluetooth", ToastLength.Long).Show();
                }
            }
        }
    }
}
