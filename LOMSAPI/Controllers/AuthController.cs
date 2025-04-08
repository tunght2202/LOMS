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
using System.Security.Claims;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        public AuthController(UserManager<User> userManager, IDistributedCache cache, IConfiguration config
            ,IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;

        }
        // Thanh Tùng
        // Login 
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

        [HttpPost("register-account-request")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterRequest([FromForm] RegisterRequestModel model, IFormFile Avatar)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userRepository.RegisterRequestAsync(model, Avatar);
            if (!result) return BadRequest("Lỗi trong quá trình đăng ký.");

            return Ok(new { message = "Vui lòng kiểm tra email để nhập mã xác thực." });
        }
        
        
        [HttpPost("register-account")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAccount([FromBody] VerifyOtpModel model)
        {
            var result = await _userRepository.RegisterAccountAsync(model);
            if (!result) return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            return Ok(new { message = "Đăng kí thành công!" });
        }

        [HttpPost("reset-password-request")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ForgotPasswordModel model)
        {
            var result = await _userRepository.RequestPasswordResetAsync(model);
            if (!result) return NotFound("Không tìm thấy người dùng.");

            return Ok(new { message = "Mã OTP đã được gửi qua email." });
        }
        [HttpPost("reset-password-verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpModel model)
        {
            var result = await _userRepository.VerifyOtpAsync(model);
            if (!result) return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            return Ok(new { message = "OTP hợp lệ. Bạn có thể đặt lại mật khẩu." });
        }
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _userRepository.ResetPasswordAsync(model);
            if (!result) return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            return Ok(new { message = "Mật khẩu đã được đặt lại thành công." });
        }
        [HttpGet("user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            User user =  await _userRepository.GetUserProfile(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }
        [HttpPut("update-userProfile-request")]
        public async Task<IActionResult> UpdateProfileRequest([FromForm] UpdateUserProfileModel model)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Không tìm thấy user");
            var result = await _userRepository.UpdateUserProfileRequest(user, model);
            if (!result) return BadRequest("Lỗi trong quá trình sửa thông tin");
            if (model.Email != null)
            {
                return Ok(new { message = "Vui lòng nhập mã xác thực email." });
            }
            return Ok(new { message = "Thông tin đã sửa thành công." });
        }
        [HttpPut("update-userProfie")]
        public async Task<IActionResult> UpdateProfile(VerifyOtpModel OTP)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await _userManager.FindByIdAsync(userId);
            var result = await _userRepository.UpdateUserProfile(OTP,user);
            if (!result) return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            return Ok(new { message = "Thông tin đã sửa thành công" });
        }
    }
}
