
using Azure.Core;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LOMSAPI.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        public OrderRepository(LOMSDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        private OrderModel MapToModel(Order order)
        {
            return new OrderModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Quantity = order.Quantity,
                ProductID = order.ProductID,
                CommentID = order.CommentID
            };
        }

        private Order MapToEntity(OrderModel model)
        {
            return new Order
            {
                OrderID = model.OrderID,
                OrderDate = model.OrderDate,
                Status = model.Status,
                Quantity = model.Quantity,
                ProductID = model.ProductID,
                CommentID = model.CommentID
            };
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrdersByUserIdAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.CommentID != null) 
                .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrdersByLiveStreamIdAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.CommentID != null) 
                .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByCustomerIdAsync(string customerId)
        {
            var orders = await _context.Orders
                .Where(o => o.Comment.LiveStreamCustomer.CustomerID.Equals(customerId))
                .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }
        public async Task<IEnumerable<OrderModel>> GetOrdersByLiveStreamCustomerIdAsync(int liveStreamCustomerID)
        {
            var orders = await _context.Orders
                .Where(o => o.Comment.LiveStreamCustomerID == liveStreamCustomerID)
                .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }
        public async Task<OrderModel?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderId);
            return order != null ? MapToModel(order) : null;
        }

        public async Task<bool> OrderExistsAsync(int orderId)
        {
            return await _context.Orders.AnyAsync(o => o.OrderID == orderId);
        }

        public async Task<int> AddOrderAsync(OrderModel orderModel)
        {
            var order = MapToEntity(orderModel);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order.OrderID;
        }

        public async Task<int> UpdateOrderAsync(OrderModel orderModel)
        {
            var existing = await _context.Orders.FindAsync(orderModel.OrderID);
            if (existing == null) return 0;

            existing.OrderDate = orderModel.OrderDate;
            existing.Status = orderModel.Status;
            existing.Quantity = orderModel.Quantity;
            existing.ProductID = orderModel.ProductID;
            existing.CommentID = orderModel.CommentID;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStatusOrderAsync(int orderId, int newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return 0;

            order.Status = (OrderStatus)newStatus;
            return await _context.SaveChangesAsync();
        }

    }
}
