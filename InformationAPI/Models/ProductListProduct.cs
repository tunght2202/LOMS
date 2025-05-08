using System;
using System.Collections.Generic;

namespace InformationAPI.Models;

public partial class ProductListProduct
{
    public int ProductListProductId { get; set; }

    public int ProductId { get; set; }

    public int ListProductId { get; set; }

    public virtual ListProduct ListProduct { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
