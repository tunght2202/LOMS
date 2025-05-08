using System;
using System.Collections.Generic;

namespace Information.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public int Status { get; set; }

    public int Quantity { get; set; }

    public int ProductId { get; set; }

    public string? CommentId { get; set; }

    public decimal? CurrentPrice { get; set; }

    public string? Note { get; set; }

    public bool StatusCheck { get; set; }

    public string? TrackingNumber { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual Product Product { get; set; } = null!;
}
