using LOMSAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered
        public int LiveStreamCustomerID { get; set; }

    }
}
