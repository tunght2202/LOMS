using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOMSAPI.Data.Entities
{
    [Table("ProductListProducts")]
    public class ProductListProduct
    {
        [Key]
        public int ProductListProductID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int ListProductID { get; set; }
        public ListProduct ListProduct { get; set; }
    }
}
