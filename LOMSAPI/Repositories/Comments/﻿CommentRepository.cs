using LOMSAPI.Data.Entities;
using System.Text.RegularExpressions;
using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LOMSAPI.Repositories.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        private const string ACCESS_TOKEN = "EAAIYLfie53cBO7ZAfbJSewdnvOhErzqgSiVJiMZCKBpTqqZANbZA2whTgehQCzeZC7eeG4Gmiv5LsvnzPiZAg2qjrDUy3JYFFmCKCl8gCCquP9sB7njId1CrB7bOJXg4ddWPWbqsJG92ZC1EQonWMBYg3DLL4oByoFcGvOOEeH0TvrnwxNbwvVYIHtoZBDa4lP8lPSGyH7SuZB7rMGTk0ZCSG4gJcZD";
        public CommentRepository(LOMSDbContext context, HttpClient httpClient, IDistributedCache cache)
        {
            _context = context;
            _httpClient = httpClient;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        public async Task<List<Comment>> GetAllComments(string LiveStreamId)
        {
            //string liveStreamId = ExtractLiveStreamId(LiveStreamURL);
            if (string.IsNullOrEmpty(LiveStreamId))
                throw new ArgumentException("Không thể lấy LiveStream ID từ URL");
            await _cache.SetStringAsync("Livestream_Id", LiveStreamId, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            });

            string apiUrl = $"https://graph.facebook.com/v22.0/{LiveStreamId}/comments?fields=from%2Cmessage%2Ccreated_time&access_token={ACCESS_TOKEN}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Lỗi khi gọi API: {response.StatusCode}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return await ParseCommentsAsync(jsonResponse, LiveStreamId);
        }


        public async Task<List<Comment>> GetCommentsByProductCode(string ProductCode)
        {
            var LiveStreamId = await _cache.GetStringAsync("Livestream_Id");
            List<Comment> allComments = await GetAllComments(LiveStreamId);
            var filteredComments = allComments
                            .Where(c => c.Content.Contains(ProductCode, StringComparison.OrdinalIgnoreCase))
                            .OrderBy(c => c.CommentTime)
                            .ToList();

            return filteredComments;
        }

        //private string ExtractLiveStreamId(string url)
        //{
        //    var match = Regex.Match(url, @"/videos/(\d+)");
        //    return match.Success ? match.Groups[1].Value : null;
        //}

        private async Task<List<Comment>> ParseCommentsAsync(string jsonResponse, string liveStreamId)
        {
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;

            if (root.TryGetProperty("data", out JsonElement dataElement))
            {
                foreach (JsonElement item in dataElement.EnumerateArray())
                {
                    string? commentID = item.GetProperty("id").GetString();
                    string? content = item.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "";
                    DateTime commentTime = DateTime.Parse(item.GetProperty("created_time").GetString().Replace("+0000", "Z"));

                    string? customerID = item.TryGetProperty("from", out JsonElement fromElement)
                                && fromElement.TryGetProperty("id", out JsonElement idElement)
                                ? idElement.GetString()
                                : null;

                    string? customerName = item.TryGetProperty("from", out JsonElement from1Element)
                                && from1Element.TryGetProperty("name", out JsonElement nameElement)
                                ? nameElement.GetString()
                                : null;

                    // Kiểm tra nếu có dữ liệu bị null
                    if (string.IsNullOrEmpty(commentID) || string.IsNullOrEmpty(customerID) || string.IsNullOrEmpty(content))
                    {
                        Console.WriteLine("Dữ liệu comment bị thiếu! Bỏ qua comment này.");
                        continue;
                    }

                    try
                    {
                        // Kiểm tra Customer có tồn tại chưa
                        var customer = await _context.Customers.FirstOrDefaultAsync(s => s.CustomerID == customerID);
                        if (customer == null)
                        {
                            customer = new Customer { CustomerID = customerID, FacebookName = customerName };
                            _context.Customers.Add(customer);
                            await _context.SaveChangesAsync(); // Lưu ngay lập tức để tránh lỗi khóa ngoại
                        }

                        // Kiểm tra LiveStreamCustomer có tồn tại chưa
                        var liveStreamCustomer = await _context.LiveStreamsCustomers
                            .FirstOrDefaultAsync(s => s.LivestreamID == liveStreamId && s.CustomerID == customerID);

                        if (liveStreamCustomer == null)
                        {
                            liveStreamCustomer = new LiveStreamCustomer { CustomerID = customerID, LivestreamID = liveStreamId };
                            _context.LiveStreamsCustomers.Add(liveStreamCustomer);
                            await _context.SaveChangesAsync();
                        }

                        // Lấy ID của LiveStreamCustomer sau khi lưu
                        int liveStreamCustomerId = liveStreamCustomer.LiveStreamCustomerId;

                        // Kiểm tra Comment có tồn tại chưa
                        bool commentExists = await _context.Comments.AnyAsync(s => s.CommentID == commentID);
                        if (!commentExists)
                        {
                            _context.Comments.Add(new Comment
                            {
                                CommentID = commentID,
                                Content = content,
                                CommentTime = commentTime,
                                LiveStreamCustomerID = liveStreamCustomerId
                            });

                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Lỗi khi lưu dữ liệu: {ex.InnerException?.Message}");
                    }
                }
            }

            return await _context.Comments.ToListAsync();
        }


    }
}