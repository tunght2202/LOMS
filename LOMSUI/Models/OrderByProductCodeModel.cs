using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class OrderByProductCodeModel
    {
        public string Status { get; set; } // Pending, Confirmed, Shipped, Delivered, Canceled, Returned
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "CurrentPrice must be a non-negative value.")]
        public decimal? TotalPrice { get; set; }
        public bool StatusCheck { get; set; } = false;
        [StringLength(50, ErrorMessage = "TrackingNumber cannot exceed 50 characters.")]
        public string? TrackingNumber { get; set; }
        public string? Note { get; set; }
        public int ProductID { get; set; }
        public int LiveStreamCustomerID { get; set; }
    }
}
