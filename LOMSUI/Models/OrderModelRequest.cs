using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class OrderModelRequest
    {
        public int OrderID { get; set; }
        public string TrackingNumber { get; set; }
        public string Note { get; set; }
    }

}
