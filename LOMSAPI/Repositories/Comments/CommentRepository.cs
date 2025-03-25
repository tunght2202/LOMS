using LOMSAPI.Data.Entities;
using System.Text.RegularExpressions;
using System;
using System.Text.Json;

namespace LOMSAPI.Repositories.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;

        private const string ACCESS_TOKEN = "EAAIYLfie53cBO9l9C8IZBmKczFFcZCVBv2hvbfjZAaw4kNctgQfiYHFh4PtDXKZBKAMxXehjHVusrdzgBgBE675GCx1HNq6fRazXvC6RefYgF2jm20kXZAH0uHCVYJaklZBFMZCixOMV9vZAakffBbp6zbR2kRX2EX1hQVW2ixohKLw9WnbPIrhzWY6487EfZBFDVQK0SLOZAYE4RLYnO9QPXdLVYZD";
        public CommentRepository(LOMSDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<List<Comment>> GetAllComments(string LiveStreamURL)
        {
            string liveStreamId = ExtractLiveStreamId(LiveStreamURL);
            if (string.IsNullOrEmpty(liveStreamId))
                throw new ArgumentException("Không thể lấy LiveStream ID từ URL");

            string apiUrl = $"https://graph.facebook.com/v22.0/{liveStreamId}/comments?fields=from%2Cmessage%2Ccreated_time&access_token={ACCESS_TOKEN}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Lỗi khi gọi API: {response.StatusCode}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return ParseComments(jsonResponse);
        }

        
        public async Task<List<Comment>> GetCommentsByProductCode(string LiveStreamURL, string ProductCode)
        {
            List<Comment> allComments = await GetAllComments(LiveStreamURL);
            return allComments.FindAll(c => c.Content.Contains(ProductCode, StringComparison.OrdinalIgnoreCase));
        }

        private string ExtractLiveStreamId(string url)
        {
            var match = Regex.Match(url, @"/videos/(\d+)");
            return match.Success ? match.Groups[1].Value : null;
        }

        private List<Comment> ParseComments(string jsonResponse)
        {
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;

            List<Comment> comments = new List<Comment>();

            if (root.TryGetProperty("data", out JsonElement dataElement))
            {
                foreach (JsonElement item in dataElement.EnumerateArray())
                {
                    var comment = new Comment
                    {
                        CommentID = item.GetProperty("id").ToString(),
                        Content = item.GetProperty("message").GetString().ToString(),
                        CommentTime = DateTime.Parse(item.GetProperty("created_time").ToString().Replace("+0000", "Z")),
                        CustomerID = int.Parse(item.GetProperty("from").GetProperty("id").GetString())
                    };
                    comments.Add(comment);
                }
            }

            return comments;
        }
        
    }
}

