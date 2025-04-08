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
        public DateTime StartTime { get; set; }
        public string StreamURL { get; set; }
        public string GetFormattedTime()
        {
            TimeSpan timeDiff = DateTime.UtcNow - StartTime.ToUniversalTime();
            if (timeDiff.TotalMinutes < 1) return "Vừa xong";
            if (timeDiff.TotalMinutes < 60) return $"{(int)timeDiff.TotalMinutes} phút trước";
            if (timeDiff.TotalHours < 24) return $"{(int)timeDiff.TotalHours} giờ trước";
            return StartTime.ToString("dd/MM/yyyy HH:mm");
        }
    }
     
    }
