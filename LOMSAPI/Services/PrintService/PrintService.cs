using System.Drawing;
using System.Text;
using System;
using System.IO.Ports;
using static System.Net.Mime.MediaTypeNames;
namespace LOMSAPI.Services
{
    public class PrintInfo
    {
        public string MaSo { get; set; }
        public string TenKhach { get; set; }
        public DateTime? ThoiGian { get; set; }
        public string NoiDungCommment { get; set; }
        public string SanPham { get; set; }
        public string TongGia { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public int Stock { get; set; }
    }
    public class PrintService : IPrintService
    {


        public void PrintCustomerLabel(string comPort, PrintInfo info)
        {
            using (SerialPort port = new SerialPort(comPort, 9600))
            {
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                port.Encoding = Encoding.UTF8;
                port.Open();

                // Reset máy in
                port.Write(new byte[] { 0x1B, 0x40 }, 0, 2);

                // Tạo ảnh từ dữ liệu
                Bitmap bitmap = CreateCustomerLabel(info);
                byte[] imageData = ConvertBitmapToESC_POS(bitmap);
                port.Write(imageData, 0, imageData.Length);
                port.Write("\n\n\n");

                port.Close();
            }
        }
        public static byte[] ConvertBitmapToESC_POS(Bitmap bitmap)
        {
            List<byte> data = new List<byte>();

            // ESC * m nL nH d1...dk
            // Gửi lệnh mỗi dòng ảnh, theo chiều dọc
            int width = bitmap.Width;
            int height = bitmap.Height;

            for (int y = 0; y < height; y += 24)
            {
                data.AddRange(new byte[] { 0x1B, 0x2A, 33, (byte)(width % 256), (byte)(width / 256) });

                for (int x = 0; x < width; x++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; b++)
                        {
                            int yy = y + (k * 8) + b;
                            if (yy >= height) continue;

                            Color pixelColor = bitmap.GetPixel(x, yy);
                            int luminance = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);

                            if (luminance < 128)
                            {
                                slice |= (byte)(1 << (7 - b));
                            }
                        }
                        data.Add(slice);
                    }
                }
                // Xuống dòng
                data.Add(0x0A);
            }

            return data.ToArray();
        }

        public static Bitmap CreateCustomerLabel(PrintInfo info)
        {
            int width = 384;
            Bitmap bitmap = new Bitmap(width, 350);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                int y = 10;
                StringFormat center = new StringFormat() { Alignment = StringAlignment.Center };

                // Các font cần dùng
                var fontNormal = new System.Drawing.Font("Arial", 14, FontStyle.Regular);
                var fontBold = new System.Drawing.Font("Arial", 25, FontStyle.Bold);
                var fontBig = new System.Drawing.Font("Arial", 25, FontStyle.Regular);

                void DrawLine(string text, System.Drawing.Font font)
                {
                    if (string.IsNullOrWhiteSpace(text)) return;
                    g.DrawString(text, font, Brushes.Black, new RectangleF(0, y, width, 40), center);
                    y += (int)font.GetHeight(g) + 5;
                }

                DrawLine(info.MaSo, fontBold);
                DrawLine(info.TenKhach, fontBold);
                DrawLine(info.ThoiGian?.ToString("dd/MM/yyyy HH:mm"), fontNormal);
                var noiDungComment = info.NoiDungCommment;
                if (noiDungComment.Length > 24)
                {
                    DrawLine(noiDungComment.Substring(0, 20), fontBig);
                    DrawLine(noiDungComment.Substring(20), fontBig);
                }
                else
                {
                    DrawLine(noiDungComment, fontBig);
                }
                    var product = info.SanPham;
                if (!string.IsNullOrWhiteSpace(product))
                { 

                    if (product.Length > 24)
                    {
                        DrawLine(product.Substring(0, 20), fontBig);
                        DrawLine(product.Substring(20), fontBig);
                    }
                    else
                    {
                        DrawLine(product, fontBig);
                    }
                }
                if (!string.IsNullOrWhiteSpace(product))
                {
                    DrawLine("Total: " + info.TongGia, fontBold);
                }
                if (info.Stock != null)
                {
                    DrawLine("Stock: " + info.Stock, fontNormal);
                }
                if (!string.IsNullOrWhiteSpace(info.DiaChi))
                {
                    // Có thể chia thành 2 dòng nếu dài
                    var diaChiParts = info.DiaChi.Split(new[] { ',' }, 2);
                    DrawLine("ĐC: " + diaChiParts[0], fontNormal);
                    if (diaChiParts.Length > 1) DrawLine(diaChiParts[1], fontNormal);
                }

                if (!string.IsNullOrWhiteSpace(info.SoDienThoai))
                {
                    DrawLine("SĐT " + info.SoDienThoai, fontNormal);
                }

            }

            return bitmap;
        }

    }
}

