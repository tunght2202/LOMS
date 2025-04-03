namespace LOMSAPI.Repositories.Revenues
{
    public interface IRevenueRepository
    {
        Task<decimal> GetTotalRevenue(); // Tổng doanh thu
        Task<int> GetTotalOrders(); // Tổng số lượng đơn hàng
        Task<decimal> GetRevenueByLivestreamId(int livestreamId); // Doanh thu của một phiên livestream
    }
}
