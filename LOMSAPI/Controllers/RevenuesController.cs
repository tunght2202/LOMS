﻿using LOMSAPI.Repositories.Revenues;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
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
            var revenue = await _revenueRepository.GetTotalRevenue();
            return Ok(new { TotalRevenue = revenue });
        }

        /// <summary>
        /// API tính tổng số lượng đơn hàng.
        /// </summary>
        [HttpGet("total-orders")]
        public async Task<IActionResult> GetTotalOrders()
        {
            var orders = await _revenueRepository.GetTotalOrders();
            return Ok(new { TotalOrders = orders });
        }

        /// <summary>
        /// API tính doanh thu của một phiên livestream theo Livestream ID.
        /// </summary>
        [HttpGet("livestream-revenue/{livestreamId}")]
        public async Task<IActionResult> GetRevenueByLivestreamId(int livestreamId)
        {
            var revenue = await _revenueRepository.GetRevenueByLivestreamId(livestreamId);
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
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var revenue = await _revenueRepository.GetRevenueByDateRange(startDate, endDate);
            return Ok(new { StartDate = startDate, EndDate = endDate, TotalRevenue = revenue });
        }
    }
}
