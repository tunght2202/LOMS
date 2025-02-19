using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Users
{
    public interface IUserRepository
    {
        Task<string> Authencate(LoginRequest loginRequest);
    }
}
