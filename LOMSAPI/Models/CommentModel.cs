namespace LOMSAPI.Models
{
    public class CommentModel
    {
        public string CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.Now;
        public string CustomerAvatar {  get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
    }
}
