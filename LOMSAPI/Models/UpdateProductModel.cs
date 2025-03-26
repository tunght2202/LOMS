namespace LOMSAPI.Models
{
    public class UpdateProductModel
    {
        public string Name { get; set; }
        public string? ProductCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
