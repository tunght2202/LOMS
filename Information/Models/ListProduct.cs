using System;
using System.Collections.Generic;

namespace Information.Models;

public partial class ListProduct
{
    public int ListProductId { get; set; }

    public string UserId { get; set; } = null!;

    public string ListProductName { get; set; } = null!;

    public virtual ICollection<LiveStream> LiveStreams { get; set; } = new List<LiveStream>();

    public virtual ICollection<ProductListProduct> ProductListProducts { get; set; } = new List<ProductListProduct>();

    public virtual User User { get; set; } = null!;
}
