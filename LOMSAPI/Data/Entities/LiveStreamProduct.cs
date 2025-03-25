using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Data.Entities
{
    [Table("LiveStreamProducts")]
    public class LiveStreamProduct
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("LiveStream")]
        public string LivestreamID { get; set; }
        public LiveStream LiveStream { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int? DisplayOrder { get; set; } // Thứ tự hiển thị sản phẩm trong livestream
    }
}
