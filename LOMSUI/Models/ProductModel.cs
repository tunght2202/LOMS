using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        public string? ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
<<<<<<< HEAD
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Status { get; set; }
        public string? ImageURL { get; set; }
=======
        public string ProductCode { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

>>>>>>> 7dcccd97e68a72de4489f90f8e8b12ae1625b9d2
    }
}