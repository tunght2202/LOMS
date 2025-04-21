using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class OrderModelWrapper : Java.Lang.Object
    {
        public OrderModel Order { get; set; }

        public OrderModelWrapper(OrderModel order)
        {
            Order = order;
        }
    }

}
