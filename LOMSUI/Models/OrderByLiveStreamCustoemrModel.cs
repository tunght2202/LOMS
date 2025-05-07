using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class OrderByLiveStreamCustoemrModel
    {
        public string LiveStreamTital { get; set; }
        public string CustoemrName { get; set; }
        public int TotalOrder { get; set; }
        public long TotalPrice { get; set; }
        public int LiveStreamCustoemrID { get; set; }
        public List<OrderByProductCodeModel> orderByProductCodeModels { get; set; }
    }
}
