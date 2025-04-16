﻿
using Azure;
using Azure.Core;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.ListProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        private readonly IConfiguration _configuration;
        private string ACCESS_TOKEN;
        public OrderRepository(LOMSDbContext context, HttpClient httpClient
            , IListProductRepository listProductRepository, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _listProductRepository = listProductRepository;
            _configuration = configuration;
            ACCESS_TOKEN = "EAAIYLfie53cBOzepCakr5Kp7jI8aZBFoxNkDOBqLZBYAqh6pTXkDgaIVAD2KZCJsZAlceo5wdJml1BMZAgZAaGZBA9rAfOah75Lmi8SAZAKFov98yqzs8uUY5CYHyNHcPabDuEcCMjZCPeNRFP4ZBdlcN9E9LKaWQaODAykzSc8icDrQzyUVY2vAanW1qBJaqa1jRCLVdJTEBOZC8tJmtV7kLoTJjcZD";
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
                CommentID = order.CommentID,
                Product = new ProductModel()
                {
                    ProductID = order.ProductID,
                    ProductCode = order.Product.ProductCode,
                    Description = order.Product.Description,
                    Name = order.Product.Name,
                    ImageURL = order.Product.ImageURL,
                    Price = order.Product.Price,
                    Stock = order.Product.Stock,
                    Status = order.Product.Status

                }
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
            var orders = await _context.Orders.Include(o => o.Product).ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrdersByLiveStreamIdAsync(string liveStreamId)
        {
            var orders = await _context.Orders.Include(o => o.Product)
                .Where(o => o.Comment.LiveStreamCustomer.LivestreamID.Equals(liveStreamId))
                .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByCustomerIdAsync(string customerId)
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.Comment.LiveStreamCustomer.CustomerID.Equals(customerId))
                .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByLiveStreamCustomerIdAsync(int liveStreamCustomerID)
        {
            var orders = await _context.Orders.Include(o => o.Product)
                .Where(o => o.Comment.LiveStreamCustomerID == liveStreamCustomerID)
                .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }
        public async Task<OrderModel?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
            return order != null ? MapToModel(order) : null;
        }

        public async Task<bool> OrderExistsAsync(int orderId)
        {
            return await _context.Orders.AnyAsync(o => o.OrderID == orderId);
        }
        // order thủ công 
        public async Task<int> AddOrderAsync(OrderModel orderModel)
        {
            orderModel.OrderDate = DateTime.Now;
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

        public async Task<int> UpdateStatusOrderAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return 0;
            if(newStatus < order.Status)
            {
                throw new Exception("Can't change status");
                return 0;
            }
            if (order.Status == OrderStatus.Canceled)
            {
                throw new Exception("Can't change status");
                return 0;
            }
            var getProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == order.ProductID);
            if (getProduct == null)
            {
                throw new Exception($"This id {order.ProductID} invite ");
            }
            if(newStatus == OrderStatus.Canceled || newStatus == OrderStatus.Returned)
            {
                getProduct.Stock += order.Quantity;
            }
           
            order.Status = (OrderStatus)newStatus;
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrdersByUserIdAsync(string userID)
        {
            var orders = await _context.Orders
                        .Include(o => o.Product)
                        .Where(o => o.CommentID.Equals(userID))
                        .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }

        // Kiểm tra thời gian order xem có bị trùng nhau không
        // gủi messager
        // cộng trừ trong kho 
        // update số lượng sản phẩm trong order thì update luôn số lương sản phẩm trong stock

        public async Task<int> CreateOrderFromComments2(string liveStreamId)
        {
            try
            {

                var liveStream = await _context.LiveStreams
                    .FirstOrDefaultAsync(l => l.LivestreamID.Equals(liveStreamId));

                if (liveStream == null)
                {
                    throw new ArgumentException("Invalid LiveStreamID");
                }
                if (liveStream.ListProductID == null)
                {
                    throw new ArgumentException("Invalid ListProductID");
                }

                var listProductID = liveStream.ListProductID.Value;

                var products = await _listProductRepository.GetProductListProductById(listProductID);
                var productCodeToId = products.ToDictionary(p => p.ProductCode.ToLower(), p => p.ProductID);
                var order = await _context.Orders
                    .Include(o => o.Comment)
                    .ThenInclude(c => c.LiveStreamCustomer)
                    .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == liveStreamId)
                    .OrderByDescending(o => o.OrderDate)
                    .FirstOrDefaultAsync();
                var comments = new List<Comment>();
                if (order == null)
                {
                    comments = await _context.Comments
                    .Where(c => c.LiveStreamCustomer.LivestreamID.Equals(liveStreamId))
                    .ToListAsync();
                }
                else
                {
                    comments = await _context.Comments
                    .Where(c => (c.LiveStreamCustomer.LivestreamID.Equals(liveStreamId))
                    && (c.CommentTime >= order.Comment.CommentTime))
                    .ToListAsync();
                }
                // produccode xnumber prr 3, prt, 
                var result = 0;
                var regex = new Regex(@"\b(?<code>[a-zA-Z]+\d*)\b(?:\s*[xX]?\s*(?<qty>\d+))?", RegexOptions.IgnoreCase);

                foreach (var comment in comments.OrderBy(c => c.CommentTime))
                {
                    if (order == null)
                    {
                        var match = regex.Match(comment.Content);
                        if (match.Success)
                        {
                            string code = match.Groups["code"].Value.ToLower();
                            int quantity = int.Parse(match.Groups["qty"].Value);

                            if (productCodeToId.TryGetValue(code, out int productId))
                            {
                                var product = await _context.Products.FindAsync(productId);
                                if (product != null)
                                {
                                    product.Stock -= quantity;
                                    if (product.Stock < 0)
                                    {
                                        continue;
                                    }
                                }
                                var newOrder = new Order
                                {
                                    ProductID = productId,
                                    Quantity = quantity,
                                    CommentID = comment.CommentID,
                                    OrderDate = comment.CommentTime
                                };

                                await _context.Orders.AddAsync(newOrder);
                                result += await _context.SaveChangesAsync();
                                await SendMessageAsync(newOrder.OrderID);
                            }
                        }
                    }
                    else
                    {
                        if (comment.CommentID != order.CommentID)
                        {
                            var match = regex.Match(comment.Content);
                            if (match.Success)
                            {
                                string code = match.Groups["code"].Value.ToLower();
                                int quantity = 1;
                                if (match.Groups["qty"].Success)
                                {
                                    quantity = int.Parse(match.Groups["qty"].Value);
                                }

                                if (productCodeToId.TryGetValue(code, out int productId))
                                {
                                    var product = await _context.Products.FindAsync(productId);
                                    if (product != null)
                                    {
                                        
                                        if (product.Stock < quantity)
                                        {
                                            continue;
                                        }
                                        product.Stock -= quantity;
                                    }
                                    var newOrder = new Order
                                    {
                                        ProductID = productId,
                                        Quantity = quantity,
                                        CommentID = comment.CommentID,
                                        OrderDate = comment.CommentTime
                                    };

                                    await _context.Orders.AddAsync(newOrder);
                                    result += await _context.SaveChangesAsync();
                                    await SendMessageAsync(newOrder.OrderID);
                                }
                            }
                        }
                    }

                }
                
                    return result ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }
        public async Task<int> CreateOrderFromComments(string liveStreamId)
        {
            try
            {

                var liveStream = await _context.LiveStreams
                    .FirstOrDefaultAsync(l => l.LivestreamID.Equals(liveStreamId));

                if (liveStream == null)
                {
                    throw new ArgumentException("Invalid LiveStreamID");
                }
                if (liveStream.ListProductID == null)
                {
                    throw new ArgumentException("Invalid ListProductID");
                }

                var listProductID = liveStream.ListProductID.Value;

                var products = await _listProductRepository.GetProductListProductById(listProductID);
                var productCodeToId = products.ToDictionary(p => p.ProductCode.ToLower(), p => p.ProductID);
                var order = await _context.Orders
                    .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == liveStreamId)
                    .ToListAsync();
                var comments = new List<Comment>();
                    comments = await _context.Comments
                    .Where(c => c.LiveStreamCustomer.LivestreamID.Equals(liveStreamId))
                    .ToListAsync();
                // produccode xnumber prr 3, prt, 
                var result = 0;
                var regex = new Regex(@"\b(?<code>[a-zA-Z]+\d*)\b(?:\s*[xX]?\s*(?<qty>\d+))?", RegexOptions.IgnoreCase);

                foreach (var comment in comments.OrderBy(c => c.CommentTime))
                {
                    if((order.Count <= 0)||(!order.Any(x => x.CommentID.Equals(comment.CommentID))))
                    {

                    
                            var match = regex.Match(comment.Content);
                            if (match.Success)
                            {
                                string code = match.Groups["code"].Value.ToLower();
                                int quantity = 1;
                                if (match.Groups["qty"].Success)
                                {
                                    quantity = int.Parse(match.Groups["qty"].Value);
                                }

                                if (productCodeToId.TryGetValue(code, out int productId))
                                {
                                    var product = await _context.Products.FindAsync(productId);
                                    if (product != null)
                                    {
                                        
                                        if (product.Stock < quantity)
                                        {
                                            continue;
                                        }
                                        product.Stock -= quantity;
                                    }
                                    var newOrder = new Order
                                    {
                                        ProductID = productId,
                                        Quantity = quantity,
                                        CommentID = comment.CommentID,
                                        OrderDate = comment.CommentTime
                                    };

                                    await _context.Orders.AddAsync(newOrder);
                                    result += await _context.SaveChangesAsync();
                                }
                            }
                    }


                }
                
                    return result ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        private async Task SendMessageAsync(int orderID)
        {
            var Order = await _context.Orders.FindAsync(orderID);
            var Comment = await _context.Comments.FindAsync(Order.OrderID);
            var LiveStreamCustomer = await _context.LiveStreamsCustomers.FindAsync(Comment.LiveStreamCustomerID);
            var Customer = await _context.Customers.FindAsync(LiveStreamCustomer.CustomerID);
            bool IsOldCustomer = Customer.Address == null || Customer.PhoneNumber == null;
            var url = $"https://graph.facebook.com/v22.0/me/messages?access_token={ACCESS_TOKEN}";
            
            if (IsOldCustomer)
            {
                var payload = new
                {
                    recipient = new { id = Customer.CustomerID },
                    message = new
                    {
                        text = "Your order has been successfully created\n" +
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == Order.ProductID).Name}\n" +
                                       $"Order creation time : {Order.OrderDate}\n" +
                                       $"Customer : {Customer.FacebookName}\n" +
                                       $"Address : {Customer.Address}\n" +
                                       $"Phone number : {Customer.PhoneNumber}"
                    },
                };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error send message: {result}");
                }
                else
                {
                    Console.WriteLine("Sent message successfully");
                }
            }
            else
            {
                 var payload = new
                {
                    recipient = new { id = Customer.CustomerID },
                    message = new
                    {
                        text = "Your order has been successfully created\n" +
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == Order.ProductID).Name}\n" +
                                       $"Order creation time : {Order.OrderDate}\n" +
                                       $"Customer : {Customer.FacebookName}\n" +
                                       "Please provide your address and phone number for shipping!"
                    },
                };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error send message: {result}");
                }
                else
                {
                    Console.WriteLine("Sent message successfully");
                }
            }  
        }


    }
}
