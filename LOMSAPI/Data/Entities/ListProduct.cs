using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOMSAPI.Data.Entities
{
    [Table("ListProducts")]
    public class ListProduct
    {
        [Key]
        public int ListProductId { get; set; }
        public string UserID { get; set; }
        public string ListProductName { get; set; }
        public ICollection<LiveStream> LiveStreams { get; set; }
        public ICollection<ProductListProduct> ProductListProducts { get; set; }
        public virtual User User { get; set; }

    }
}
