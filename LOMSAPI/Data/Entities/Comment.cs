using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public string CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.Now;
        public int LiveStreamCustomerID { get; set; }
        public LiveStreamCustomer LiveStreamCustomer { get; set; }
    }

}
