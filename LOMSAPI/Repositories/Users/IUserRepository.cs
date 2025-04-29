using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Users
{
    public interface IUserRepository
    {
        Task<string> Authencate(LoginRequest loginRequest);
        Task<(bool success, string message)> RegisterRequestAsync(RegisterRequestModel model, IFormFile? image);
        Task<bool> RegisterAccountAsync(VerifyOtpModel model);
        Task<bool> RequestPasswordResetAsync(ForgotPasswordModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
        Task<bool> VerifyOtpAsync(VerifyOtpModel model);
        Task<User> GetUserProfile(string UserId);
        Task<bool> UpdateUserProfileRequest(User user, UpdateUserProfileModel model);
        Task<bool> UpdateUserProfile(VerifyOtpModel model, User user);
        Task<bool> UpdateTokenFacbook(string token,string userid);
        Task<User> GetUserById(string userId);
        Task<bool> UpdatePageId(string pageId, string userId);  
    }
}