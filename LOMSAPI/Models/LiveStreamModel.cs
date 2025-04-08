namespace LOMSAPI.Models
{
    public class LiveStreamModel
    {
        public int LiveStreamID { get; set; }
        public string StreamTitle { get; set; }
        public string StreamURL { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; } //  Live, VOD       
    }
}
