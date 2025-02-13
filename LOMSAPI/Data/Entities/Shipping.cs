using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOMSAPI.Data.Entities
{
    [Table("Shiping")]
    public class Shipping
    {
        [Key]
        public int ShippingID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Order Order { get; set; }

        public string TrackingNumber { get; set; }
        public string ShippingStatus { get; set; } // Pending, Shipped, Delivered, Failed
    }

}