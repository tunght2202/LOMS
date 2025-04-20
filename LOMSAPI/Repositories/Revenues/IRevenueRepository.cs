namespace LOMSAPI.Repositories.Revenues
{
    public interface IRevenueRepository
    {
        Task<decimal> GetTotalRevenue(); // Tổng doanh thu
        Task<int> GetTotalOrders(); // Tổng số lượng đơn hàng
        Task<decimal> GetRevenueByLivestreamId(string livestreamId); // Doanh thu của một phiên livestream
        Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate);
        Task<int> GetTotalOrederCancelled(); // Tổng số lượng đơn hàng đã hủy
        Task<int> GetTotalOrederReturned(); // Tổng số lượng đơn hàng đã trả lại
        Task<int> GetTotalOrederDelivered(); // Tổng số lượng đơn hàng đã giao
    }
}
