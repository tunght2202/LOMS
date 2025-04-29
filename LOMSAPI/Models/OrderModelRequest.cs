namespace LOMSAPI.Models
{
    public class OrderModelRequest
    {
        public int OrderID { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Note { get; set; }
    }

}
