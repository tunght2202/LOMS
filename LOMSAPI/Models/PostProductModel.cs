namespace LOMSAPI.Models
{
    public class PostProductModel
    {
        public string? ProductCode { get; set; }
        public string Name { get; set; }
        public string UserID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public string? ImageURL { get; set; }
    }
}
