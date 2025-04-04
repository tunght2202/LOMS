using LOMSAPI.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOMSAPI.Models
{
    public class UpdateOrderDetailModel
    {
        public int OrderDetailID { get; set; }
        public int Quantity { get; set; }
    }
}
