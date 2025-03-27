
using LOMSAPI.Data.Entities;
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

    }
}
