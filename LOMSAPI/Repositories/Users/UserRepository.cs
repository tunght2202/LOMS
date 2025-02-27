using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
namespace LOMSAPI.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager
            , IConfiguration config, IDistributedCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        public async Task<string> Authencate(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null) return null;
            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> RegisterRequestAsync(RegisterRequestModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = false
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            var otpCode = new Random().Next(100000, 999999).ToString();
            var cacheKey = "user_info";

            await _cache.SetStringAsync($"OTP_{user.Email}", otpCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            await _cache.SetStringAsync("REGISTER_EMAIL", model.Email, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            await SendEmailAsync(user.Email, "Xác thực tài khoản", $"Mã OTP của bạn là: {otpCode}");

            return true;
        }

        public async Task<bool> RegisterAccountAsync(VerifyOtpModel model)
        {
            var userEmail = await _cache.GetStringAsync("REGISTER_EMAIL");
            var otpCode = await _cache.GetStringAsync($"OTP_{userEmail}");
            if (otpCode == null || otpCode != model.OtpCode) return false;

            var userInfoString = await _cache.GetStringAsync("user_info");
            if (string.IsNullOrEmpty(userInfoString)) return false;

            var userInfo = JsonConvert.DeserializeObject<User>(userInfoString);
            var newUser = new User
            {
                UserName = userInfo.UserName,
                Email = userEmail,
                PhoneNumber = userInfo.PhoneNumber,
                EmailConfirmed = true,
                PasswordHash = userInfo.PasswordHash
            };

            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded) return false;

            await _cache.RemoveAsync("user_info");
            await _cache.RemoveAsync($"OTP_{userEmail}");
            await _cache.RemoveAsync("REGISTER_EMAIL");

            return true;
        }

        public async Task<bool> RequestPasswordResetAsync(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return false;

            var otpCode = new Random().Next(100000, 999999).ToString();
            await _cache.SetStringAsync($"OTP_RESET_{user.Email}", otpCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            await _cache.SetStringAsync("RESET_EMAIL", user.Email, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            await SendEmailAsync(user.Email, "Mã OTP đặt lại mật khẩu", $"Mã OTP: {otpCode}");

            return true;
        }
        public async Task<bool> VerifyOtpAsync(VerifyOtpModel model)
        {
            var userEmail = await _cache.GetStringAsync("RESET_EMAIL");
            var cachedOtp = await _cache.GetStringAsync($"OTP_RESET_{userEmail}");
            if (cachedOtp == null || cachedOtp != model.OtpCode) return false;

            
            await _cache.SetStringAsync($"Verified_{userEmail}", "true", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            var userEmail = await _cache.GetStringAsync("RESET_EMAIL");
            var isVerified = await _cache.GetStringAsync($"Verified_{userEmail}");
            if (string.IsNullOrEmpty(isVerified) || isVerified != "true") return false;
            var user = await _userManager.FindByEmailAsync($"{userEmail}");
            if (user == null) return false;

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded) return false;

            // Xóa trạng thái OTP sau khi đặt lại mật khẩu thành công
            await _cache.RemoveAsync("RESET_EMAIL");
            await _cache.RemoveAsync($"Verified_{userEmail}");
            await _cache.RemoveAsync($"OTP_RESET_{userEmail}");

            return true;
        }

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