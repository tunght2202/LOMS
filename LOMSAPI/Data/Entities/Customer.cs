using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOMSAPI.Data.Entities
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required, MaxLength(255)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public string Address { get; set; }

        public int SuccessfulDeliveries { get; set; } = 0;
        public int FailedDeliveries { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Order> Orders { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

}
