using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics; // Nếu bạn muốn lưu trữ hình ảnh dưới dạng Bitmap

namespace LOMSUI.Activities
{
    public class ManagerListOrderProductModel
    {
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public Bitmap ProductImage { get; set; } 
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string TotalPrice { get; set; }

        public ManagerListOrderProductModel()
        {
        }

        public ManagerListOrderProductModel(string customerName, string status, Bitmap productImage, string productName, string productPrice, string totalPrice)
        {
            CustomerName = customerName;
            Status = status;
            ProductImage = productImage;
            ProductName = productName;
            ProductPrice = productPrice;
            TotalPrice = totalPrice;
        }

        
    }
}