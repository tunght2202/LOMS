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
                ImageURL = customer.ImageURL,
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

        public async Task<IEnumerable<GetCustomerModel>> GetCustomersByUserIdAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));
            if (user == null)
            {
                throw new Exception($"{userId} was exit!");
            }
            var customers = await _context.LiveStreamsCustomers
                .Where(lc => lc.LiveStream.UserID.Equals(userId))
                .Include(cs => cs.Customer)
                .Select(c => new GetCustomerModel()
                {
                    CustomerID = c.CustomerID,
                    FacebookName = c.Customer.FacebookName,
                    FullName = c.Customer.FullName,
                    Email = c.Customer.Email,
                    Address = c.Customer.Address,
                    PhoneNumber = c.Customer.PhoneNumber,
                    FailedDeliveries = c.Customer.FailedDeliveries,
                    SuccessfulDeliveries = c.Customer.SuccessfulDeliveries
                })
                .ToListAsync();
            if(customers == null)
            {
                throw new Exception($"No customers found for user {userId}");
            }
            return customers;
        }

        public async Task<IEnumerable<GetCustomerModel>> GetCustomersByLiveStreamIdAsync(string liveStreamId)
        {
            var liveStream = await _context.LiveStreams
                .FirstOrDefaultAsync(x => x.LivestreamID.Equals(liveStreamId));
            if (liveStream == null)
            {
                throw new Exception($"{liveStream} was exit!");
            }
            var customers = await _context.LiveStreamsCustomers
                .Where(lc => lc.LivestreamID.Equals(liveStreamId))
                .Select(c => new GetCustomerModel()
                {
                    CustomerID = c.CustomerID,
                    FacebookName = c.Customer.FacebookName,
                    FullName = c.Customer.FullName,
                    Email = c.Customer.Email,
                    Address = c.Customer.Address,
                    PhoneNumber = c.Customer.PhoneNumber,
                    FailedDeliveries = c.Customer.FailedDeliveries,
                    SuccessfulDeliveries = c.Customer.SuccessfulDeliveries
                })
                .ToListAsync();
            if (customers == null)
            {
                throw new Exception($"No customers found for liveStream {liveStreamId}");
            }
            return customers;
        }
    }
}
