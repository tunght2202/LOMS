using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class HomePageModel
    {
        // Dữ liệu
        public double Revenue { get; set; }
        public int OrderCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Product> ProductList { get; set; }

        // Các phương thức
        public HomePageModel()
        {
            // Khởi tạo dữ liệu mặc định
            Revenue = 0;
            OrderCount = 0;
            StartDate = DateTime.Now.AddDays(-7); // Ví dụ: 7 ngày trước
            EndDate = DateTime.Now;
            ProductList = new List<Product>();
        }

        public void LoadData()
        {
            // Logic để tải dữ liệu từ nguồn (ví dụ: API, cơ sở dữ liệu)
            // ...

            // Ví dụ: Tạo dữ liệu giả định
            Revenue = 1000000;
            OrderCount = 100;

            ProductList.Add(new Product { Id = 1, Name = "Sản phẩm A", Quantity = 10, Price = 100000 });
            ProductList.Add(new Product { Id = 2, Name = "Sản phẩm B", Quantity = 5, Price = 200000 });
            ProductList.Add(new Product { Id = 3, Name = "Sản phẩm C", Quantity = 20, Price = 50000 });
        }

        public void UpdateRevenueAndOrder(DateTime startDate, DateTime endDate)
        {
            // Logic để cập nhật doanh thu và số đơn hàng theo khoảng thời gian
            // ...

            // Ví dụ: Tính toán doanh thu và số đơn hàng giả định
            Revenue = 2000000;
            OrderCount = 200;
        }

        // Các phương thức khác (nếu cần)
        // ...
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
