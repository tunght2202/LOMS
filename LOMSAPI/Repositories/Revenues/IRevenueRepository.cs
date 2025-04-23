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
        Task<int> GetTotalOrderByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng theo Livestream ID
        Task<int> GetTotalOrederCancelledByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng đã hủy theo Livestream ID
        Task<int> GetTotalOrederReturnedByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng đã trả lại theo Livestream ID
        Task<int> GetTotalOrederDeliveredByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng đã giao theo Livestream ID


    }
}
