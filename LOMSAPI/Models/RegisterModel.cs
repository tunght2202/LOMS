using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    [Required]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.")]
    public string PhoneNumber { get; set; }

    



}
