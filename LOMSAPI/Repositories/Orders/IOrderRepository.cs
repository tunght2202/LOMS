using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderFromComments(int listProductID, string liveStreamId);
        Task<IEnumerable<OrderModel>> GetAllOrdersAsync();
        Task<IEnumerable<OrderModel>> GetAllOrdersByUserIdAsync(string userID);
        Task<IEnumerable<OrderModel>> GetAllOrdersByLiveStreamIdAsync(string liveStreamId);
        Task<IEnumerable<OrderModel>> GetOrdersByCustomerIdAsync(string customerId);
        Task<IEnumerable<OrderModel>> GetOrdersByLiveStreamCustomerIdAsync(int liveStreamCustomerID);
        Task<OrderModel?> GetOrderByIdAsync(int orderId);
        Task<bool> OrderExistsAsync(int orderId);
        Task<int> AddOrderAsync(OrderModel order);
        Task<int> UpdateOrderAsync(OrderModel order);
        Task<int> UpdateStatusOrderAsync(int orderID, int newStatus);
    }
}
