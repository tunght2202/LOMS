﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("LiveStream")]
        public int LiveStreamID { get; set; }
        public LiveStream LiveStream { get; set; }

        public string Content { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.Now;
    }

}
