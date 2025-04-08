using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderByProductCodeAuto(string productCode);
        Task<int> CreateOrderDetail(OrderDetailAddModel orderModel);
        Task<GetOrderDetailByOrderModel> GetOrderByLivestreamCustomerId(int livestreamCustomerId);
        Task<IEnumerable<OrderModel>> GetOrderByLivestreamId(string livestreamId);
        Task<IEnumerable<OrderModel>> GetOrderByUserId(string userId);
        Task<int> UpdateOrderDetail(int orderDetailId, int quantity);
        Task<int> DeleteOrderDetail(int orderDetailId);
        Task<IEnumerable<OrderModel>> GetOrdersByStatusByLivestreamId(string livestreamId, OrderStatus status);
        Task<IEnumerable<OrderModel>> GetOrdersByStatusByUserId(string userId, OrderStatus status);
        Task<OrderModel> GetOrdersById(int orderId);
        Task<IEnumerable<OrderModel>> GetOrders();
        Task<bool> UpdateOrderStatus(int orderId, OrderStatus newStatus);
    }
}
