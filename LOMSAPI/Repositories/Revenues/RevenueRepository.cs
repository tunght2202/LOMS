using LOMSAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LOMSAPI.Repositories.Revenues
{
    public class RevenueRepository : IRevenueRepository
    {
        private readonly LOMSDbContext _context;
        public RevenueRepository(LOMSDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetTotalRevenue()
        {
            return await _context.Orders
               .Where(o => o.Status == OrderStatus.Delivered)
               .SelectMany(o => o.OrderDetails)
               .SumAsync(od => od.Price * od.Quantity);
        }
        public async Task<int> GetTotalOrders()
        {
            return await _context.Orders.CountAsync();
        }
        public async Task<decimal> GetRevenueByLivestreamId(int livestreamId)
        {
            return await _context.Orders
                .Where(o => o.LiveStreamCustomerID == livestreamId && o.Status == OrderStatus.Delivered)
                .SelectMany(o => o.OrderDetails)
                .SumAsync(od => od.Price * od.Quantity);
        }
    }

}
