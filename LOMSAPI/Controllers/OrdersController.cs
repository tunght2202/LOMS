﻿using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using LOMSAPI.Repositories.Users;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IDistributedCache _cache;
        private readonly IUserRepository _userRepository;
        public OrdersController(IOrderRepository context,IDistributedCache cache, IUserRepository userRepository)
        {
            _orderRepo = context;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            if(orders == null || !orders.Any())
                return NotFound("No orders found.");
            return Ok(orders);
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetAllByUserId()
        {
            string userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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

        [HttpGet("LivestreamCustomer/{livestreamCustomerID}/Product/{ProductID}")]
        public async Task<IActionResult> OrderByProductCodeModel(int livestreamCustomerID, int ProductID)
        {
            var orderByProductCodeModeal = await _orderRepo.OrderByProductCodeModel(livestreamCustomerID, ProductID);
            if (orderByProductCodeModeal == null) return NotFound();
            return Ok(orderByProductCodeModeal);
        }
        [HttpGet("LivestreamCustomer/{livestreamCustomerID}")]
        public async Task<IActionResult> GetOrderByLiveStreamCustoemrModel(int livestreamCustomerID)
        {
            var orderByLiveStreamCustmer = await _orderRepo.GetOrderByLiveStreamCustoemrModel(livestreamCustomerID);
            if (orderByLiveStreamCustmer == null) return NotFound();
            return Ok(orderByLiveStreamCustmer);
        }

        [HttpGet("GetListOrderByLiveStreamCustoemrModel")]
        public async Task<IActionResult> GetListOrderByLiveStreamCustoemrModel()
        {
            string userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ListOrderByLiveStreamCustmer = await _orderRepo.GetListOrderByLiveStreamCustoemrModel(userID);
            if (ListOrderByLiveStreamCustmer == null) return NotFound();
            return Ok(ListOrderByLiveStreamCustmer);
        }

        [HttpGet("GetListOrderByLiveStreamCustomerByLiveStremModel/LiveStream/{liveStreamId}")]
        public async Task<IActionResult> GetListOrderByLiveStreamCustomerByLiveStremModel(string liveStreamId)
        {
            var ListOrderByLiveStreamCustmer = await _orderRepo.GetListOrderByLiveStreamCustoemrLiveStremModel(liveStreamId);
            if (ListOrderByLiveStreamCustmer == null) return NotFound();
            return Ok(ListOrderByLiveStreamCustmer);
        }

        [HttpGet("GetListOrderByLiveStreamCustomerByCustomerModel/Customer/{customerId}")]
        public async Task<IActionResult> GetListOrderByLiveStreamCustomerByCustomerModel(string customerId)
        {
            var ListOrderByLiveStreamCustmer = await _orderRepo.GetListOrderByLiveStreamCustoemrByCustoemrModel(customerId);
            if (ListOrderByLiveStreamCustmer == null) return NotFound();
            return Ok(ListOrderByLiveStreamCustmer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string commentId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await _userRepository.GetUserById(userId);
            string accessToken = user.TokenFacbook;
            var result = await _orderRepo.AddOrderAsync(commentId, accessToken);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Can't create this order");
        }
         

        [HttpPost("CreateOrderFromComments/LiveStreamID/{liveStreamId}")]
        public async Task<IActionResult> CreateOrderFromComments(string liveStreamId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await _userRepository.GetUserById(userId);
            string accessToken = user.TokenFacbook;
            if (liveStreamId == null) return BadRequest("liveStreamId is null");
            var result = await _orderRepo.CreateOrderFromComments(liveStreamId, accessToken);
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
            try
            {
                var result = await _orderRepo.UpdateStatusOrderAsync(id, status);
                return result > 0 ? Ok() : NotFound("Can't update status of this order");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("status-check/{id}")]
        public async Task<IActionResult> UpdateStatusCheck(int id, [FromForm] bool newStatusCheck)
        {
            var result = await _orderRepo.UpdateStatusCheckOrderAsync(id, newStatusCheck);
            if (result >= 0)
                return Ok(new { message = "Status check updated successfully" });
            return NotFound(new { message = "Order not found or update failed" });
        }



        [HttpPut("TestPrint")]
        public async Task<IActionResult> TestPrint()
        {
            await _orderRepo.PrinTest();
            return Ok("Test print success");
        }

        [HttpPut("UpdateStatusOrderByLiveStreamCustoemrAsync/LivestreamCustomer/{livestreamCustomerID}/Status/{newStatus}")]
        public async Task<IActionResult> UpdateStatusOrderByLiveStreamCustoemrAsync(int livestreamCustomerID, OrderStatus newStatus)
        {
            var updateStatus = await _orderRepo.UpdateStatusOrderByLiveStreamCustoemrAsync(livestreamCustomerID, newStatus);
            if (updateStatus > 0)
            {
                return Ok(new { message = "Status updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Order not found or update failed" });
            }
        }

        [HttpPut("UpdateStatusCalldAsync/LivestreamCustomer/{livestreamCustomerID}/statusCheck/{statusCheck}")]
        public async Task<IActionResult> UpdateStatusCalldAsync(int livestreamCustomerID, bool statusCheck)
        {
            var updateStatus = await _orderRepo.UpdateStatusCalldAsync(livestreamCustomerID, statusCheck);
            if (updateStatus > 0)
            {
                return Ok(new { message = "Status updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Order not found or update failed" });
            }
        }

        [HttpPut("UpdateStatusTrackingNumberdAsync/LivestreamCustomer/{livestreamCustomerID}/trackingNumber/{trackingNumber}/Note/{note}")]
        public async Task<IActionResult> UpdateStatusTrackingNumberdAsync(int livestreamCustomerID, string trackingNumber, string note)
        {
            var updateStatusTrackingNumberdAsync = await _orderRepo.UpdateStatusTrackingNumberdAsync(livestreamCustomerID, trackingNumber, note);
            if (updateStatusTrackingNumberdAsync > 0)
            {
                return Ok(new { message = "Updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Order not found or update failed" });
            }
        }
    }
}
