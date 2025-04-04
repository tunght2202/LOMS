
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
        public async Task<int> CreateOrderByProductCodeAuto(string productCode)
        {
            //int ProductID = _context.Products.FirstOrDefault(p => p.ProductCode == productCode).ProductID;
            //string LiveStreamId = _context.LiveStreamsProducts.FirstOrDefault(p => p.ProductID == ProductID).LivestreamID;
            //string StreamURL = _context.LiveStreams.FirstOrDefault(l => l.LivestreamID == LiveStreamId).StreamURL;

            //string apiUrl = $"https://localhost:7112/api/Comment/get-comments-productcode?liveStreamURL={StreamURL}&ProductCode={productCode}";

            // Gọi API lấy danh sách comment theo productCode
            //var response = await _httpClient.GetAsync(apiUrl);
            //if (!response.IsSuccessStatusCode)
            //{
            //    throw new Exception($"Lỗi khi gọi API: {response.ReasonPhrase}");
            //}

            //var jsonResponse = await response.Content.ReadAsStringAsync();
            //var comments = JsonSerializer.Deserialize<List<Comment>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //if (comments == null || comments.Count == 0)
            //{
            //    return 0;
            //}

            // Sắp xếp comment theo thời gian tạo
            //comments = comments.OrderBy(c => c.CommentTime).ToList();

            int orderCount = 0;
            //foreach (var comment in comments)
            //{
            //    _context.Orders.Add(new Order
            //    {

            //    });
            //    orderCount++;

            //}

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            return orderCount; // Trả về số bản ghi đã thêm
        }

        public async Task<int> CreateOrderDetail(OrderDetailAddModel orderModel)
        {
            // Kiểm tra số lượng sản phẩm hợp lệ
            if (orderModel.quanity <= 0)
            {
                throw new Exception("Số lượng sản phẩm phải lớn hơn 0.");
            }

            // Kiểm tra sản phẩm có tồn tại không
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == orderModel.productId);
            if (product == null)
            {
                throw new Exception($"Sản phẩm với ID {orderModel.productId} không tồn tại.");
            }

            // Tính tổng giá tiền
            var totalPrice = product.Price * orderModel.quanity;

            // Tạo chi tiết đơn hàng
            var orderDetail = new OrderDetail
            {
                ProductID = orderModel.productId,
                Quantity = orderModel.quanity,
                Price = totalPrice
            };

            // Kiểm tra xem đã có đơn hàng cho khách hàng này chưa
            var order = await _context.Orders
                .Include(o => o.OrderDetails) // Load danh sách OrderDetails nếu có
                .FirstOrDefaultAsync(x => x.LiveStreamCustomerID == orderModel.livestreamCustomerId);

            if (order == null)
            {
                // Nếu chưa có, tạo mới đơn hàng
                order = new Order
                {
                    LiveStreamCustomerID = orderModel.livestreamCustomerId,
                    OrderDate = DateTime.UtcNow,
            //        Status = "Pending",
                    OrderDetails = new List<OrderDetail>() // Khởi tạo danh sách tránh lỗi null
                };

                order.OrderDetails.Add(orderDetail);
                await _context.Orders.AddAsync(order); // Thêm mới vào DB
            }
            else
            {
                // Nếu đơn hàng đã tồn tại, thêm chi tiết đơn hàng vào
                order.OrderDetails.Add(orderDetail);
            }

            // Lưu thay đổi vào DB
            return await _context.SaveChangesAsync();
        }


        // Thanh Tùng
        // Get order by livestream cuatomer id
        public async Task<GetOrderDetailByOrderModel> GetOrderByLivestreamCustomerId(int livestreamCustomerId)
        {
            var order = await _context.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.LiveStreamCustomerID == livestreamCustomerId);
            if (order == null)
            {
                throw new Exception($"order have livestreamcustomerId is:  {livestreamCustomerId} not exit");
            }

            var result = new GetOrderDetailByOrderModel()
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
             //   Status = order.Status,
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailModel
                {
                    OrderDetailId = od.OrderDetailID,
                    ProductID = od.ProductID,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };

            return result;
        }

        // Thanh Tùng
        // Get order by livestream id
        public async Task<IEnumerable<OrderModel>> GetOrderByLivestreamId(string livestreamId)
        {
            var orders = await _context.Orders
                                        .Where(o => o.LiveStreamCustomer.LivestreamID.Equals(livestreamId)) // Lọc theo LiveStreamID
                                        .Select(o => new OrderModel()
                                        {
                                            OrderID = o.OrderID,
                                            OrderDate = o.OrderDate,
                                            Status = o.Status,
                                            LiveStreamCustomerID = o.LiveStreamCustomerID
                                        })
                                        .ToListAsync();
            if (orders.Any())
            {
                return orders;
            }
            throw new Exception("not exit order");
        }

        // Thanh Tùng
        // Update quantity in order detail 
        public async Task<int> UpdateOrderDetail(int orderDetailId, int quantity)
        {
            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailID == orderDetailId);
            if (orderDetail == null)
            {
                throw new Exception("Chi tiết đơn hàng không tồn tại");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == orderDetail.ProductID);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại");
            }

            orderDetail.Quantity = quantity;
            orderDetail.Price = product.Price * quantity;

            return await _context.SaveChangesAsync();
        }

        // Thanh Tùng
        // Delete order detail by order detail id
        public async Task<int> DeleteOrderDetail(int orderDetailId)
        {
            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailID == orderDetailId);
            if (orderDetail == null)
            {
                throw new Exception("Chi tiết đơn hàng không tồn tại");
            }

            _context.OrderDetails.Remove(orderDetail);
            return await _context.SaveChangesAsync();
        }

        // Thanh Tùng
        // Get order by user id
        public async Task<IEnumerable<OrderModel>> GetOrderByUserId(string userId)
        {
            var orders = await _context.Orders
                            .Where(o => o.LiveStreamCustomer.LiveStream.UserID.Equals(userId)) // Lọc theo LiveStreamID
                            .Select(o => new OrderModel()
                            {
                                OrderID = o.OrderID,
                                OrderDate = o.OrderDate,
                                Status = o.Status,
                                LiveStreamCustomerID = o.LiveStreamCustomerID
                            })
                            .ToListAsync();
            if (orders.Any())
            {
                return orders;
            }
            throw new Exception("not exit order");
        }

        // Thanh Tùng
        // Get order by status and livestreamid
        public async Task<IEnumerable<OrderModel>> GetOrdersByStatusByLivestreamId(string livestreamId, string status)
        {
            var getOrder = await GetOrderByLivestreamId(livestreamId);
            var orders =  getOrder.Where(x => x.Status.Equals(status)).ToList();
            if (orders.Any())
            {
                return orders;
            }
            throw new Exception("not exit order");

        }

        // Thanh Tùng
        // Get order by status and user id
        public async Task<IEnumerable<OrderModel>> GetOrdersByStatusByUserId(string userId, string status)
        {
            var getOrder = await GetOrderByUserId(userId);
            var orders = getOrder.Where(x => x.Status.Equals(status)).ToList();
            if (orders.Any())
            {
                return orders;
            }
            throw new Exception("not exit order");
        }

        // Thanh Tùng
        // Get order by id
        public async Task<OrderModel> GetOrdersById(int orderId)
        {
            var orders = await _context.Orders.FirstOrDefaultAsync(x => x.OrderID == orderId);
            if (orders == null )
            {
                throw new Exception("not exit order");

            }

            var result = new OrderModel()
            {
                OrderID = orders.OrderID,
                OrderDate = orders.OrderDate,
                Status = orders.Status,
                LiveStreamCustomerID = orders.LiveStreamCustomerID
            };
            return result;

        }

        // Thanh Tùng
        // Update order status by order id
        public async Task<bool> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderID == orderId);
            if (order == null)
            {
                throw new Exception("not exit order");

            }

            order.Status = newStatus;
            var result =  await _context.SaveChangesAsync();
            if(result <= 0)
            {
                return false;
            }
            return true;
        }
        // Thanh Tùng
        // Get all orders
        public async Task<IEnumerable<OrderModel>> GetOrders()
        {
            return await _context.Orders
                .Select(o => new OrderModel()
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    LiveStreamCustomerID = o.LiveStreamCustomerID
                })
                 .ToListAsync();

        }
    }
}
