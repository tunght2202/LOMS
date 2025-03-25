using System;

namespace LOMSUI.Models
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string Content { get; set; }
        public string CreateTime { get; set; }
    }

}
