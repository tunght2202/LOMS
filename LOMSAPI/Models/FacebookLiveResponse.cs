using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LOMSAPI.Models
{
    public class FacebookLiveResponse
    {
        [JsonProperty("data")]
        public List<FacebookLiveVideo> Data { get; set; }
    }

    public class FacebookLiveVideo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("permalink_url")]
        public string PermalinkUrl { get; set; }

        [JsonProperty("creation_time")]
        public DateTime CreationTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

    }

}
