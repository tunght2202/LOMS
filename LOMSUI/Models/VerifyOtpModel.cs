using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class VerifyOtpModel
    {
        public string Email { get; set; }

        [JsonProperty("OtpCode")]  // Đổi tên thành OtpCode để khớp với API
        public string Otp { get; set; }
    }
}
