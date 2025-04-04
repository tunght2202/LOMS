using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class LiveStreamModel
    {
        public string LivestreamID { get; set; }
        public string StreamTitle { get; set; }
        public string Status { get; set; }
        public string StartTime { get; set; }
        public string StreamURL { get; set; }
    }
}
