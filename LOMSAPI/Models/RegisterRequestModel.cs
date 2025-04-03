using System.ComponentModel.DataAnnotations;

public class RegisterRequestModel
{
    [Required]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.")]
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    
    public string Gender { get; set; }

    
    


}
