using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class LiveVideo
    {
        [JsonProperty("livestreamID")]
        public string LivestreamID { get; set; }

        [JsonProperty("streamTitle")]
        public string StreamTitle { get; set; }

        [JsonProperty("streamURL")]
        public string StreamURL { get; set; }

        [JsonProperty("startTime")]
        public DateTime? StartTime { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        public string FormattedStartTime
        {
            get
            {
                if (StartTime.HasValue)
                {
                    return StartTime.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }
                return "Invalid Date";
            }
        }
    }
}
