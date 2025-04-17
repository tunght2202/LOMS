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
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.")]
    public string PhoneNumber { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Address { get; set; }
    [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender chỉ được là 'Male' hoặc 'Female'.")]
    public string? Gender { get; set; }

    
    


}
