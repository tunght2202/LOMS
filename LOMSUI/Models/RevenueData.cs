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


    public class RevenueLivestream
    {
        public decimal LiveStreamRevenue { get; set; }
    }

    public class OrderLive
    {
        public int totalOrders { get; set; }
    }

    public class CancelledOrderLive
    {
        public int totalOrdersCancelled { get; set; }
    }

    public class ReturnedOrderLive
    {
        public int totalOrdersReturned { get; set; }
    }

    public class DeliveredOrderLive
    {
        public int totalOrdersDelivered { get; set; }
    }





}
