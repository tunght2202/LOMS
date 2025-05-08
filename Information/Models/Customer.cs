namespace Information.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string FacebookName { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int SuccessfulDeliveries { get; set; }

    public int FailedDeliveries { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool Status { get; set; }
    public string? ProvinceId { get; set; }
    public string? DistrictId { get; set; }
    public string? WardId { get; set; }
    public string? DetailAddress { get; set; }
    public virtual ICollection<LiveStreamCustomer> LiveStreamCustomers { get; set; } = new List<LiveStreamCustomer>();
}
