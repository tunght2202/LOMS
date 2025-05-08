using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class OrderByLiveStreamCustoemrModel
    {
        public int OrderID { get; set; }
        public string LiveStreamTital { get; set; }
        public decimal? PriceMax { get; set; }
        public string CustoemrName { get; set; }
        public int TotalOrder { get; set; }
        public long TotalPrice { get; set; }
        public int LiveStreamCustoemrID { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Note { get; set; }
        public string FacebookName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string OrderStatus { get; set; }
        public string ImageUrl { get; set; }
        public List<OrderByProductCodeModel> orderByProductCodeModels { get; set; }
    }
}
