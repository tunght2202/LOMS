using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Users
{
    public interface IUserRepository
    {
        Task<string> Authencate(LoginRequest loginRequest);
        Task<bool> RegisterRequestAsync(RegisterRequestModel model);
        Task<bool> RegisterAccountAsync(VerifyOtpModel model);
        Task<bool> RequestPasswordResetAsync(ForgotPasswordModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
        Task<bool> VerifyOtpAsync(VerifyOtpModel model);
    }
}