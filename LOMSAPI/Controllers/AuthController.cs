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
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(UserManager<User> userManager, IDistributedCache cache, IConfiguration config
            ,IUserRepository userRepository)
        {
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

        [HttpPost("register-account-request")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userRepository.RegisterAsync(model);
            if (!result) return BadRequest("Lỗi trong quá trình đăng ký.");

            return Ok(new { message = "Vui lòng kiểm tra email để nhập mã xác thực." });
        }

        [HttpPost("register-account")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyOtpModel model)
        {
            var result = await _userRepository.VerifyEmailAsync(model);
            if (!result) return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            return Ok(new { message = "Register successfully!" });
        }

        [HttpPost("reset-password-request")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ForgotPasswordModel model)
        {
            var result = await _userRepository.RequestPasswordResetAsync(model);
            if (!result) return NotFound("Không tìm thấy người dùng.");

            return Ok(new { message = "Mã OTP đã được gửi qua email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _userRepository.ResetPasswordAsync(model);
            if (!result) return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn.");

            return Ok(new { message = "Mật khẩu đã được đặt lại thành công." });
        }
    }
}
