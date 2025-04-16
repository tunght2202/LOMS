using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderFromComments(string liveStreamId, string userId);
        Task<IEnumerable<OrderModel>> GetAllOrdersAsync();
        Task<IEnumerable<OrderModel>> GetAllOrdersByUserIdAsync(string userID);
        Task<IEnumerable<OrderModel>> GetAllOrdersByLiveStreamIdAsync(string liveStreamId);
        Task<IEnumerable<OrderModel>> GetOrdersByCustomerIdAsync(string customerId);
        Task<IEnumerable<OrderModel>> GetOrdersByLiveStreamCustomerIdAsync(int liveStreamCustomerID);
        Task<OrderModel?> GetOrderByIdAsync(int orderId);
        Task<bool> OrderExistsAsync(int orderId);
        Task<bool> AddOrderAsync(string commentId);
        Task<int> UpdateOrderAsync(OrderModel order);
        Task<int> UpdateStatusOrderAsync(int orderID, OrderStatus newStatus);
    }
}
