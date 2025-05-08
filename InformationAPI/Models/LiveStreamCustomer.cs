using System;
using System.Collections.Generic;

namespace InformationAPI.Models;

public partial class LiveStreamCustomer
{
    public int LiveStreamCustomerId { get; set; }

    public string LivestreamId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual LiveStream Livestream { get; set; } = null!;
}
