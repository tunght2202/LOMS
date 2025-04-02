using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderByProductCodeAuto(string productCode);
        Task<int> CreateOrderDetail(OrderDetailAddModel orderModel);
        Task<GetOrderDetailByOrderModel> GetOrderByLivestreamCustomerId(int livestreamCustomerId);

    }
}
