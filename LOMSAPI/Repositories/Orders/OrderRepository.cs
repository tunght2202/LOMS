
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
    }
}
