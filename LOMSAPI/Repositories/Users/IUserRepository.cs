using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Users
{
    public interface IUserRepository
    {
        Task<string> Authencate(LoginRequest loginRequest);
        Task<bool> RegisterAsync(RegisterModel model);
        Task<bool> VerifyEmailAsync(VerifyOtpModel model);
        Task<bool> RequestPasswordResetAsync(ForgotPasswordModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    }
}