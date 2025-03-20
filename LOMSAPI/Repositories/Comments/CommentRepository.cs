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

        private const string ACCESS_TOKEN = "EAAIYLfie53cBOZCtyhios9b8BSoytLgFoJjGWdeXAKE8hXLZBgB6qCoqACWsHyWfqbDpxrgfUzZBDNVdI0hgfHLwc8Fwa4RqZCsZA60nrsZAaFRfqqCHaZClF5prag16xmBhYSNyShTMsA1fb4zP1AkUnffwMqSli5BLvNsIn28gLDUYKm5544dwvhDD0njJIfaBAOFvSyP9fZAewv9LsR8bHm3OruEZD";
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
                        CommentID = item.GetProperty("id").GetInt32(),
                        Content = item.GetProperty("message").GetString(),
                        CommentTime = item.GetProperty("created_time").GetDateTime(),
                        CustomerID = item.GetProperty("from").GetProperty("id").GetInt32()
                    };
                    comments.Add(comment);
                }
            }

            return comments;
        }
        
    }
}

