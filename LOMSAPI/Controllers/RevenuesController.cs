using System.Security.Claims;
using LOMSAPI.Repositories.Revenues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RevenuesController : ControllerBase
    {
        private readonly IRevenueRepository _revenueRepository;

        public RevenuesController(IRevenueRepository revenueRepository)
        {
            _revenueRepository = revenueRepository;
        }

        /// <summary>
        /// API tính tổng doanh thu từ tất cả đơn hàng đã giao.
        /// </summary>
        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var revenue = await _revenueRepository.GetTotalRevenue(userId);
            return Ok(new { TotalRevenue = revenue });
        }

        /// <summary>
        /// API tính tổng số lượng đơn hàng.
        /// </summary>
        [HttpGet("total-orders")]
        public async Task<IActionResult> GetTotalOrders()
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _revenueRepository.GetTotalOrders(userId);
            return Ok(new { TotalOrders = orders });
        }

        /// <summary>
        /// API tính doanh thu của một phiên livestream theo Livestream ID.
        /// </summary>
        [HttpGet("livestream-revenue/{livestreamId}")]
        public async Task<IActionResult> GetRevenueByLivestreamId(string livestreamId)
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var revenue = await _revenueRepository.GetRevenueByLivestreamId(userId, livestreamId);
            return Ok(new { LiveStreamRevenue = revenue });
        }
        /// <summary>
        /// API tính doanh thu theo khoảng thời gian.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("revenue-by-date")]
        public async Task<IActionResult> GetRevenueByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var revenue = await _revenueRepository.GetRevenueByDateRange(userId, startDate, endDate);
            return Ok(new { StartDate = startDate, EndDate = endDate, TotalRevenue = revenue });
        }
        [HttpGet("total-orders-cancelled")]
        public async Task<IActionResult> GetTotalOrdersCancelled()
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var totalCancelled = await _revenueRepository.GetTotalOrederCancelled(userId);
            return Ok(new { TotalOrdersCancelled = totalCancelled });
        }
        [HttpGet("total-orders-returned")]
        public async Task<IActionResult> GetTotalOrdersReturned()
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var totalReturned = await _revenueRepository.GetTotalOrederReturned(userId);
            return Ok(new { TotalOrdersReturned = totalReturned });
        }
        [HttpGet("total-orders-delivered")]
        public async Task<IActionResult> GetTotalOrdersDelivered()
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var totalDelivered = await _revenueRepository.GetTotalOrederDelivered(userId);
            return Ok(new { TotalOrdersDelivered = totalDelivered });
        }
        [HttpGet("total-orders-by-livestream/{livestreamId}")]
        public async Task<IActionResult> GetTotalOrdersByLivestreamId(string livestreamId)
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var totalOrders = await _revenueRepository.GetTotalOrderByLivestreamId(userId, livestreamId);
            return Ok(new { TotalOrders = totalOrders });
        }
        [HttpGet("total-orders-cancelled-by-livestream/{livestreamId}")]
        public async Task<IActionResult> GetTotalOrdersCancelledByLivestreamId(string livestreamId)
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var totalCancelled = await _revenueRepository.GetTotalOrederCancelledByLivestreamId(userId, livestreamId);
            return Ok(new { TotalOrdersCancelled = totalCancelled });
        }
        [HttpGet("total-orders-returned-by-livestream/{livestreamId}")]
        public async Task<IActionResult> GetTotalOrdersReturnedByLivestreamId(string livestreamId)
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var totalReturned = await _revenueRepository.GetTotalOrederReturnedByLivestreamId(userId, livestreamId);
            return Ok(new { TotalOrdersReturned = totalReturned });
        }
        [HttpGet("total-orders-delivered-by-livestream/{livestreamId}")]
        public async Task<IActionResult> GetTotalOrdersDeliveredByLivestreamId(string livestreamId)
        {
            // Lấy UserID từ token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var totalDelivered = await _revenueRepository.GetTotalOrederDeliveredByLivestreamId(userId, livestreamId);
            return Ok(new { TotalOrdersDelivered = totalDelivered });
        }
    }
}