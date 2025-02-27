using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class VerifyOtpModel
    {

        [Required, MinLength(6), MaxLength(6)]
        public string OtpCode { get; set; }
    }
}
