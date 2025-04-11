using LOMSAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered, Canceled, Returned
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public string? CommentID { get; set; }

    }
}
