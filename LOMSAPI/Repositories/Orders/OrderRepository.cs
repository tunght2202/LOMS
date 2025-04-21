
using Azure;
using Azure.Core;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.ListProducts;
using LOMSAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using LOMSAPI.Services;
namespace LOMSAPI.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IListProductRepository _listProductRepository;
        private readonly IConfiguration _configuration;
        private readonly IPrintService _print;
        public OrderRepository(LOMSDbContext context, HttpClient httpClient
            , IListProductRepository listProductRepository, IConfiguration configuration
            , IPrintService print)
        {
            _context = context;
            _httpClient = httpClient;
            _listProductRepository = listProductRepository;
            _configuration = configuration;

            _print = print;
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
        public async Task<bool> AddOrderAsync(string commentId, string TokenFacbook)
        {
            try
            {
                if(commentId == null)
                {
                    throw new ArgumentNullException("Comment ID is null");
                }
                var commentorder = await _context.Comments
                    .Include(c => c.LiveStreamCustomer)
                    .ThenInclude(c => c.Customer)
                    .FirstOrDefaultAsync(c => c.CommentID == commentId);
                string text = string.Empty;
                if ((commentorder.LiveStreamCustomer.Customer.Address== null) 
                    || (commentorder.LiveStreamCustomer.Customer.PhoneNumber == null))
                {
                    text = "Your order from\n" +
                                       $"Comment : {commentorder.Content}\n" +
                                       "has been successfully created.\n" +
                                       "Please provide your address and phone number for shipping!";
                }
                else
                {
                    text = "Your order from\n" +
                                       $"Comment : {commentorder.Content}\n" +
                                       "has been successfully created.\n" +
                                       "Thank you so much for your purchase!";
                }
                 var resultSendMessage = await SendMessage2Async(commentorder.LiveStreamCustomer.CustomerID, TokenFacbook, text);
                if (!resultSendMessage)
                {
                    return false;
                }
                var inforprint = new PrintInfo()
                {
                    NoiDungCommment = commentorder.Content,
                    TenKhach = commentorder.LiveStreamCustomer.Customer.FacebookName,
                    ThoiGian = commentorder.CommentTime,
                    DiaChi = commentorder.LiveStreamCustomer.Customer.Address,
                    SoDienThoai = commentorder.LiveStreamCustomer.Customer.PhoneNumber
                };
                _print.PrintCustomerLabel("com5", inforprint);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }

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
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.LiveStream)
                        .Where(o => o.Comment.LiveStreamCustomer.LiveStream.UserID.Equals(userID))
                        .ToListAsync();
            return orders.Select(o => MapToModel(o));
        }
        public async Task<int> CreateOrderFromComments(string liveStreamId, string TokenFacbook)
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
                    .Include(c => c.LiveStreamCustomer)
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

                                    var sanpham = $"{product.Name} X{quantity}";
                                var tonggiaDecimal  = product.Price * quantity;
                                var tonggia = (int)tonggiaDecimal;
                                string formatted = tonggia.ToString("N0", new System.Globalization.CultureInfo("vi-VN")) + " VND";
                                var newOrder = new Order
                                {
                                    ProductID = productId,
                                    Quantity = quantity,
                                    CommentID = comment.CommentID,
                                    OrderDate = comment.CommentTime
                                };
                                var customer = await _context.Customers
                                    .FirstOrDefaultAsync(c => c.CustomerID.Equals(comment.LiveStreamCustomer.CustomerID));

                                var text = string.Empty;
                                if (customer.Address != null || customer.PhoneNumber != null)
                                {
                                    newOrder.Status = OrderStatus.Confirmed;

                                    text = "Your order has been successfully created\n" +
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == productId).Name}\n" +
                                       $"Order creation time : {comment.CommentTime}\n" +
                                       $"Customer : {customer.FacebookName}";

                                }
                                else
                                {
                                    text = "Your order has been successfully created\n" +
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == productId).Name}\n" +
                                       $"Order creation time : {comment.CommentTime}\n" +
                                       $"Customer : {customer.FacebookName}\n" +
                                       "Please provide your address and phone number for shipping!";
                                }

                                var resultSendMessage = await SendMessage2Async(customer.CustomerID, TokenFacbook, text);
                                if (!resultSendMessage)
                                {
                                    continue;
                                }
                                await _context.Orders.AddAsync(newOrder);
                                    await _context.SaveChangesAsync();
                                    result++;
                            //    await SendMessageAsync(customer.CustomerID, TokenFacbook, newOrder.OrderID);

                                var ordernew = await _context.Orders
                                    .FirstOrDefaultAsync(Orders => Orders.CommentID.Equals(comment.CommentID));
                                var printInfo = new PrintInfo()
                                {
                                    MaSo = ordernew.OrderID.ToString(),
                                    TenKhach = customer.FacebookName,
                                    ThoiGian = comment.CommentTime,
                                    NoiDungCommment = comment.Content,
                                    SanPham = sanpham,
                                    TongGia = formatted,
                                    DiaChi = customer.Address,
                                    SoDienThoai = customer.PhoneNumber
                                };
                                _print.PrintCustomerLabel("COM5", printInfo);

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

        private async Task<bool> SendMessage2Async(string customerId, string TokenFacbook, string messageSend)
        {
            var url = $"https://graph.facebook.com/v22.0/me/messages?access_token={TokenFacbook}";


                var payload = new
                {
                    recipient = new { id = customerId},
                    message = new
                    {
                        text = messageSend
                    },
                };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            
        }

        private async Task SendMessageAsync(string customerId, string TokenFacbook, int OrderId)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(s =>s.OrderID == OrderId);
            var Customer = await _context.Customers.FirstOrDefaultAsync(s => s.CustomerID == customerId);
            bool IsNewCustomer = Customer.Address == null || Customer.PhoneNumber == null;
            var url = $"https://graph.facebook.com/v22.0/me/messages?access_token={TokenFacbook}";
            
            if (!IsNewCustomer)
            {
                var payload = new
                {
                    recipient = new { id = customerId },
                    message = new
                    {
                        text = "Your order has been successfully created\n" +
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == order.ProductID).Name}\n" +
                                       $"Order creation time : {order.OrderDate}\n" +
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
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == order.ProductID).Name}\n" +
                                       $"Order creation time : {order.OrderDate}\n" +
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

        public Task<bool> PrinTest()
        {
            var printInfo = new PrintInfo()
            {
                MaSo = "123",
                TenKhach = "LOMS APP",
                ThoiGian = DateTime.Now,
                NoiDungCommment = "Thank you",
                SanPham = "Test product X1",
                TongGia = "10.0000 VND",
                DiaChi = "FPT University",
                SoDienThoai = "0123456789"
            };
            _print.PrintCustomerLabel("COM5", printInfo);
            return Task.FromResult(true);
        }
    }
}
