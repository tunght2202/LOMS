using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class ResetPasswordModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6), MaxLength(6)]
        public string OtpCode { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }

    }
}
