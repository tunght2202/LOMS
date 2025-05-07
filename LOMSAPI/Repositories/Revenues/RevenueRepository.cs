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


        public async Task<decimal> GetRevenueByLivestreamId(string userid, string livestreamId)
        {
            try
            {
                return await _context.Orders
                             .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == livestreamId
                             && o.Status == OrderStatus.Delivered
                             && o.Product.UserID == userid)
                              .Select(o => (o.CurrentPrice ?? 0) * o.Quantity) // Projection chỉ lấy CurrentPrice và Quantity
                            .SumAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calculating revenue by livestream", ex);
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
                             && o.OrderDate <= endDate
                             && o.Product.UserID == userid)
                            .Select(o => (o.CurrentPrice ?? 0) * o.Quantity) // Projection chỉ lấy CurrentPrice và Quantity
                            .SumAsync();
            }
            catch (Exception ex)
            {
                // Log the exception here in a real application
                throw new InvalidOperationException("Error calculating revenue by date range", ex);
            }
        }

        public Task<int> GetTotalOrderByLivestreamId(string userid, string livestreamId)
        {
            try
            {
                return _context.Orders
                    .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == livestreamId)
                    .Join(_context.Products,
                        order => order.ProductID,
                        product => product.ProductID,
                        (order, product) => new { order, product })
                       .Where(op => op.product.UserID == userid)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total orders by livestream", ex);
            }
        }

        public Task<int> GetTotalOrederCancelledByLivestreamId(string userid, string livestreamId)
        {
            try
            {
                return _context.Orders
                        .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == livestreamId && o.Status == OrderStatus.Canceled)
                        .Join(_context.Products,
                            order => order.ProductID,
                            product => product.ProductID,
                            (order, product) => new { order, product })
                           .Where(op => op.product.UserID == userid)
                        .CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total canceled orders by livestream", ex);
            }
        }
        public Task<int> GetTotalOrederReturnedByLivestreamId(string userid, string livestreamId)
        {
            try
            {
                return _context.Orders
                .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == livestreamId && o.Status == OrderStatus.Returned)
                .Join(_context.Products,
                    order => order.ProductID,
                    product => product.ProductID,
                    (order, product) => new { order, product })
                   .Where(op => op.product.UserID == userid)
                .CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total canceled orders by livestream", ex);
            }
        }

        public Task<int> GetTotalOrederDeliveredByLivestreamId(string userid, string livestreamId)
        {
            try
            {
                return _context.Orders
                        .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == livestreamId && o.Status == OrderStatus.Delivered)
                        .Join(_context.Products,
                            order => order.ProductID,
                            product => product.ProductID,
                            (order, product) => new { order, product })
                           .Where(op => op.product.UserID == userid)
                        .CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total canceled orders by livestream", ex);
            }
        }

        public Task<int> GetTotalOrdersByDateRange(string userid, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _context.Products
                    .Where(p => p.UserID == userid)
                    .Join(_context.Orders,
                        product => product.ProductID,
                        order => order.ProductID,
                        (product, order) => order)
                    .CountAsync(o => o.OrderDate >= startDate && o.OrderDate <= endDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total orders by date range", ex);
            }
        }
        public Task<int> GetTotalOrederCancelledByDateRange(string userid, DateTime startDate, DateTime endDate)
        {

            try
            {
                return _context.Products
                    .Where(p => p.UserID == userid)
                    .Join(_context.Orders,
                        product => product.ProductID,
                        order => order.ProductID,
                        (product, order) => order)
                    .CountAsync(o => o.Status == OrderStatus.Canceled && o.OrderDate >= startDate && o.OrderDate <= endDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total canceled orders by date range", ex);
            }
        }
        public Task<int> GetTotalOrederReturnedByDateRange(string userid, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _context.Products
                    .Where(p => p.UserID == userid)
                    .Join(_context.Orders,
                        product => product.ProductID,
                        order => order.ProductID,
                        (product, order) => order)
                    .CountAsync(o => o.Status == OrderStatus.Returned && o.OrderDate >= startDate && o.OrderDate <= endDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total returned orders by date range", ex);
            }
        }
        public Task<int> GetTotalOrederDelivered(string userid, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _context.Products
                    .Where(p => p.UserID == userid)
                    .Join(_context.Orders,
                        product => product.ProductID,
                        order => order.ProductID,
                        (product, order) => order)
                    .CountAsync(o => o.Status == OrderStatus.Delivered && o.OrderDate >= startDate && o.OrderDate <= endDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error counting total delivered orders by date range", ex);

            }
        }
    }
}