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

        public async Task<decimal> GetTotalRevenue(string userid)
        {
            try
            {
                // Find revenue from all delivered orders for products owned by this user
                return await _context.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                    .Where(op => op.product.UserID == userid)
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error calculating total revenue", ex);
            }
        }

        public async Task<int> GetTotalOrders(string userid)
        {
            try
            {
              return await _context.Products
             .Where(p => p.UserID == userid)
             .Join(_context.Orders,
                 product => product.ProductID,
                 order => order.ProductID,
                 (product, order) => order)
             .CountAsync();
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total orders", ex);
            }
        }

        //Note: This method needs LiveStreamCustomerID which wasn't in the original Order entity
        // I'll comment it out and provide an alternative

        public async Task<decimal> GetRevenueByLivestreamId(string userid, string livestreamId)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == livestreamId && o.Status == OrderStatus.Delivered)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                       .Where(op => op.product.UserID == userid)
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calculating revenue by livestream", ex);
            }
        }


        // Alternative version using ProductID instead
        public async Task<decimal> GetRevenueByProductId(string userid, int productId)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.ProductID == productId && o.Status == OrderStatus.Delivered)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                       .Where(op => op.product.UserID == userid)
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calculating revenue by product", ex);
            }
        }

        public async Task<decimal> GetRevenueByDateRange(string userid, DateTime startDate, DateTime endDate)
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
                       .Where(op => op.product.UserID == userid)
                    .SumAsync(op => op.product.Price * op.order.Quantity);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error calculating revenue by date range", ex);
            }
        }

        public async Task<int> GetTotalOrederCancelled(string userid)
        {
            try
            {
                return await _context.Products
              .Where(p => p.UserID == userid)
              .Join(_context.Orders,
                  product => product.ProductID,
                  order => order.ProductID,
                  (product, order) => order)
              .CountAsync(o => o.Status == OrderStatus.Canceled);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total canceled orders", ex);
            }
        }
        public async Task<int> GetTotalOrederReturned(string userid)
        {
            try
            {
                return await _context.Products
             .Where(p => p.UserID == userid)
             .Join(_context.Orders,
                 product => product.ProductID,
                 order => order.ProductID,
                 (product, order) => order)
             .CountAsync(o => o.Status == OrderStatus.Returned);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total returned orders", ex);
            }
        }

        public async Task<int> GetTotalOrederDelivered(string userid)       
        {
            try
            {
                return await _context.Products.Where(p => p.UserID == userid)
                    .Join(_context.Orders, product => product.ProductID,
                    order => order.ProductID, 
                    (product, order) => order)
                    .CountAsync(o => o.Status == OrderStatus.Delivered);
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error counting total delivered orders", ex);
            }

            }
        }
}