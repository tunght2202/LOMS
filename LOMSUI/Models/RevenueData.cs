namespace LOMSAPI.Models
{
    public class RevenueData
    {
        public decimal TotalRevenue { get; set; }
    }
    public class OrderResponse
    {
        public int TotalOrders { get; set; }
    }
    public class CancelledOrderResponse
    {
        public int TotalOrdersCancelled { get; set; }
    }

    public class ReturnedOrderResponse
    {
        public int TotalOrdersReturned { get; set; }
    }

    public class DeliveredOrderResponse 
    {
        public int TotalOrdersDelivered { get; set; }
    }

    public class LivestreamRevenueResponse
    {
        public decimal LiveStreamRevenue { get; set; }
    }



}
