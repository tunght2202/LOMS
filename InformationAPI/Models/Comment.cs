using System;
using System.Collections.Generic;

namespace InformationAPI.Models;

public partial class Comment
{
    public string CommentId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CommentTime { get; set; }

    public int LiveStreamCustomerId { get; set; }

    public virtual LiveStreamCustomer LiveStreamCustomer { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
