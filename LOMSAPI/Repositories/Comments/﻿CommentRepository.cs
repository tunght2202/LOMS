//using LOMSAPI.Data.Entities;
//using System.Text.RegularExpressions;
//using System;
//using System.Text.Json;

//namespace LOMSAPI.Repositories.Comments
//{
//    public class CommentRepository : ICommentRepository
//    {
//        private readonly LOMSDbContext _context;
//        private readonly HttpClient _httpClient;

//        private const string ACCESS_TOKEN = "EAAIYLfie53cBO1gZBjycUJLWSy2CV9KeLZBZBQ6jtd5WfvS6QqRiYRGTWt7UXJmfUFY71T4HiDEoBGatv3eT9UZAcV9QwTwdn5DXCpkm93X8BlZCbTUctymxcWyZBefqKsXsPoVJlMjcOpAh7OeMZBUXjxTXeuunAOJHfpiZCnSBhfcU5e21sZC9Sj9cWO2nZB0D61YwLMdItPtvMXrdeDaC3ikMMZD";
//        public CommentRepository(LOMSDbContext context, HttpClient httpClient)
//        {
//            _context = context;
//            _httpClient = httpClient;
//        }


//        public async Task<List<Comment>> GetAllComments(string LiveStreamURL)
//        {
//            string liveStreamId = ExtractLiveStreamId(LiveStreamURL);
//            if (string.IsNullOrEmpty(liveStreamId))
//                throw new ArgumentException("Không thể lấy LiveStream ID từ URL");

//            string apiUrl = $"https://graph.facebook.com/v22.0/{liveStreamId}/comments?fields=from%2Cmessage%2Ccreated_time&access_token={ACCESS_TOKEN}";

//            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
//            if (!response.IsSuccessStatusCode)
//                throw new Exception($"Lỗi khi gọi API: {response.StatusCode}");

//            string jsonResponse = await response.Content.ReadAsStringAsync();
//            return ParseComments(jsonResponse, liveStreamId);
//        }


//        public async Task<List<Comment>> GetCommentsByProductCode(string LiveStreamURL, string ProductCode)
//        {
//            List<Comment> allComments = await GetAllComments(LiveStreamURL);
//            return allComments.FindAll(c => c.Content.Contains(ProductCode, StringComparison.OrdinalIgnoreCase));
//        }

//        private string ExtractLiveStreamId(string url)
//        {
//            var match = Regex.Match(url, @"/videos/(\d+)");
//            return match.Success ? match.Groups[1].Value : null;
//        }

//        private List<Comment> ParseComments(string jsonResponse, string LiveStreamId)
//        {
//            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
//            JsonElement root = doc.RootElement;

//            List<Comment> comments = new List<Comment>();

//            if (root.TryGetProperty("data", out JsonElement dataElement))
//            {
//                foreach (JsonElement item in dataElement.EnumerateArray())
//                {
//                    var comment = new Comment
//                    {
//                        CommentID = item.GetProperty("id").GetString(),
//                        Content = item.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "",
//                        CommentTime = DateTime.Parse(item.GetProperty("created_time").GetString().Replace("+0000", "Z")),
//                        CustomerID = item.TryGetProperty("from", out JsonElement fromElement)
//                                && fromElement.TryGetProperty("id", out JsonElement idElement)
//                                ? idElement.GetString()
//                                : "Unknown", // Gán giá trị mặc định nếu không có "from"
//                        LiveStreamID = LiveStreamId,
                        
//                    };
//                    comments.Add(comment);
//                }
//            }
            
            
//            return comments;
//        }

//    }
//}