using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class HomePageModel
    {
        // Dữ liệu
        public double Revenue { get; set; }
        public int OrderCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ProductModel> ProductList { get; set; }


    }
}