using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LOMSAPI.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly LOMSDbContext _context;
        public CustomerRepository(LOMSDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddCustomer(string customerId, string customerFacebookName)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerID.Equals(customerId));
            if (customer != null) 
            {
                throw new Exception($"{customerFacebookName} was exit!");
            }
            var newCustomer = new Customer()
            {
                CustomerID = customerId,
                FacebookName = customerFacebookName
            };
            
            await _context.Customers.AddAsync(newCustomer);
            return _context.SaveChanges();
        }

        public async Task<int> AddLivestreamCustomer(string customerId, string livestreamId)
        {
            var livestreamCustomer = await _context.LiveStreamsCustomers
                .FirstOrDefaultAsync(c => c.CustomerID.Equals(customerId) && c.LivestreamID.Equals(livestreamId));
            if (livestreamCustomer != null)
            {
                throw new Exception($"this livestream customer was exit!");
            }
            var newLivestreamCustomer = new LiveStreamCustomer()
            {
                CustomerID = customerId,
                LivestreamID = livestreamId
            };

            await _context.LiveStreamsCustomers.AddAsync(newLivestreamCustomer);
            return _context.SaveChanges();
        }

        public async Task<GetCustomerModel> GetCustomerById(string customerId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerID.Equals(customerId));
            if (customer == null) 
            {
                throw new Exception($"{customerId} was exit!");
            }
            var getCustomer = new GetCustomerModel()
            {
                CustomerID = customerId,
                FacebookName = customer.FacebookName,
                FullName = customer.FullName,
                Email = customer.Email,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                FailedDeliveries = customer.FailedDeliveries,
                SuccessfulDeliveries = customer.SuccessfulDeliveries

            };

            return getCustomer;

        }

        public async Task<int> UpdateCustomer(string customerId, UpdateCustomerModel customerUpdate)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerID.Equals(customerId));
            if (customer == null)
            {
                throw new Exception($"{customerId} was exit!");
            }

            customer.FullName = customerUpdate.FullName;
            customer.Email = customerUpdate.Email;
            customer.Address = customerUpdate.Address;
            customer.PhoneNumber = customerUpdate.PhoneNumber;
            return await _context.SaveChangesAsync();
        }


        public async Task<GetCustomerModel> GetCustomerByOrderIdAsync(int orderID)
        {
            var customerId = await _context.Orders
                .Where(o => o.OrderID == orderID)
                .Select(o => new
                {
                    customerID = o.Comment.LiveStreamCustomer.CustomerID
                }
                )
                .FirstOrDefaultAsync();
            var customer = await _context.Customers
               .FirstOrDefaultAsync(c => c.CustomerID.Equals(customerId.customerID));
            if (customer == null)
            {
                throw new Exception($"{customerId} was exit!");
            }
            var getCustomer = new GetCustomerModel()
            {
                CustomerID = customerId.customerID,
                FacebookName = customer.FacebookName,
                FullName = customer.FullName,
                Email = customer.Email,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber,
                FailedDeliveries = customer.FailedDeliveries,
                SuccessfulDeliveries = customer.SuccessfulDeliveries

            };
            return getCustomer;
        }
    }
}
