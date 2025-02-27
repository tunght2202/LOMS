using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class ResetPasswordModel
    {

        [Required, MinLength(6)]
        public string NewPassword { get; set; }

    }
}
