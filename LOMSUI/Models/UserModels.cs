using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class UserModels
    {
        public string Id { get; set; }

        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.")]
        public string? PhoneNumber { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? ImageUrl { get; set; }
    }
}


