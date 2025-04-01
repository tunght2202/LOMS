using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered
        public Shipping Shipping { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public int LiveStreamCustomerID { get; set; }
        public LiveStreamCustomer LiveStreamCustomer { get; set; }
    }

}