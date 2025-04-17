using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        // Thanh Tùng
        // Get customer by ID  
        [HttpGet("GetCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById(string id) 
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null) { return BadRequest($"{id} not exit"); }
            return Ok(customer);
        }
        // Thanh Tùng
        // Get customer by order ID 
        [HttpGet("Order/{orderID}")]
        public async Task<IActionResult> GetByOrderID(int orderID) 
        {
            var customer = await _customerRepository.GetCustomerByOrderIdAsync(orderID);
            if (customer == null) { return BadRequest($"{orderID} not exit"); }
            return Ok(customer);
        }
        // Thanh Tùng
        // Get customer by User ID 
        [HttpGet("User")]

        public async Task<IActionResult> GetByUserID() 
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var customer = await _customerRepository.GetCustomersByUserIdAsync(userId);
            if (customer == null) { return BadRequest($"{userId} not exit"); }
            return Ok(customer);
        }
        // Thanh Tùng
        // Get customer by LiveStream ID 
        [HttpGet("LiveStream/{liveStreamID}")]
        public async Task<IActionResult> GetByLiveStreamID(string liveStreamID) 
        {
            var customer = await _customerRepository.GetCustomersByLiveStreamIdAsync(liveStreamID);
            if (customer == null) { return BadRequest($"{liveStreamID} not exit"); }
            return Ok(customer);
        }
        // Thanh Tùng
        // Add customer by id and Facebook Name  
        [HttpPost("AddCustomer/{customerId}/{customerFacebookName}")]
        public async Task<IActionResult> AddCustomer(string customerId, string customerFacebookName)
        {
            var result = await _customerRepository.AddCustomer(customerId, customerFacebookName);
            if(result == 0)
            {
                return BadRequest($"Can't add {customerFacebookName} customer!");
            }

            return Ok();
        }
        // Thanh Tùng
        // Update customer by customerId
        [HttpPut("UpdateCustomerByID/{customerId}")]
        public async Task<IActionResult> UpdateCustomerByID(string customerId,[FromBody] UpdateCustomerModel customerUpdate)
        {
            var result = await _customerRepository.UpdateCustomer(customerId, customerUpdate);
            if(result == 0)
            {
                return BadRequest($"Can't update customer!");
            }

            return Ok();
        }
    }
}
