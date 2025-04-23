using Android.Bluetooth;
using Android.Graphics;
using Java.Util;
using System.Text;
namespace LOMSUI.Services
{
    public class PrintInfo
    {
        public string MaDonHang { get; set; }
        public string TenKhachHang { get; set; }
        public string MaVach { get; set; }
        public DateTime NgayGio { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
    }
    public class ThermalPrinterService
    {
        private BluetoothSocket socket;

        public async Task ConnectAsync(BluetoothDevice device)
        {
            socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
            await socket.ConnectAsync();
        }

        public async Task PrintCustomerLabelAsync(PrintInfo info)
        {
            Bitmap bitmap = CreateBitmap(info);
            byte[] data = ConvertBitmapToESCPOS(bitmap);
            await socket.OutputStream.WriteAsync(data, 0, data.Length);
        }

        public void Disconnect()
        {
            socket?.Close();
        }

        private Bitmap CreateBitmap(PrintInfo info)
        {
            int width = 384;
            int height = 600;
            Bitmap bmp = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bmp);
            canvas.DrawColor(Color.White);

            Paint paint = new Paint { Color = Color.Black, TextSize = 28, AntiAlias = true };
            Paint bold = new Paint(paint) { FakeBoldText = true };

            float y = 40;
            float centerX = width / 2f;

            void DrawCenter(string text, Paint p, float sizeInc = 0)
            {
                if (string.IsNullOrWhiteSpace(text)) return;
                p.TextSize += sizeInc;
                var textWidth = p.MeasureText(text);
                canvas.DrawText(text, centerX - textWidth / 2, y, p);
                y += p.TextSize + 10;
                p.TextSize -= sizeInc;
            }

            // Nội dung
            DrawCenter("Cửa hàng Tinh Hoa", bold);
            DrawCenter(info.MaDonHang, bold);
            DrawCenter(info.TenKhachHang, bold);
            DrawCenter(info.MaVach, bold);
            DrawCenter(info.NgayGio.ToString("dd/MM/yyyy HH:mm"), paint);
            if (!string.IsNullOrWhiteSpace(info.DiaChi))
                DrawCenter("ĐC: " + info.DiaChi, paint);
            if (!string.IsNullOrWhiteSpace(info.SoDienThoai))
                DrawCenter("SĐT: " + info.SoDienThoai, paint);
            DrawCenter("TPOS.VN", paint);
            DrawCenter("Cảm ơn quý khách!", bold, 10);

            return bmp;
        }

        private byte[] ConvertBitmapToESCPOS(Bitmap bitmap)
        {
            var bytes = new List<byte>();

            bitmap = Bitmap.CreateScaledBitmap(bitmap, 384, bitmap.Height, false);

            // ESC/POS commands
            bytes.AddRange(new byte[] { 0x1B, 0x40 }); // Reset
            bytes.AddRange(DecodeBitmap(bitmap));
            bytes.AddRange(new byte[] { 0x1D, 0x56, 0x42, 0x10 }); // Cut

            return bytes.ToArray();
        }

        // Convert Bitmap to ESC/POS raster format
        private List<byte> DecodeBitmap(Bitmap bmp)
        {
            List<byte> list = new List<byte>();
            int width = bmp.Width;
            int height = bmp.Height;

            list.AddRange(new byte[] { 0x1D, 0x76, 0x30, 0x00, (byte)(width / 8), 0x00, (byte)(height % 256), (byte)(height / 256) });

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x += 8)
                {
                    byte b = 0;
                    for (int bit = 0; bit < 8; bit++)
                    {
                        int pixel = x + bit < width ? bmp.GetPixel(x + bit, y) : Color.White;
                        int r = Color.GetRedComponent(pixel);
                        int g = Color.GetGreenComponent(pixel);
                        int bl = Color.GetBlueComponent(pixel);
                        int gray = (r + g + bl) / 3;
                        if (gray < 128)
                            b |= (byte)(1 << (7 - bit));
                    }
                    list.Add(b);
                }
            }
            return list;
        }
    }

}
