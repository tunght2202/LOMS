using LOMSAPI.Data.Entities;

namespace LOMSAPI.Models
{
    public class GetOrderDetailByOrderModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered
        public List<OrderDetailModel> OrderDetails { get; set; }
    }

    public class OrderDetailModel 
    {
        public int OrderDetailId { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
