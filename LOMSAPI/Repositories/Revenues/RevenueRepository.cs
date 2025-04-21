using LOMSAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LOMSAPI.Repositories.Revenues
{
    public class RevenueRepository : IRevenueRepository
    {
        private readonly LOMSDbContext _context;

        public RevenueRepository(LOMSDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<decimal> GetTotalRevenue()
        {
            try
            {
                // Assuming there's a Price property in Product since OrderDetails isn't available
                return await _context.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error calculating total revenue", ex);
            }
        }

        public async Task<int> GetTotalOrders()
        {
            try
            {
                return await _context.Orders.CountAsync();
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total orders", ex);
            }
        }

        //Note: This method needs LiveStreamCustomerID which wasn't in the original Order entity
        // I'll comment it out and provide an alternative

        public async Task<decimal> GetRevenueByLivestreamId(string livestreamId)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == livestreamId && o.Status == OrderStatus.Delivered)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calculating revenue by livestream", ex);
            }
        }


        // Alternative version using ProductID instead
        public async Task<decimal> GetRevenueByProductId(int productId)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.ProductID == productId && o.Status == OrderStatus.Delivered)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calculating revenue by product", ex);
            }
        }

        public async Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    throw new ArgumentException("Start date must be before end date");

                return await _context.Orders
                    .Where(o => o.Status == OrderStatus.Delivered
                             && o.OrderDate >= startDate
                             && o.OrderDate <= endDate)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error calculating revenue by date range", ex);
            }
        }

        public Task<int> GetTotalOrederCancelled()
        {
            try
            {
                return _context.Orders.CountAsync(o => o.Status == OrderStatus.Canceled);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total canceled orders", ex);
            }
        }
        public Task<int> GetTotalOrederReturned()
        {
            try
            {
                return _context.Orders.CountAsync(o => o.Status == OrderStatus.Returned);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total returned orders", ex);
            }
        }

        public Task<int> GetTotalOrederDelivered()
        {
            try
            {
                return _context.Orders.CountAsync(o => o.Status == OrderStatus.Delivered);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total delivered orders", ex);
            }
        }
    }
}