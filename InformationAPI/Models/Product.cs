using System;
using System.Collections.Generic;

namespace InformationAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? ProductCode { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public string UserId { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductListProduct> ProductListProducts { get; set; } = new List<ProductListProduct>();

    public virtual User User { get; set; } = null!;
}
