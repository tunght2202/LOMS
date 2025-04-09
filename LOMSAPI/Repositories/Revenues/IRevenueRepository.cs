namespace LOMSAPI.Repositories.Revenues
{
    public interface IRevenueRepository
    {
        Task<decimal> GetTotalRevenue(); // Tổng doanh thu
        Task<int> GetTotalOrders(); // Tổng số lượng đơn hàng
        Task<decimal> GetRevenueByLivestreamId(string livestreamId); // Doanh thu của một phiên livestream
        Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate);
    }
}
