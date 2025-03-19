using LOMSAPI.Data.Entities;

namespace LOMSAPI.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<int> CreateOrder();
    }
}
