using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Canceled,
        Returned
    }
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } // Pending, Confirmed, Shipped, Delivered, Canceled, Returned
        public int Quantity { get; set; }
        public decimal? CurrentPrice { get; set; }
        public bool StatusCheck { get; set; } = false;
        public string? TrackingNumber { get; set; }
        public string? Note { get; set; }
        public int ProductID { get; set; }
        public string? CommentID { get; set; }
        public Product Product { get; set; }
        public Comment Comment { get; set; }
    }

}