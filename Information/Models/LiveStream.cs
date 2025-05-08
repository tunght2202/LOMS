using System;
using System.Collections.Generic;

namespace Information.Models;

public partial class LiveStream
{
    public string LivestreamId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public int? ListProductId { get; set; }

    public string StreamUrl { get; set; } = null!;

    public string StreamTitle { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public string Status { get; set; } = null!;

    public bool StatusDelete { get; set; }

    public decimal? PriceMax { get; set; }

    public virtual ListProduct? ListProduct { get; set; }

    public virtual ICollection<LiveStreamCustomer> LiveStreamCustomers { get; set; } = new List<LiveStreamCustomer>();

    public virtual User User { get; set; } = null!;
}
