using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class PutProductModel
    {
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "ProductCode can only contain letters and numbers.")]
        public string? ProductCode { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 1000000000000, ErrorMessage = "Price must be greater or equal 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, 1000000, ErrorMessage = "Stock must be between 0 and 1,000,000.")]
        public int Stock { get; set; }
    }
}
