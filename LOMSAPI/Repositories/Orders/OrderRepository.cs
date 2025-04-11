
using Azure.Core;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.ListProducts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LOMSAPI.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IListProductRepository _listProductRepository;

        public OrderRepository(LOMSDbContext context, HttpClient httpClient
            , IListProductRepository listProductRepository)
        {
            _context = context;
            _httpClient = httpClient;
            _listProductRepository = listProductRepository;
        }

        private OrderModel MapToModel(Order order)
        {
            return new OrderModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
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
                Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), model.Status),
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

        public async Task<IEnumerable<OrderModel>> GetAllOrdersByLiveStreamIdAsync(string liveStreamId)
        {
            var orders = await _context.Orders
                .Where(o => o.Comment.LiveStreamCustomer.LivestreamID.Equals(liveStreamId))
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
            existing.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), orderModel.Status);
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

        public async Task<IEnumerable<OrderModel>> GetAllOrdersByUserIdAsync(string userID)
        {
            var orders = await _context.Orders
                        .Where(o => o.CommentID.Equals(userID))
                        .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        // Kiểm tra thời gian order xem có bị trùng nhau không
        // gủi messager
        // cộng trừ trong kho 
        // update số lượng sản phẩm trong order thì update luôn số lương sản phẩm trong stock

        public async Task<int> CreateOrderFromComments(int listProductID, string liveStreamId)
        {
            if(listProductID <= 0 || listProductID == null)
            {
                throw new ArgumentException("Invalid ListProductID");
            }
            var liveStream = await _context.LiveStreams
                .FirstOrDefaultAsync(l => l.LivestreamID.Equals(liveStreamId));
            if (liveStream == null)
            {
                throw new ArgumentException("Invalid LiveStreamID");
            }
            var products = await _listProductRepository.GetProductListProductById(listProductID);
            var productCodeToId = products.ToDictionary(p => p.ProductCode.ToLower(), p => p.ProductID);
            var comments = await _context.Comments
                .Where(c => c.LiveStreamCustomer.LivestreamID.Equals(liveStreamId))
                .ToListAsync();
            var result = new List<Order>();
            var regex = new Regex(@"(?<code>\w+)\s*(?:[xX]?\s*)(?<qty>\d+)", RegexOptions.IgnoreCase);
            // (?< code >\w +)   // Nhóm "code": Mã sản phẩm (chữ + số)
            //\s *               // Khoảng trắng (có thể có)
            //(?: [xX] ?\s *)    // Nhóm không bắt (non-capturing): có hoặc không có 'x' hoặc 'X', sau đó khoảng trắng
            //(?< qty >\d +)     // Nhóm "qty": Số lượng sản phẩm
            foreach (var comment in comments.OrderBy(c => c.CommentTime))
            {
                var match = regex.Match(comment.Content);
                if (match.Success)
                {
                    string code = match.Groups["code"].Value.ToLower();
                    int quantity = int.Parse(match.Groups["qty"].Value);

                    if (productCodeToId.TryGetValue(code, out int productId))
                    {
                        result.Add(new Order
                        {
                            ProductID = productId,
                            Quantity = quantity,
                            CommentID = comment.CommentID,
                            OrderDate = comment.CommentTime
                        });
                    }
                }
            }
            if (result.Count > 0)
            {
                await _context.Orders.AddRangeAsync(result);
                return await _context.SaveChangesAsync(); ;
            }
            return 0;
        }

    }
}
