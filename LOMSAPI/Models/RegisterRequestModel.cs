using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class RegisterRequestModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username only includes characters from A - Z and numbers.")]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }
    [Required,RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must be 10 digits and start with 0.")]
    public string PhoneNumber { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Address { get; set; }
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender can only be 'Male' or 'Female'.")]
    public string? Gender { get; set; }

    
    


}
