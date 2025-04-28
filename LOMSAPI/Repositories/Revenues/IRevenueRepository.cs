namespace LOMSAPI.Repositories.Revenues
{
    public interface IRevenueRepository
    {
        Task<decimal> GetRevenueByDateRange(string userid, DateTime startDate, DateTime endDate); // // Doanh thu theo khoảng thời gian
        Task<int> GetTotalOrdersByDateRange(string userid, DateTime startDate, DateTime endDate); // Tổng số lượng đơn hàng theo khoảng thời gian
        Task<int> GetTotalOrederCancelledByDateRange(string userid, DateTime startDate, DateTime endDate); // Tổng số lượng đơn hàng đã hủy theo khoảng thời gian
        Task<int> GetTotalOrederReturnedByDateRange(string userid, DateTime startDate, DateTime endDate); //// Tổng số lượng đơn hàng đã trả lại theo khoảng thời gian
        Task<int> GetTotalOrederDelivered(string userid, DateTime startDate, DateTime endDate); //// Tổng số lượng đơn hàng đã giao theo khoảng thời gian
        Task<decimal> GetRevenueByLivestreamId(string userid, string livestreamId); // Doanh thu của một phiên livestream
        Task<int> GetTotalOrderByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng theo Livestream ID
        Task<int> GetTotalOrederCancelledByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng đã hủy theo Livestream ID
        Task<int> GetTotalOrederReturnedByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng đã trả lại theo Livestream ID
        Task<int> GetTotalOrederDeliveredByLivestreamId(string userid, string livestreamId); // Tổng số lượng đơn hàng đã giao theo Livestream ID



    }
}