using LOMSAPI.Data.Entities;

namespace LOMSAPI.Models
{
    public class OrderCustomerModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered, Canceled, Returned
        public int Quantity { get; set; }
        public decimal? CurrentPrice { get; set; }
        public bool StatusCheck { get; set; } = false;
        public string? TrackingNumber { get; set; }
        public string? Note { get; set; }
        public string FacebookName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
