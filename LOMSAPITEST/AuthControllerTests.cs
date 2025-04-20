using System.Security.Claims;
using LOMSAPI.Controllers;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LOMSAPITEST
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _controller = new AuthController(_userManagerMock.Object, null, null, _userRepositoryMock.Object);

            // Mock user claims for authenticated endpoints
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "test-user-id") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.Authencate(loginRequest)).ReturnsAsync("valid-token");

            // Act
            var result = await _controller.Authenticate(loginRequest);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = JObject.FromObject(okResult.Value);

            Assert.Equal("valid-token", message["token"]?.ToString());
        }

        [Fact]
        public async Task Authenticate_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "wrongemail", Password = "wrongpass" };
            _userRepositoryMock.Setup(repo => repo.Authencate(loginRequest)).ReturnsAsync((string)null);

            // Act
            var result = await _controller.Authenticate(loginRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or password in correct!", badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterRequest_ValidModel_ReturnsOk()
        {
            // Arrange
            var registerRequest = new RegisterRequestModel();
            var avatarFile = new Mock<IFormFile>().Object;
            _userRepositoryMock.Setup(repo => repo.RegisterRequestAsync(registerRequest, avatarFile)).ReturnsAsync(true);

            // Act
            var result = await _controller.RegisterRequest(registerRequest, avatarFile);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);

            Assert.Equal("Please check email for otp code!", dict["message"]);

        }

        [Fact]
        public async Task RegisterRequest_FailedProcess_ReturnsBadRequest()
        {
            // Arrange
            var registerRequest = new RegisterRequestModel();
            var avatarFile = new Mock<IFormFile>().Object;
            _userRepositoryMock.Setup(repo => repo.RegisterRequestAsync(registerRequest, avatarFile)).ReturnsAsync(false);

            // Act
            var result = await _controller.RegisterRequest(registerRequest, avatarFile);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error in the register process.", badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterRequest_InvalidEmail_ReturnsBadRequest()
        {
            var registerRequest = new RegisterRequestModel
            {
                UserName = "testuser",
                Email = "invalidemail", // invalid
                Password = "123456",
                PhoneNumber = "0123456789"
            };

            _controller.ModelState.AddModelError("Email", "Invalid email");

            var avatarFile = new Mock<IFormFile>().Object;
            var result = await _controller.RegisterRequest(registerRequest, avatarFile);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RegisterRequest_MissingRequiredFields_ReturnsBadRequest()
        {
            var registerRequest = new RegisterRequestModel(); // all required fields missing
            _controller.ModelState.AddModelError("UserName", "Required");

            var avatarFile = new Mock<IFormFile>().Object;
            var result = await _controller.RegisterRequest(registerRequest, avatarFile);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RegisterRequest_InvalidPhoneNumber_ReturnsBadRequest()
        {
            var registerRequest = new RegisterRequestModel
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "123456",
                PhoneNumber = "123456789", // invalid, doesn't start with 0
            };

            _controller.ModelState.AddModelError("PhoneNumber", "Invalid format");

            var avatarFile = new Mock<IFormFile>().Object;
            var result = await _controller.RegisterRequest(registerRequest, avatarFile);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RegisterAccount_ValidOtp_ReturnsOk()
        {
            // Arrange
            var verifyOtpModel = new VerifyOtpModel();
            _userRepositoryMock.Setup(repo => repo.RegisterAccountAsync(verifyOtpModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.RegisterAccount(verifyOtpModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            Assert.Equal("Register successfully!", dict["message"]);

        }

        [Fact]
        public async Task RegisterAccount_InvalidOtp_ReturnsBadRequest()
        {
            // Arrange
            var verifyOtpModel = new VerifyOtpModel();
            _userRepositoryMock.Setup(repo => repo.RegisterAccountAsync(verifyOtpModel)).ReturnsAsync(false);

            // Act
            var result = await _controller.RegisterAccount(verifyOtpModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("OTP code is invalid or expired.", badRequestResult.Value);
        }

        [Fact]
        public async Task ResetPasswordRequest_UserExists_ReturnsOk()
        {
            // Arrange
            var forgotPasswordModel = new ForgotPasswordModel();
            _userRepositoryMock.Setup(repo => repo.RequestPasswordResetAsync(forgotPasswordModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.ResetPasswordRequest(forgotPasswordModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            Assert.Equal("OTP code has been sent via email.", dict["message"]);
        }

        [Fact]
        public async Task ResetPasswordRequest_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var forgotPasswordModel = new ForgotPasswordModel();
            _userRepositoryMock.Setup(repo => repo.RequestPasswordResetAsync(forgotPasswordModel)).ReturnsAsync(false);

            // Act
            var result = await _controller.ResetPasswordRequest(forgotPasswordModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Can't find the user.", notFoundResult.Value);
        }

        [Fact]
        public async Task VerifyOtp_ValidOtp_ReturnsOk()
        {
            // Arrange
            var verifyOtpModel = new VerifyOtpModel();
            _userRepositoryMock.Setup(repo => repo.VerifyOtpAsync(verifyOtpModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.VerifyOtp(verifyOtpModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            Assert.Equal("OTP valid. You can reset your password.", dict["message"]);

        }

        [Fact]
        public async Task VerifyOtp_InvalidOtp_ReturnsBadRequest()
        {
            // Arrange
            var verifyOtpModel = new VerifyOtpModel();
            _userRepositoryMock.Setup(repo => repo.VerifyOtpAsync(verifyOtpModel)).ReturnsAsync(false);

            // Act
            var result = await _controller.VerifyOtp(verifyOtpModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("OTP code is invalid or expired.", badRequestResult.Value);
        }

        [Fact]
        public async Task ResetPassword_ValidOtp_ReturnsOk()
        {
            // Arrange
            var resetPasswordModel = new ResetPasswordModel();
            _userRepositoryMock.Setup(repo => repo.ResetPasswordAsync(resetPasswordModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.ResetPassword(resetPasswordModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            Assert.Equal("Password was reset successfully.", dict["message"]);
        }

        [Fact]
        public async Task ResetPassword_InvalidOtp_ReturnsBadRequest()
        {
            // Arrange
            var resetPasswordModel = new ResetPasswordModel();
            _userRepositoryMock.Setup(repo => repo.ResetPasswordAsync(resetPasswordModel)).ReturnsAsync(false);

            // Act
            var result = await _controller.ResetPassword(resetPasswordModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("OTP code is invalid or expired.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetUserProfile_UserExists_ReturnsOk()
        {
            // Arrange
            var user = new User { Id = "test-user-id", UserName = "testuser" };
            _userRepositoryMock.Setup(repo => repo.GetUserProfile("test-user-id")).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<User>(okResult.Value);
            Assert.Equal("testuser", returnValue.UserName);
        }

        [Fact]
        public async Task GetUserProfile_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetUserProfile("test-user-id")).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUserProfile();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateProfileRequest_UserExistsWithEmailChange_ReturnsOkWithVerificationMessage()
        {
            // Arrange
            var updateModel = new UpdateUserProfileModel { Email = "newemail@example.com" };
            var user = new User { Id = "test-user-id" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-user-id")).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUserProfileRequest(user, updateModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProfileRequest(updateModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            Assert.Equal("Please enter email verification code.", dict["message"]);
        }

        [Fact]
        public async Task UpdateProfileRequest_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var updateModel = new UpdateUserProfileModel();
            _userManagerMock.Setup(m => m.FindByIdAsync("test-user-id")).ReturnsAsync((User)null);

            // Act
            var result = await _controller.UpdateProfileRequest(updateModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't find the user", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProfile_ValidOtp_ReturnsOk()
        {
            // Arrange
            var otpModel = new VerifyOtpModel();
            var user = new User { Id = "test-user-id" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-user-id")).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUserProfile(otpModel, user)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProfile(otpModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);
            Assert.Equal("Information edited successfully.", dict["message"]);
        }

        [Fact]
        public async Task UpdateProfile_InvalidOtp_ReturnsBadRequest()
        {
            // Arrange
            var otpModel = new VerifyOtpModel();
            var user = new User { Id = "test-user-id" };
            _userManagerMock.Setup(m => m.FindByIdAsync("test-user-id")).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUserProfile(otpModel, user)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateProfile(otpModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("OTP code is invalid or expired.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateTokenFacebook_ValidToken_ReturnsOk()
        {
            // Arrange
            var token = "facebook-token";
            _userRepositoryMock.Setup(repo => repo.UpdateTokenFacbook(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            // Debug: Kiểm tra userId
            var userId = _controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"userId: {userId}");

            // Act
            var result = await _controller.UpdateTokenFacebook(token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(serialized);

            Assert.Equal("Facebook token updated successfully!", dict["message"]);
        }

        [Fact]
        public async Task UpdateTokenFacebook_UpdateFailed_ReturnsBadRequest()
        {
            // Arrange
            var token = "facebook-token";
            _userRepositoryMock.Setup(repo => repo.UpdateTokenFacbook(token, "test-user-id")).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateTokenFacebook(token);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var json = JsonConvert.SerializeObject(badRequestResult.Value);
            var returnValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            Assert.Equal("Error updating Facebook token", returnValue["message"]);
        }
    }
}