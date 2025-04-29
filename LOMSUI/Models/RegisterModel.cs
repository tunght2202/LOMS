using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }

        public byte[] AvatarData { get; set; }

        public string AvatarFileName { get; set; }

        public string AvatarContentType { get; set; }
    }
    public class RegisterResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }

    }


}

