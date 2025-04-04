
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    [Table("LiveStreams")]
    public class LiveStream
    {
        [Key]
        public string LivestreamID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        [ForeignKey("ListProduct")]
        public int? ListProductID { get; set; }
        public string StreamURL { get; set; }
        public string StreamTitle { get; set; }
        public DateTime StartTime { get; set; }
        // Neu la true thi da xoa, con neu la false thi chua xoa
        public bool StatusDelete { get; set; } = false;
        public virtual User User { get; set; } 
        public virtual ListProduct ListProduct { get; set; }
        public virtual ICollection<LiveStreamCustomer> LiveStreamCustomers { get; set; }

    }
}
