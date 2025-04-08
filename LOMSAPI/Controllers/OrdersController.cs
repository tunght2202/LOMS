using LOMSAPI.Data.Entities;
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
        // Get all orders
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.GetOrders();
            if(orders.Any())
            {
                return Ok(orders);
            }
            return BadRequest("Not exit any order");
        }

        // Thanh Tùng
        // Get order by id
        [HttpGet("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _context.GetOrdersById(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
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

        // Thanh Tùng
        // Get list order by User Id
        [HttpGet("GetOrderByUserId/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            try
            {
                var orders = await _context.GetOrderByUserId(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        // Thanh Tùng
        // Get order and order detail by livestream customer id
        [HttpGet("GetOrderByLivestreamCustomerId/{livstreamCustomerId}")]
        public async Task<IActionResult> GetOrderByLivestreamCustomerId(int livstreamCustomerId)
        {
            var result = await _context.GetOrderByLivestreamCustomerId(livstreamCustomerId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        
        // Thanh Tùng
        // Get list order by status and livestream
        [HttpGet("GetOrderByStatusAndLivestreams/livestream/{livestreamId}/status/{status}")]
        public async Task<IActionResult> GetOrdersByStatusByLivestreamId(string livestreamId, OrderStatus status)
        {
            try
            {
                var orders = await _context.GetOrdersByStatusByLivestreamId(livestreamId, status);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Thanh Tùng
        // Get list order by status and user id
        [HttpGet("GetOrderByStatusAndUser/user/{userId}/status/{status}")]
        public async Task<IActionResult> GetOrdersByStatusByUserId(string userId, OrderStatus status)
        {
            try
            {
                var orders = await _context.GetOrdersByStatusByUserId(userId, status);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        // Thanh Tùng
        // Add new Order Detail
        [HttpPost("AddOrderDetail")]
        public async Task<IActionResult> AddOrderDetail([FromForm] OrderDetailAddModel orderModel)
        {
            var result = await _context.CreateOrderDetail(orderModel);
            if (result == null || result < 1)
            {
                return BadRequest("can create");
            }
            return Ok(result);
        }

        // Thanh Tùng
        // Update Order Detail by
        [HttpPut("UpdateOrderDetail")]
        public async Task<IActionResult> UpdateOrderDetail([FromBody] UpdateOrderDetailModel request)
        {
            if (request == null || request.OrderDetailID <= 0 || request.Quantity <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ!");
            }

            var result = await _context.UpdateOrderDetail(request.OrderDetailID, request.Quantity);
            return result > 0 ? Ok("Cập nhật thành công") : BadRequest("Cập nhật thất bại");
        }

        // Thanh Tùng
        // Update new status order
        [HttpPut("UpdateStatus/order/{orderId}/status/{newStatus}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            try
            {
                var result = await _context.UpdateOrderStatus(orderId, newStatus);
                return result ? Ok("Cập nhật trạng thái thành công") : BadRequest("Cập nhật thất bại");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Thanh Tùng
        // Delete Order Detail
        [HttpDelete("DeleteOrderDetail/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderDetailId)
        {
            if (orderDetailId <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ!");
            }

            var result = await _context.DeleteOrderDetail(orderDetailId);
            return result > 0 ? Ok("Xóa thành công") : BadRequest("Xóa thất bại");
        }
    }
}
