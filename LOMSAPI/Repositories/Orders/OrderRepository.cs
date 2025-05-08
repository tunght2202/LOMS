using CloudinaryDotNet.Actions;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.ListProducts;
using LOMSAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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

        public async Task<IEnumerable<OrderCustomerModel>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                        .Include(o => o.Product)
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.Customer).ToListAsync();
            if (orders == null) return null;
            var orderModels = orders.Select(o => new OrderCustomerModel
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                Quantity = o.Quantity,
                CurrentPrice = o.CurrentPrice,
                Note = o.Note,
                StatusCheck = o.StatusCheck,
                TrackingNumber = o.TrackingNumber,
                Address = o.Comment.LiveStreamCustomer.Customer.Address,
                Email = o.Comment.LiveStreamCustomer.Customer.Email,
                FacebookName = o.Comment.LiveStreamCustomer.Customer.FacebookName,
                PhoneNumber = o.Comment.LiveStreamCustomer.Customer.PhoneNumber,
                CommentID = o.CommentID,
                ProductID = o.ProductID,
                Product = new ProductModel
                {
                    ProductID = o.Product.ProductID,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Stock = o.Product.Stock,
                    Description = o.Product.Description,
                    ImageURL = o.Product.ImageURL
                }
            }).ToList();
            return orderModels;
        }

        public async Task<IEnumerable<OrderCustomerModel>> GetAllOrdersByLiveStreamIdAsync(string liveStreamId)
        {
            var orders = await _context.Orders
                        .Include(o => o.Product)
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .Where(o => o.Comment.LiveStreamCustomer.LivestreamID.Equals(liveStreamId))
                .ToListAsync();
            if (orders == null) return null;
            var orderModels = orders.Select(o => new OrderCustomerModel
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                Quantity = o.Quantity,
                CurrentPrice = o.CurrentPrice,
                Note = o.Note,
                StatusCheck = o.StatusCheck,
                TrackingNumber = o.TrackingNumber,
                Address = o.Comment.LiveStreamCustomer.Customer.Address,
                Email = o.Comment.LiveStreamCustomer.Customer.Email,
                FacebookName = o.Comment.LiveStreamCustomer.Customer.FacebookName,
                PhoneNumber = o.Comment.LiveStreamCustomer.Customer.PhoneNumber,
                CommentID = o.CommentID,
                ProductID = o.ProductID,
                Product = new ProductModel
                {
                    ProductID = o.Product.ProductID,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Stock = o.Product.Stock,
                    Description = o.Product.Description,
                    ImageURL = o.Product.ImageURL
                }
            }).ToList();
            return orderModels;
        }

        public async Task<IEnumerable<OrderCustomerModel>> GetOrdersByCustomerIdAsync(string customerId)
        {
            var orders = await _context.Orders
                        .Include(o => o.Product)
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .Where(o => o.Comment.LiveStreamCustomer.CustomerID.Equals(customerId))
                .ToListAsync();
            if (orders == null) return null;
            var orderModels = orders.Select(o => new OrderCustomerModel
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                Quantity = o.Quantity,
                CurrentPrice = o.CurrentPrice,
                Note = o.Note,
                StatusCheck = o.StatusCheck,
                TrackingNumber = o.TrackingNumber,
                Address = o.Comment.LiveStreamCustomer.Customer.Address,
                Email = o.Comment.LiveStreamCustomer.Customer.Email,
                FacebookName = o.Comment.LiveStreamCustomer.Customer.FacebookName,
                PhoneNumber = o.Comment.LiveStreamCustomer.Customer.PhoneNumber,
                CommentID = o.CommentID,
                ProductID = o.ProductID,
                Product = new ProductModel
                {
                    ProductID = o.Product.ProductID,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Stock = o.Product.Stock,
                    Description = o.Product.Description,
                    ImageURL = o.Product.ImageURL
                }
            }).ToList();
            return orderModels;
        }

        public async Task<IEnumerable<OrderCustomerModel>> GetOrdersByLiveStreamCustomerIdAsync(int liveStreamCustomerID)
        {
            var orders = await _context.Orders
                        .Include(o => o.Product)
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .Where(o => o.Comment.LiveStreamCustomerID == liveStreamCustomerID)
                .ToListAsync();
            if (orders == null) return null;
            var orderModels = orders.Select(o => new OrderCustomerModel
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                Quantity = o.Quantity,
                CurrentPrice = o.CurrentPrice,
                Note = o.Note,
                StatusCheck = o.StatusCheck,
                TrackingNumber = o.TrackingNumber,
                Address = o.Comment.LiveStreamCustomer.Customer.Address,
                Email = o.Comment.LiveStreamCustomer.Customer.Email,
                FacebookName = o.Comment.LiveStreamCustomer.Customer.FacebookName,
                PhoneNumber = o.Comment.LiveStreamCustomer.Customer.PhoneNumber,
                CommentID = o.CommentID,
                ProductID = o.ProductID,
                Product = new ProductModel
                {
                    ProductID = o.Product.ProductID,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Stock = o.Product.Stock,
                    Description = o.Product.Description,
                    ImageURL = o.Product.ImageURL
                }
            }).ToList();
            return orderModels;
        }
        public async Task<OrderCustomerModel?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                        .Include(o => o.Product)
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
            if (order == null) return null;
            var orderCustomer = new OrderCustomerModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                Quantity = order.Quantity,
                CurrentPrice = order.CurrentPrice,
                Note = order.Note,
                StatusCheck = order.StatusCheck,
                TrackingNumber = order.TrackingNumber,
                Address = order.Comment.LiveStreamCustomer.Customer.Address,
                Email = order.Comment.LiveStreamCustomer.Customer.Email,
                FacebookName = order.Comment.LiveStreamCustomer.Customer.FacebookName,
                PhoneNumber = order.Comment.LiveStreamCustomer.Customer.PhoneNumber,
                CommentID = order.CommentID,
                ProductID = order.ProductID,
                Product = new ProductModel
                {
                    ProductID = order.Product.ProductID,
                    Name = order.Product.Name,
                    Price = order.Product.Price,
                    Stock = order.Product.Stock,
                    Description = order.Product.Description,
                    ImageURL = order.Product.ImageURL
                }
            };

            return orderCustomer;

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
                if (commentId == null)
                {
                    throw new ArgumentNullException("Comment ID is null");
                }
                var commentorder = await _context.Comments
                    .Include(c => c.LiveStreamCustomer)
                    .ThenInclude(c => c.Customer)
                    .FirstOrDefaultAsync(c => c.CommentID == commentId);
                string text = string.Empty;
                if ((commentorder.LiveStreamCustomer.Customer.Address == null)
                    || (commentorder.LiveStreamCustomer.Customer.PhoneNumber == null))
                {
                    text = "Your order from\n" +
                                       $"Comment : {commentorder.Content}\n" +
                                       "has been successfully created.\n" +
                                       "Please provide your address and phone number for shipping! \n"+
                                       "Click to link :  "  + 
                                       $"https://4e00-118-70-211-238.ngrok-free.app/Customers/Update?id={commentorder.LiveStreamCustomer.CustomerID}";
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
                //_print.PrintCustomerLabel("COM5", inforprint);
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
            existing.StatusCheck = orderModel.StatusCheck;
            existing.TrackingNumber = orderModel.TrackingNumber;
            existing.Note = orderModel.Note;
            existing.CurrentPrice = orderModel.CurrentPrice;
            existing.ProductID = orderModel.ProductID;
            existing.CommentID = orderModel.CommentID;

            return await _context.SaveChangesAsync();
        }


        public async Task<int> UpdateOrderAsync2(OrderModelRequest orderModel)
        {
            var existing = await _context.Orders.FindAsync(orderModel.OrderID);
            if (existing == null) return 0;
            existing.TrackingNumber = orderModel.TrackingNumber;
            existing.Note = orderModel.Note;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStatusOrderAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders
                        .Include(o => o.Product)
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.LiveStream)
                        .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null) return 0;
            if (newStatus < order.Status)
            {
                throw new Exception("Can't change status");
            }
            if ((order.Status == OrderStatus.Pending) && order.Comment?.LiveStreamCustomer?.Customer == null)
                throw new Exception("Can't confirm order without customer information!");

            if (order.Status == OrderStatus.Canceled)
            {
                throw new Exception("Can't change status");
            }
            if (((order.StatusCheck == false) && (order.Comment.LiveStreamCustomer.LiveStream.PriceMax <= (order.CurrentPrice * order.Quantity))))
            {
                throw new Exception("Can't change status, you must call for customer !");
            }
            if ((newStatus.Equals(OrderStatus.Shipped)) && (string.IsNullOrEmpty(order.TrackingNumber)))
            {
                throw new Exception("Can't change status, you must input tracking number !");
            }
            if ((string.IsNullOrEmpty(order.Comment.LiveStreamCustomer.Customer.Address)) || (string.IsNullOrEmpty(order.Comment.LiveStreamCustomer.Customer.PhoneNumber)))
            {
                throw new Exception("Can't change status, you must input address and phone number !");
                return 0;
            }

            if ((string.IsNullOrEmpty(order.Comment.LiveStreamCustomer.Customer.Address)) || (string.IsNullOrEmpty(order.Comment.LiveStreamCustomer.Customer.PhoneNumber)))
            {
                throw new Exception("Can't change status, you must input address and phone number !");
                return 0;
            }
            var getProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductID == order.ProductID);
            if (getProduct == null)
            {
                throw new Exception($"This id {order.ProductID} invite ");
            }
            if (newStatus == OrderStatus.Canceled || newStatus == OrderStatus.Returned)
            {
                getProduct.Stock += order.Quantity;
            }

            order.Status = (OrderStatus)newStatus;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStatusCheckOrderAsync(int orderId, bool newStatusCheck)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return 0;
            order.StatusCheck = newStatusCheck;
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderCustomerModel>> GetAllOrdersByUserIdAsync(string userID)
        {
            var orders = await _context.Orders
                        .Include(o => o.Product)
                        .Include(o => o.Comment)
                        .Include(o => o.Comment.LiveStreamCustomer)
                        .Include(o => o.Comment.LiveStreamCustomer.Customer)
                        .Include(o => o.Comment.LiveStreamCustomer.LiveStream)
                        .Where(o => o.Comment.LiveStreamCustomer.LiveStream.UserID.Equals(userID))
                        .ToListAsync();
            var orderModels = orders.Select(o => new OrderCustomerModel
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                Quantity = o.Quantity,
                CurrentPrice = o.CurrentPrice,
                Note = o.Note,
                StatusCheck = o.StatusCheck,
                TrackingNumber = o.TrackingNumber,
                Address = o.Comment.LiveStreamCustomer.Customer.Address,
                Email = o.Comment.LiveStreamCustomer.Customer.Email,
                FacebookName = o.Comment.LiveStreamCustomer.Customer.FacebookName,
                PhoneNumber = o.Comment.LiveStreamCustomer.Customer.PhoneNumber,
                CommentID = o.CommentID,
                ProductID = o.ProductID,
                Product = new ProductModel
                {
                    ProductID = o.Product.ProductID,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Stock = o.Product.Stock,
                    Description = o.Product.Description,
                    ImageURL = o.Product.ImageURL
                }

            }).ToList();
            return orderModels;
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
                var regex = new Regex(@"^(?<code>[a-zA-Z\d]+)(?:\s+(?<qty>\d+))?$", RegexOptions.IgnoreCase);

                foreach (var comment in comments.OrderBy(c => c.CommentTime))
                {
                    if ((order.Count <= 0) || (!order.Any(x => x.CommentID.Equals(comment.CommentID))))
                    {


                        var match = regex.Match(comment.Content);
                        if (match.Success)
                        {
                            string code = match.Groups["code"].Value.ToLower();
                            int quantity = 1;
                            if (match.Groups["qty"].Success)
                            {
                                quantity = int.Parse(match.Groups["qty"].Value);
                                if (quantity <= 0)
                                {
                                    continue;
                                }
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

                                var sanpham = $"{product.Name} X {quantity}";
                                var tonggiaDecimal = product.Price * quantity;
                                var tonggia = (long)tonggiaDecimal;
                                var stock = product.Stock;
                                string formatted = tonggia.ToString("N0", new System.Globalization.CultureInfo("vi-VN")) + " VND";
                                var priceMax = liveStream.PriceMax;
                                var newOrder = new Order
                                {
                                    ProductID = productId,
                                    Quantity = quantity,
                                    CommentID = comment.CommentID,
                                    OrderDate = comment.CommentTime,
                                    CurrentPrice = product.Price

                                };
                                var customer = await _context.Customers
                                    .FirstOrDefaultAsync(c => c.CustomerID.Equals(comment.LiveStreamCustomer.CustomerID));

                                var oldOrder = _context.Orders
                                        .Include(o => o.Product)
                                        .Where(o => o.Comment.LiveStreamCustomerID == comment.LiveStreamCustomerID)
                                        .ToList();
                                decimal total = 0;
                                if (oldOrder != null)
                                {
                                    total = oldOrder.Sum(order => order.Product.Price * order.Quantity);

                                }
                                if (total == null)
                                {
                                    total = 0;
                                }
                                var text = string.Empty;
                                if (customer.Address != null || customer.PhoneNumber != null)
                                {
                                    if ((tonggia + total) >= priceMax)
                                    {

                                        ChangePendingStatus(comment.LiveStreamCustomerID);
                                        newOrder.Status = OrderStatus.Pending;
                                    }
                                    else
                                    {
                                        newOrder.Status = OrderStatus.Confirmed;
                                    }

                                    text = "Your order has been successfully created\n" +
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == productId).Name} \n" +
                                       $"Quantity : {quantity} \n" +
                                       $"Total price : {formatted} \n" +
                                       $"Order creation time : {comment.CommentTime}\n" +
                                       $"Customer : {customer.FacebookName}";

                                }
                                else
                                {
                                    text = "Your order has been successfully created\n" +
                                       $"Product : {_context.Products.FirstOrDefault(s => s.ProductID == productId).Name}\n" +
                                       $"Quantity : {quantity} \n" +
                                       $"Total price : {formatted} \n" +
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
                                    SoDienThoai = customer.PhoneNumber,
                                    Stock = stock
                                };
                                try
                                {
                                    _print.PrintCustomerLabel("COM5", printInfo);

                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }

                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        private void ChangePendingStatus(int LiveStreamCustomerID)
        {
            var totalOder = _context.Orders
                .Include(o => o.Comment)
                .Where(o => o.Comment.LiveStreamCustomerID == LiveStreamCustomerID)
                .ToList();
            foreach (var order in totalOder)
            {

                order.Status = OrderStatus.Pending;
            }
            _context.UpdateRange(totalOder);
            _context.SaveChanges();
        }

        private long TotalPrice(int liveStreamCustomerID, int productId)
        {
            var order = _context.Orders
                .Include(o => o.Product)
                .Where(o => o.Comment.LiveStreamCustomerID == liveStreamCustomerID
                && o.ProductID == productId).ToList();
            var total = order.Sum(order => order.Product.Price * order.Quantity);
            return (long)total;
        }

        private async Task<bool> SendMessage2Async(string customerId, string TokenFacbook, string messageSend)
        {
            var url = $"https://graph.facebook.com/v22.0/me/messages?access_token={TokenFacbook}";


            var payload = new
            {
                recipient = new { id = customerId },
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

        public async Task<OrderByProductCodeModel> OrderByProductCodeModel(int LiveStreamCustomerID, int productID)
        {
            var totalOrder = await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.Comment.LiveStreamCustomerID == LiveStreamCustomerID && o.ProductID == productID)
                .ToListAsync();

            if (!totalOrder.Any())
            {
                return null;
            }

            var totalPrice = totalOrder.Sum(order => order.Product.Price * order.Quantity);

            var orderByProductCodeModel = new OrderByProductCodeModel
            {
                Status = totalOrder.First().Status.ToString(),
                Quantity = totalOrder.Sum(order => order.Quantity),
                TotalPrice = totalOrder.Sum(order => order.CurrentPrice * order.Quantity),
                StatusCheck = totalOrder.First().StatusCheck,
                TrackingNumber = totalOrder.First().TrackingNumber,
                Note = totalOrder.First().Note,
                ProductID = productID,
                CurrentPrice = totalOrder.First().CurrentPrice,
                Description = totalOrder.First().Product.Description,
                Name = totalOrder.First().Product.Name,
                ImageURL = totalOrder.First().Product.ImageURL,

                LiveStreamCustomerID = LiveStreamCustomerID
            };

            return orderByProductCodeModel;
        }

        public async Task<OrderByLiveStreamCustoemrModel> GetOrderByLiveStreamCustoemrModel(int LiveStreamCustomerID)
        {
            var order = await _context.Orders
                .Include(o => o.Comment)
                .Include(o => o.Comment.LiveStreamCustomer.LiveStream)
                .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .Where(o => o.Comment.LiveStreamCustomerID == LiveStreamCustomerID).ToListAsync();
            var orderByLiveStreamCustoemrModel = new OrderByLiveStreamCustoemrModel();
            orderByLiveStreamCustoemrModel.LiveStreamTital = order.FirstOrDefault().Comment.LiveStreamCustomer.LiveStream.StreamTitle;
            orderByLiveStreamCustoemrModel.CustoemrName = order.FirstOrDefault().Comment.LiveStreamCustomer.Customer.FacebookName;
            orderByLiveStreamCustoemrModel.LiveStreamCustoemrID = LiveStreamCustomerID;
            orderByLiveStreamCustoemrModel.OrderStatus = order.FirstOrDefault().Status.ToString();
            orderByLiveStreamCustoemrModel.PriceMax = order.FirstOrDefault().Comment.LiveStreamCustomer.LiveStream.PriceMax;
            orderByLiveStreamCustoemrModel.TrackingNumber = order.FirstOrDefault().TrackingNumber;
            orderByLiveStreamCustoemrModel.Note = order.FirstOrDefault().Note;
            orderByLiveStreamCustoemrModel.Address = order.FirstOrDefault().Comment.LiveStreamCustomer.Customer.Address;
            orderByLiveStreamCustoemrModel.PhoneNumber = order.FirstOrDefault().Comment.LiveStreamCustomer.Customer.PhoneNumber;
            orderByLiveStreamCustoemrModel.Email = order.FirstOrDefault().Comment.LiveStreamCustomer.Customer.Email;
            orderByLiveStreamCustoemrModel.FacebookName = order.FirstOrDefault().Comment.LiveStreamCustomer.Customer.FacebookName;
            orderByLiveStreamCustoemrModel.TotalPrice = order.Sum(order => (long)order.CurrentPrice * order.Quantity);
            orderByLiveStreamCustoemrModel.ImageUrl = order.FirstOrDefault().Comment.LiveStreamCustomer.Customer.ImageURL;
            orderByLiveStreamCustoemrModel.StatusCheck = order.FirstOrDefault().StatusCheck;
            var listProductID = order.Select(o => o.ProductID).Distinct().ToList();
            var listOrderByProductCodeModel = new List<OrderByProductCodeModel>();
            foreach (var productID in listProductID)
            {
                var orderByProductCodeModel = new OrderByProductCodeModel();
                orderByProductCodeModel = OrderByProductCodeModel(LiveStreamCustomerID, productID).Result;
                if(orderByLiveStreamCustoemrModel == null)
                {
                    continue;
                }
                listOrderByProductCodeModel.Add(orderByProductCodeModel);
            }

            orderByLiveStreamCustoemrModel.orderByProductCodeModels = listOrderByProductCodeModel;

            orderByLiveStreamCustoemrModel.TotalOrder = listProductID.Count();
            return orderByLiveStreamCustoemrModel;

        }


        public async Task<IEnumerable<OrderByLiveStreamCustoemrModel>> GetListOrderByLiveStreamCustoemrModel(string userID)
        {
            var order = await _context.Orders
                .Include(o => o.Comment)
                .Include(o => o.Comment.LiveStreamCustomer.LiveStream)
                .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .Where(o => o.Comment.LiveStreamCustomer.LiveStream.UserID == userID).ToListAsync();
            var listLiveStreamCustomerID = order.Select(o => o.Comment.LiveStreamCustomerID).Distinct().ToList();
            var listOrderByLiveStreamCustoemrModel = new List<OrderByLiveStreamCustoemrModel>();
            foreach (var LiveStreamCustomerID in listLiveStreamCustomerID)
            {
                var orderByLiveStreamCustoemrModel = new OrderByLiveStreamCustoemrModel();
                orderByLiveStreamCustoemrModel.LiveStreamTital = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.LiveStream.StreamTitle;
                orderByLiveStreamCustoemrModel.CustoemrName = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.FacebookName;
                orderByLiveStreamCustoemrModel.ImageUrl = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.ImageURL;
                orderByLiveStreamCustoemrModel.LiveStreamCustoemrID = LiveStreamCustomerID;
                orderByLiveStreamCustoemrModel.OrderStatus = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Status.ToString();
                orderByLiveStreamCustoemrModel.PriceMax = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.LiveStream.PriceMax;
                orderByLiveStreamCustoemrModel.TrackingNumber = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).TrackingNumber;
                orderByLiveStreamCustoemrModel.Note = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Note;
                orderByLiveStreamCustoemrModel.Address = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.Address;
                orderByLiveStreamCustoemrModel.PhoneNumber = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.PhoneNumber;
                orderByLiveStreamCustoemrModel.Email = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.Email;
                orderByLiveStreamCustoemrModel.FacebookName = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.FacebookName;
                orderByLiveStreamCustoemrModel.StatusCheck = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).StatusCheck;
                orderByLiveStreamCustoemrModel.TotalPrice = order.Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Sum(order => (long)order.CurrentPrice * order.Quantity);

                var listProductID = order.Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Select(o => o.ProductID).Distinct().ToList();
                var listOrderByProductCodeModel = new List<OrderByProductCodeModel>();
                foreach (var productID in listProductID)
                {
                    var orderByProductCodeModel = await OrderByProductCodeModel(LiveStreamCustomerID, productID);
                    if (orderByProductCodeModel == null) continue;
                    listOrderByProductCodeModel.Add(orderByProductCodeModel);
                }

                orderByLiveStreamCustoemrModel.orderByProductCodeModels = listOrderByProductCodeModel;
                orderByLiveStreamCustoemrModel.TotalOrder = listProductID.Count();
                listOrderByLiveStreamCustoemrModel.Add(orderByLiveStreamCustoemrModel);
            }


            return listOrderByLiveStreamCustoemrModel;

        }
        public async Task<IEnumerable<OrderByLiveStreamCustoemrModel>> GetListOrderByLiveStreamCustoemrLiveStremModel(string liveStreamId)
        {
            var order = await _context.Orders
                .Include(o => o.Comment)
                .Include(o => o.Comment.LiveStreamCustomer.LiveStream)
                .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .Where(o => o.Comment.LiveStreamCustomer.LivestreamID == liveStreamId).ToListAsync();
            var listLiveStreamCustomerID = order.Select(o => o.Comment.LiveStreamCustomerID).Distinct().ToList();
            var listOrderByLiveStreamCustoemrModel = new List<OrderByLiveStreamCustoemrModel>();
            foreach (var LiveStreamCustomerID in listLiveStreamCustomerID)
            {
                var orderByLiveStreamCustoemrModel = new OrderByLiveStreamCustoemrModel();
                orderByLiveStreamCustoemrModel.LiveStreamTital = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.LiveStream.StreamTitle;
                orderByLiveStreamCustoemrModel.CustoemrName = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.FacebookName;
                orderByLiveStreamCustoemrModel.ImageUrl = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.ImageURL;
                orderByLiveStreamCustoemrModel.LiveStreamCustoemrID = LiveStreamCustomerID;
                orderByLiveStreamCustoemrModel.OrderStatus = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Status.ToString();
                orderByLiveStreamCustoemrModel.PriceMax = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.LiveStream.PriceMax;
                orderByLiveStreamCustoemrModel.TrackingNumber = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).TrackingNumber;
                orderByLiveStreamCustoemrModel.Note = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Note;
                orderByLiveStreamCustoemrModel.Address = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.Address;
                orderByLiveStreamCustoemrModel.PhoneNumber = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.PhoneNumber;
                orderByLiveStreamCustoemrModel.Email = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.Email;
                orderByLiveStreamCustoemrModel.FacebookName = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.FacebookName;
                orderByLiveStreamCustoemrModel.StatusCheck = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).StatusCheck;
                orderByLiveStreamCustoemrModel.TotalPrice = order.Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Sum(order => (long)order.CurrentPrice * order.Quantity);

                var listProductID = order.Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Select(o => o.ProductID).Distinct().ToList();
                var listOrderByProductCodeModel = new List<OrderByProductCodeModel>();
                foreach (var productID in listProductID)
                {
                    var orderByProductCodeModel = await OrderByProductCodeModel(LiveStreamCustomerID, productID);
                    if (orderByProductCodeModel == null) continue;
                    listOrderByProductCodeModel.Add(orderByProductCodeModel);
                }

                orderByLiveStreamCustoemrModel.orderByProductCodeModels = listOrderByProductCodeModel;
                orderByLiveStreamCustoemrModel.TotalOrder = listProductID.Count();
                listOrderByLiveStreamCustoemrModel.Add(orderByLiveStreamCustoemrModel);
            }


            return listOrderByLiveStreamCustoemrModel;

        }
        
        public async Task<IEnumerable<OrderByLiveStreamCustoemrModel>> GetListOrderByLiveStreamCustoemrByCustoemrModel(string customerId)
        {
            var order = await _context.Orders
                .Include(o => o.Comment)
                .Include(o => o.Comment.LiveStreamCustomer.LiveStream)
                .Include(o => o.Comment.LiveStreamCustomer.Customer)
                .Where(o => o.Comment.LiveStreamCustomer.CustomerID == customerId).ToListAsync();
            var listLiveStreamCustomerID = order.Select(o => o.Comment.LiveStreamCustomerID).Distinct().ToList();
            var listOrderByLiveStreamCustoemrModel = new List<OrderByLiveStreamCustoemrModel>();
            foreach (var LiveStreamCustomerID in listLiveStreamCustomerID)
            {
                var orderByLiveStreamCustoemrModel = new OrderByLiveStreamCustoemrModel();
                orderByLiveStreamCustoemrModel.LiveStreamTital = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.LiveStream.StreamTitle;
                orderByLiveStreamCustoemrModel.CustoemrName = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.FacebookName;
                orderByLiveStreamCustoemrModel.ImageUrl = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.ImageURL;
                orderByLiveStreamCustoemrModel.LiveStreamCustoemrID = LiveStreamCustomerID;
                orderByLiveStreamCustoemrModel.OrderStatus = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Status.ToString();
                orderByLiveStreamCustoemrModel.PriceMax = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.LiveStream.PriceMax;
                orderByLiveStreamCustoemrModel.TrackingNumber = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).TrackingNumber;
                orderByLiveStreamCustoemrModel.Note = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Note;
                orderByLiveStreamCustoemrModel.Address = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.Address;
                orderByLiveStreamCustoemrModel.PhoneNumber = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.PhoneNumber;
                orderByLiveStreamCustoemrModel.Email = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.Email;
                orderByLiveStreamCustoemrModel.FacebookName = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Comment.LiveStreamCustomer.Customer.FacebookName;
                orderByLiveStreamCustoemrModel.StatusCheck = order.FirstOrDefault(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).StatusCheck;
                orderByLiveStreamCustoemrModel.TotalPrice = order.Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Sum(order => (long)order.CurrentPrice * order.Quantity);

                var listProductID = order.Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == LiveStreamCustomerID).Select(o => o.ProductID).Distinct().ToList();
                var listOrderByProductCodeModel = new List<OrderByProductCodeModel>();
                foreach (var productID in listProductID)
                {
                    var orderByProductCodeModel = await OrderByProductCodeModel(LiveStreamCustomerID, productID);
                    if (orderByProductCodeModel == null) continue;
                    listOrderByProductCodeModel.Add(orderByProductCodeModel);
                }

                orderByLiveStreamCustoemrModel.orderByProductCodeModels = listOrderByProductCodeModel;
                orderByLiveStreamCustoemrModel.TotalOrder = listProductID.Count();
                listOrderByLiveStreamCustoemrModel.Add(orderByLiveStreamCustoemrModel);
            }


            return listOrderByLiveStreamCustoemrModel;

        }

        public async Task<int> UpdateStatusOrderByLiveStreamCustoemrAsync(int livestreamCustomerID, OrderStatus newStatus)
        {
            var orderByLiveStreamCustomer = await GetOrderByLiveStreamCustoemrModel(livestreamCustomerID);
            if (orderByLiveStreamCustomer == null) return 0;


            if (newStatus < (OrderStatus)Enum.Parse(typeof(OrderStatus), orderByLiveStreamCustomer.OrderStatus, true))
            {
                throw new Exception("Can't change status");
            }

            if ((OrderStatus)Enum.Parse(typeof(OrderStatus), orderByLiveStreamCustomer.OrderStatus, true) == OrderStatus.Canceled)
            {
                throw new Exception("Can't change status");
            }
            if (((orderByLiveStreamCustomer.PriceMax <= orderByLiveStreamCustomer.TotalPrice) && (orderByLiveStreamCustomer.StatusCheck == false)))
            {
                throw new Exception("Can't change status, you must call for customer !");
            }
            if ((newStatus.Equals(OrderStatus.Shipped)) && (string.IsNullOrEmpty(orderByLiveStreamCustomer.TrackingNumber)))
            {
                throw new Exception("Can't change status, you must input tracking number !");
            }
            if ((string.IsNullOrEmpty(orderByLiveStreamCustomer.Address)) || (string.IsNullOrEmpty(orderByLiveStreamCustomer.PhoneNumber)))
            {
                throw new Exception("Can't change status, you must input address and phone number !");
            }

               return await UpdateStatus(livestreamCustomerID, newStatus);

        }

        public async Task<int> UpdateStatusCalldAsync(int livestreamCustomerID, bool statusCheck)
        {
            var orderByLiveStreamCustomer = await _context.Orders
                .Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == livestreamCustomerID)
                .ToListAsync();
            if (orderByLiveStreamCustomer == null) return 0;
            foreach (var order in orderByLiveStreamCustomer)
            {
                order.StatusCheck = true;
            }
            _context.UpdateRange(orderByLiveStreamCustomer);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStatusTrackingNumberdAsync(int livestreamCustomerID, string trackingNumber, string note)
        {
        var orderByLiveStreamCustomer = await _context.Orders
                .Where(o => o.Comment.LiveStreamCustomer.LiveStreamCustomerId == livestreamCustomerID)
                .ToListAsync();
            if (orderByLiveStreamCustomer == null) return 0;
            foreach (var order in orderByLiveStreamCustomer)
            {
                order.TrackingNumber = trackingNumber;
                order.Note = note;
            }
            _context.UpdateRange(orderByLiveStreamCustomer);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStatus(int liveStreamCustomerId, OrderStatus newStatus)
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.Comment.LiveStreamCustomerID == liveStreamCustomerId)
                .ToListAsync();
            foreach (var order in orders)
            {
                var product = _context.Products.Find(order.ProductID);
                if (product != null && ((newStatus == OrderStatus.Canceled) || (newStatus == OrderStatus.Returned)))
                {
                    product.Stock += order.Quantity;
                }
                order.Status = newStatus;
            }
           return await _context.SaveChangesAsync();

        }
    }
}
