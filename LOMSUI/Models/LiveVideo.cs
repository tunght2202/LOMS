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
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("permalink_url")]
        public string PermalinkUrl { get; set; }

        [JsonProperty("creation_time")]
        public string CreationTimeRaw { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public string FormattedCreationTime
        {
            get
            {
                if (DateTime.TryParse(CreationTimeRaw, out DateTime dateTime))
                {
                    return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                }
                return "Invalid Date";
            }
        }
    }
}
