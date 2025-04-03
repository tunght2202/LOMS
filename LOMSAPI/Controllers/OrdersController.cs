using LOMSAPI.Models;
using LOMSAPI.Repositories.Orders;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _context;

        public OrdersController(IOrderRepository context)
        {
            _context = context;
        }
        // Thanh Tùng
        // Add new Order Detail
        [HttpPost("AddOrderDetail")]
        public async Task<IActionResult> AddOrderDetail([FromForm] OrderDetailAddModel orderModel)
        {
            var result = await _context.CreateOrderDetail(orderModel);
            if(result == null || result < 1)
            {
                return BadRequest("can create");
            }
            return Ok(result);
        }
        // Thanh Tùng
        // Get order and order detail by livestream customer id
        [HttpGet("GetOrderByLivestreamCustomerId/{livstreamCustomerId}")]
        public async Task<IActionResult> GetOrderByLivestreamCustomerId(int livstreamCustomerId)
        {
            var result = await _context.GetOrderByLivestreamCustomerId(livstreamCustomerId);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        // Thanh Tùng
        // Get order and order detail by livestream customer id
        [HttpGet("GetOrderByLivestreamId/{livstreamId}")]
        public async Task<IActionResult> GetOrderByLivestreamId(string livstreamId)
        {
            var result = await _context.GetOrderByLivestreamId(livstreamId);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

    }
}
