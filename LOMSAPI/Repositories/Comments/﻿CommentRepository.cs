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

        private const string ACCESS_TOKEN = "EAAIYLfie53cBO29YxskVwTNeFdJoh53pKKZCFEIn9kU0eiYdGN48Gw9ZCsvHNagm0p9OwsFfVGDOHUiZAg7m1T3UZCZCmVkhjsroPO84YJ8S6iOKYRSjC2WJgcto0eNNeY1XEwaVAsZChllrocOhi8GFqX8SrxlZAFNeiG2fI7rrYZAXaEjVoMp4jaTawvKfZAByuU3jZC1xGt7svT5CWDf53nyvkZD";
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
            return allComments.FindAll(c => c.Content.Contains(ProductCode, StringComparison.OrdinalIgnoreCase));
        }

        //private string ExtractLiveStreamId(string url)
        //{
        //    var match = Regex.Match(url, @"/videos/(\d+)");
        //    return match.Success ? match.Groups[1].Value : null;
        //}

        private async Task<List<Comment>> ParseCommentsAsync(string jsonResponse, string LiveStreamId)
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
                                : "Unknown";

                    string? customerName = item.TryGetProperty("from", out JsonElement from1Element)
                                && from1Element.TryGetProperty("name", out JsonElement nameElement)
                                ? nameElement.GetString()
                                : "Unknown";

                    if (!_context.Customers.Any(s => s.CustomerID == customerID))
                    {
                        _context.Customers.Add(new Customer
                        {
                            CustomerID = customerID,
                            FacebookName = customerName
                        });
                        _context.SaveChangesAsync();
                    }

                    if (!_context.LiveStreamsCustomers.Any(s => s.LivestreamID == LiveStreamId && s.CustomerID == customerID))
                    {
                        _context.LiveStreamsCustomers.Add(new LiveStreamCustomer
                        {
                            CustomerID = customerID,
                            LivestreamID = LiveStreamId,
                        });
                        _context.SaveChangesAsync();
                    }

                    if (!_context.Comments.Any(s => s.CommentID == commentID))
                    {
                        var lsci = _context.LiveStreamsCustomers.FirstOrDefault(v => v.LivestreamID == LiveStreamId && v.CustomerID == customerID).LiveStreamCustomerId;
                        _context.Comments.Add(new Comment
                        {
                            CommentID = commentID,
                            Content = content,
                            CommentTime = commentTime,
                            LiveStreamCustomerID = lsci
                        });
                        await _context.SaveChangesAsync();
                    }
                    
                }

                
            }
            return await _context.Comments.ToListAsync();
        }
    }
}