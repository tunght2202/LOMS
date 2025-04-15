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

        public string OtpCode { get; set; }
    }
}
