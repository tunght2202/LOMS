using LOMSAPI.Models;
using System.Numerics;

namespace LOMSAPI.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<int> AddCustomer(string customerId, string customerFacebookName);
        Task<int> UpdateCustomer(string customerId, UpdateCustomerModel customerUpdate);
        Task<GetCustomerModel> GetCustomerById(string customerId);
    }
}
