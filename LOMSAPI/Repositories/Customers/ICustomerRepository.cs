using LOMSAPI.Models;
using System.Numerics;

namespace LOMSAPI.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<int> AddCustomer(string customerId, string customerFacebookName);
        Task<int> AddLivestreamCustomer(string customerId, string livestreamId);
        Task<int> UpdateCustomer(string customerId, UpdateCustomerModel customerUpdate);
        Task<GetCustomerModel> GetCustomerById(string customerId);
        Task<GetCustomerModel> GetCustomerByOrderIdAsync(int orderID);
        Task<IEnumerable<GetCustomerModel>> GetCustomersByUserIdAsync(string userId);
        Task<IEnumerable<GetCustomerModel>> GetCustomersByLiveStreamIdAsync(string liveStreamId);

    }
}
