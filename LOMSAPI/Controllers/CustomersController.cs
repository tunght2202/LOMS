using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("GetCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById(string id) 
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null) { return BadRequest($"{id} not exit"); }
            return Ok(customer);
        }

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
    }
}
