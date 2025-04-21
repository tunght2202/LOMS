namespace LOMSAPI.Repositories.Revenues
{
    public interface IRevenueRepository
    {

        Task<decimal> GetTotalRevenue(string userid); // Tổng doanh thu
        Task<int> GetTotalOrders(string userid); // Tổng số lượng đơn hàng
        Task<decimal> GetRevenueByLivestreamId(string userid, string livestreamId); // Doanh thu của một phiên livestream
        Task<decimal> GetRevenueByDateRange(string userid, DateTime startDate, DateTime endDate);
        Task<int> GetTotalOrederCancelled(string userid); // Tổng số lượng đơn hàng đã hủy
        Task<int> GetTotalOrederReturned(string userid); // Tổng số lượng đơn hàng đã trả lại
        Task<int> GetTotalOrederDelivered(string userid); // Tổng số lượng đơn hàng đã giao
    }
}
