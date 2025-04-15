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
using LOMSAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;
namespace LOMSAPI.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;
        private readonly CloudinaryService _cloudinaryService;
         
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager
            , IConfiguration config, IDistributedCache cache, CloudinaryService cloudinaryService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cloudinaryService = cloudinaryService;
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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

        public async Task<bool> RegisterRequestAsync(RegisterRequestModel model, IFormFile image)
        {
            User userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null) return false;
            string imageUrl = await _cloudinaryService.UploadImageAsync(image);
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = false,
                FullName = model.FullName,
                ImageURL = imageUrl,
                Address = model.Address,
                Sex = model.Gender,
                
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

            await SendEmailAsync(user.Email, "Verify account", $"Your OTP code is: {otpCode}");

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
                PasswordHash = userInfo.PasswordHash,
                FullName = userInfo.FullName,
                ImageURL = userInfo.ImageURL,
                Address = userInfo.Address,
                Sex = userInfo.Sex,
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
            await SendEmailAsync(user.Email, "OTP code reset password", $"OTP code: {otpCode}");

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

        public async Task<User> GetUserProfile(string UserId)
        {
            User user = await _userManager.FindByIdAsync(UserId);
            return user;
        }

        public async Task<bool> UpdateUserProfileRequest(User user, UpdateUserProfileModel model)
        {
            try

            {
                var oldEmail = user.Email;

                var userInfo = new User();
                if (model.UserName != null)
                {
                    
                    user.UserName = model.UserName;
                    userInfo.UserName = model.UserName;
                }

                if (model.Password != null)
                {
                    var passwordHasher = new PasswordHasher<User>();
                    user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
                    userInfo.PasswordHash = passwordHasher.HashPassword(userInfo, model.Password);
                }
                if (model.PhoneNumber != null)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    userInfo.PhoneNumber = model.PhoneNumber;
                }
                if (model.FullName != null)
                {
                    user.FullName = model.FullName;
                    userInfo.FullName = model.FullName;
                }
                if (model.Address != null)
                {
                    user.Address = model.Address;
                    userInfo.Address = model.Address;
                }
                if (model.Gender != null)
                {
                    user.Sex = model.Gender;
                    userInfo.Sex = model.Gender;
                }
                if (model.Avatar != null)
                {
                    user.ImageURL = await _cloudinaryService.UploadImageAsync(model.Avatar);
                    userInfo.ImageURL = await _cloudinaryService.UploadImageAsync(model.Avatar);
                }

                if (model.Email == null)
                {
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    var otpCode = new Random().Next(100000, 999999).ToString();
                    await SendEmailAsync(oldEmail, "OTP EDIT PROFILE", $"Mã OTP: {otpCode}");
                    await _cache.SetStringAsync($"OTP_UPDATE_{oldEmail}", otpCode, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                    await _cache.SetStringAsync("UPDATE_EMAIL", oldEmail, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                    await _cache.SetStringAsync("USER_INFO_UPDATE", JsonConvert.SerializeObject(userInfo), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
            return true;

        }

        public async Task<bool> UpdateUserProfile(VerifyOtpModel model, User user)
        {
            var userEmail = await _cache.GetStringAsync("UPDATE_EMAIL");
            var otpCode = await _cache.GetStringAsync($"OTP_UPDATE_{userEmail}");
            if (otpCode == null || otpCode != model.OtpCode) return false;

            var userInfoString = await _cache.GetStringAsync("USER_INFO_UPDATE");
            if (string.IsNullOrEmpty(userInfoString)) return false;

            var userInfo = JsonConvert.DeserializeObject<User>(userInfoString);
            user.Email = userEmail;
            if (userInfo.UserName != null)
            {
                user.UserName = userInfo.UserName;
            }

            if (userInfo.PasswordHash != null)
            {
                user.PasswordHash = userInfo.PasswordHash;
            }
            if (userInfo.PhoneNumber != null)
            {
                user.PhoneNumber = userInfo.PhoneNumber;
            }
            if (userInfo.FullName != null)
            {
                user.FullName = userInfo.FullName;
            }
            if (userInfo.Address != null)
            {
                user.Address = userInfo.Address;
            }
            if (userInfo.Sex != null)
            {
                user.Sex = userInfo.Sex;
            }
            if (userInfo.ImageURL != null)
            {
                user.ImageURL = userInfo.ImageURL;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return false;

            await _cache.RemoveAsync("USER_INFO_UPDATE");
            await _cache.RemoveAsync($"OTP_UPDATE_{userEmail}");
            await _cache.RemoveAsync("UPDATE_EMAIL");

            return true;
        }
    }
}