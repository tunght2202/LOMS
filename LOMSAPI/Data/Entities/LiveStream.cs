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
        public string StreamURL { get; set; }
        public string StreamTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool Status { get; set; } = true;

        public virtual User User { get; set; } 
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<LiveStreamProduct> LiveStreamProducts { get; set; }

    }
}
