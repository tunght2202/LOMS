using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOMSAPI.Data.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
        public string? ProductCode { get; set; }
        public string? Description { get; set; }
        public string LiveStreamID { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; } = true;

        public LiveStream LiveStream { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

}