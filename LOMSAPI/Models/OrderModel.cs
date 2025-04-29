using LOMSAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class OrderModel
    {
        [Required(ErrorMessage = "OrderID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be a positive number.")]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered, Canceled, Returned
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "CurrentPrice must be a non-negative value.")]
        public decimal? CurrentPrice { get; set; }
        public bool StatusCheck { get; set; } = false;
        [StringLength(50, ErrorMessage = "TrackingNumber cannot exceed 50 characters.")]
        public string? TrackingNumber { get; set; }
        public string? Note { get; set; }
        public int ProductID { get; set; }
        public string? CommentID { get; set; }
        public ProductModel Product { get; set; }
    }
}
