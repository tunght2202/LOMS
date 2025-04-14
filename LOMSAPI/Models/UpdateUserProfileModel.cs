using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class UpdateUserProfileModel
    {
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username only includes characters from A - Z and numbers.")]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must be 10 digits and start with 0.")]
        public string? PhoneNumber { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender can only be 'Male' or 'Female'.")]
        public string? Gender { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
