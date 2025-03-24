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

        private const string ACCESS_TOKEN = "EAAIYLfie53cBOZBIvkppFHMBLPrkRoScI2ZB4jWgBEcckrFq8ZA1qTaPsqwSYlbYZCFXBdnI0DZBZA1eAyBvs4abxfQvt0MdjMxYvTGmgBvFBdOK8ytZB2JOORDv8z0uY3Ay58JRz9upfNAlLtPEP86wLZAPUYcVHuqsB74S999W85ZABY5XtiBfuZBPA6VHXzGpu3lIDp21sr14jqBO4SEiP5JGQZD";
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
            //using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            //JsonElement root = doc.RootElement;

            List<Comment> comments = new List<Comment>();

            //if (root.TryGetProperty("data", out JsonElement dataElement))
            //{
            //    foreach (JsonElement item in dataElement.EnumerateArray())
            //    {
            //        var comment = new Comment
            //        {
            //            CommentID = item.GetProperty("id").GetString(),
            //            Content = item.GetProperty("message").GetString(),
            //            CommentTime = item.GetProperty("created_time").GetDateTime(),
            //            CustomerID = int.Parse(item.GetProperty("from").GetProperty("id").GetString())
            //        };
            //        comments.Add(comment);
            //    }
            //}

            return comments;
        }
        
    }
}

