using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    [Table("LiveStreams")]
    public class LiveStream
    {
        [Key]
        public int LivestreamID { get; set; }

        [ForeignKey("User")] // Foreign key đến bảng Users của Identity
        public string UserID { get; set; } // Chú ý kiểu dữ liệu phải tương thích với Id của Users
        public string StreamURL { get; set; }
        public string StreamTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual User User { get; set; } // Navigation property
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
