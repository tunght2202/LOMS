namespace LOMSAPI.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderByProductCodeAuto(string productCode);
    }
}
