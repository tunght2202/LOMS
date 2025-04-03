using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    [Table("LiveStreamCustomers")]
    public class LiveStreamCustomer
    {
        [Key]
        public int LiveStreamCustomerId { get; set; }
        [ForeignKey("LiveStream")]
        public string LivestreamID { get; set; }
        public LiveStream LiveStream { get; set; }
        [ForeignKey("Customer")]
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Order Order { get; set; }

    }
}
