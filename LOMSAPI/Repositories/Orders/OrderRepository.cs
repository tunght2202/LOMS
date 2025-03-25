
using LOMSAPI.Data.Entities;
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
        public async Task<int> CreateOrderByProductCodeAuto(string productCode, string StreamURL)
        {
            string LiveStreamId = _context.LiveStreams.FirstOrDefault(s => s.StreamURL == StreamURL).LivestreamID;
            // Tìm sản phẩm theo ProductCode
            Product p = _context.Products.FirstOrDefault(s => s.ProductCode == productCode && s.LiveStreamID==LiveStreamId);
            if (p == null)
            {
                return 0;
            }

            int productId = p.ProductID;
            
            string apiUrl = $"https://localhost:7112/api/Comment/get-comments-productcode?liveStreamURL={StreamURL}&ProductCode={productCode}";

            // Gọi API lấy danh sách comment theo productCode
            var response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi khi gọi API: {response.ReasonPhrase}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var comments = JsonSerializer.Deserialize<List<Comment>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (comments == null || comments.Count == 0)
            {
                return 0;
            }

            // Sắp xếp comment theo thời gian tạo
            comments = comments.OrderBy(c => c.CommentTime).ToList();

            int orderCount = 0;
            foreach (var comment in comments)
            {
                _context.Orders.Add(new Order
                {
                    CustomerID = comment.CustomerID,
                    LivestreamID = comment.LiveStreamID,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    OrderDetails = new OrderDetail 
                    {
                        OrderID = OrderID,
                        Quantity = 1,
                    }
                });
                orderCount++;
                
            }

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            return orderCount; // Trả về số bản ghi đã thêm
        }

    }
}
