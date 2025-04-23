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

            var thongTin = new PrintInfo
            {
                MaSo = "DH123456789012345678901234567890123456",
                TenKhach = "Nguyễn Văn A",
                ThoiGian = DateTime.Now,
                SanPham = "Áo dài tay, Size M",
                TongGia = "350.000đ",
                DiaChi = "123 Lê Lợi, Q1, TP.HCM",
                SoDienThoai = "0987654321",
                NoiDungCommment = "Giao hàng giờ hành chính"
            };


            await printer.PrintCustomerLabelAsync(thongTin);

            var thongTin2 = new PrintInfo
            {
                TenKhach = "Nguyễn Văn A",
                ThoiGian = DateTime.Now,
                NoiDungCommment = "Giao hàng giờ hành chính"
            };


            await printer.PrintCustomerLabelAsync(thongTin2);

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
            var device = adapter?.BondedDevices?.FirstOrDefault(d => d.Name.Contains("PT"));

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
