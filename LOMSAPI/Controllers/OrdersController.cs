using CloudinaryDotNet.Actions;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IDistributedCache _cache;
        private readonly LOMSDbContext _context;
        public OrdersController(IOrderRepository context,IDistributedCache cache, LOMSDbContext lomscontext)
        {
            _orderRepo = context;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _context = lomscontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            if(orders == null || !orders.Any())
                return NotFound("No orders found.");
            return Ok(orders);
        }

        [HttpGet("user/{userID}")]
        public async Task<IActionResult> GetAllByUserId(string userID)
        {
            var orders = await _orderRepo.GetAllOrdersByUserIdAsync(userID);
            if (orders == null || !orders.Any())
                return NotFound("No orders found for the user.");
            return Ok(orders);
        }

        [HttpGet("livestream/{liveStreamID}")]
        public async Task<IActionResult> GetAllByLiveStreamId(string liveStreamID)
        {
            var orders = await _orderRepo.GetAllOrdersByLiveStreamIdAsync(liveStreamID);
            if (orders == null || !orders.Any())
                return NotFound("No orders found for the livestream.");
            return Ok(orders);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var orders = await _orderRepo.GetOrdersByCustomerIdAsync(customerId);
            if (orders == null || !orders.Any())
                return NotFound($"No orders found for customer ID: {customerId}");
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderModel model)
        {
            var orderId = await _orderRepo.AddOrderAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = orderId }, model);
        }


        [HttpPost("CreateOrderFromComments/LiveStreamID/{liveStreamId}")]
        public async Task<IActionResult> CreateOrderFromComments(string liveStreamId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (liveStreamId == null) return BadRequest("liveStreamId is null");
            var result = await _orderRepo.CreateOrderFromComments(liveStreamId, userId);
            return result > 0 ? Ok(result) : NotFound("Can't create this order");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderModel model)
        {
            if (id != model.OrderID) return BadRequest();
            var result = await _orderRepo.UpdateOrderAsync(model);
            return result > 0 ? Ok() : NotFound("Can't update this order");
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromForm] OrderStatus status)
        {
            var result = await _orderRepo.UpdateStatusOrderAsync(id, status);
            return result > 0 ? Ok() : NotFound("Can;t update status of this order");
        }
    }
}
