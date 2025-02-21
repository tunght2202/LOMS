using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using LOMSAPI.Repositories.Users;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthController(UserManager<User> userManager, IDistributedCache cache, IConfiguration config
            ,IUserRepository userRepository)
        {
            _userManager = userManager;
            _cache = cache;
            _config = config;
            _userRepository = userRepository;
        }
        [HttpPost("login-account-request")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] Models.LoginRequest loginRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userRepository.Authencate(loginRequest);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Username or password in correct!");
            }
            return Ok(new {token = result});
        }

        // API yêu cầu đăng ký user
        [HttpPost("register-account-request")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = false
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            // Tạo mã OTP
            var otpCode = new Random().Next(100000, 999999).ToString();
            var cacheKey = "user_info";
            await _cache.SetStringAsync($"OTP_{user.Email}", otpCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            // Gửi email OTP
            await SendEmailAsync(user.Email, "Xác thực tài khoản", $"Mã OTP của bạn là: {otpCode}");

            return Ok(new { message = "Vui lòng kiểm tra email để nhập mã xác thực." });
        }


        [HttpPost("register-account")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyOtpModel model)
        {
            var otpCode = await _cache.GetStringAsync($"OTP_{model.Email}");
            if (otpCode == null || otpCode != model.OtpCode)
                return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            //var user = await _userManager.FindByEmailAsync(model.Email);
            //if (user == null) return NotFound("Người dùng không tồn tại.");
            
            var userInfoString = await _cache.GetStringAsync("user_info");
            
            if (string.IsNullOrEmpty(userInfoString))
                return BadRequest("Thông tin người dùng không tìm thấy.");

            var userInfo = JsonConvert.DeserializeObject<dynamic>(userInfoString);
            var NewUser = new User
            {
                UserName = userInfo.UserName,
                Email = model.Email,
                PhoneNumber = userInfo.PhoneNumber,
                EmailConfirmed = true, 
                PasswordHash = userInfo.PasswordHash
            };

            var result = await _userManager.CreateAsync(NewUser);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Xóa thông tin người dùng khỏi cache sau khi tạo tài khoản thành công
            await _cache.RemoveAsync("user_info");
            await _cache.RemoveAsync($"OTP_{model.Email}");

            return Ok(new { message = "Register sucessfully!" });
        }

        // API Yêu cầu đặt lại mật khẩu
        [HttpPost("reset-password-request")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("Không tìm thấy người dùng.");

            var otpCode = new Random().Next(100000, 999999).ToString();
            await _cache.SetStringAsync($"OTP_RESET_{user.Email}", otpCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            await SendEmailAsync(user.Email, "Mã OTP đặt lại mật khẩu", $"Mã OTP: {otpCode}");

            return Ok(new { message = "Mã OTP đã được gửi qua email." });
        }

        //  API Đặt lại mật khẩu bằng OTP
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var otpCode = await _cache.GetStringAsync($"OTP_RESET_{model.Email}");
            if (otpCode == null || otpCode != model.OtpCode)
                return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("Người dùng không tồn tại.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "Mật khẩu đã được đặt lại thành công." });
        }

        // Hàm gửi email
        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_config["EmailSettings:SmtpPort"]),
                Credentials = new NetworkCredential(_config["EmailSettings:SmtpUser"], _config["EmailSettings:SmtpPass"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
