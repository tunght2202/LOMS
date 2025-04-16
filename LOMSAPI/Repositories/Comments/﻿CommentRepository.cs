using LOMSAPI.Data.Entities;
using System.Text.RegularExpressions;
using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        private readonly IConfiguration _configuration;
        public CommentRepository(LOMSDbContext context, HttpClient httpClient, IDistributedCache cache, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _configuration = configuration;
           
        }


        public async Task<List<CommentModel>> GetAllComments(string LiveStreamId, string token)
        {
            if (string.IsNullOrEmpty(LiveStreamId))
                throw new ArgumentException("Unable to get LiveStream ID from URL");
            await _cache.SetStringAsync("Livestream_Id", LiveStreamId, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            });

            string apiUrl = $"https://graph.facebook.com/v22.0/{LiveStreamId}/comments?fields=from%7Bname%2Cpicture%7D%2Cmessage%2Ccreated_time&access_token={token}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error calling API: {response.StatusCode}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return await ParseCommentsAsync(jsonResponse, LiveStreamId);
        }

        private async Task<List<CommentModel>> ParseCommentsAsync(string jsonResponse, string liveStreamId)
        {
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;
            List<CommentModel> comments = new List<CommentModel>();
            if (root.TryGetProperty("data", out JsonElement dataElement))
            {
                foreach (JsonElement item in dataElement.EnumerateArray())
                {
                    string? commentID = item.GetProperty("id").GetString();
                    string? content = item.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "";
                    DateTime commentTime = DateTime.Parse(item.GetProperty("created_time").GetString().Replace("+0000", "Z"));

                    JsonElement? fromElement = item.TryGetProperty("from", out var fe) ? fe : (JsonElement?)null;
                    string? customerID = fromElement?.TryGetProperty("id", out var idElement) == true ? idElement.GetString() : null;
                    string? customerName = fromElement?.TryGetProperty("name", out var nameElement) == true ? nameElement.GetString() : null;
                    string? avatar = fromElement?.TryGetProperty("picture", out var pictureElement) == true &&
                                     pictureElement.TryGetProperty("data", out var data1Element) &&
                                     data1Element.TryGetProperty("url", out var urlElement)
                                     ? urlElement.GetString()
                                     : null;


                    // Kiểm tra nếu có dữ liệu bị null
                    if (string.IsNullOrEmpty(commentID) || string.IsNullOrEmpty(customerID) || string.IsNullOrEmpty(content))
                    {
                        Console.WriteLine("Comment data is missing! Ignore this comment.");
                        continue;
                    }

                    try
                    {
                        // Kiểm tra Customer có tồn tại chưa
                        var customer = await _context.Customers.FirstOrDefaultAsync(s => s.CustomerID == customerID);
                        if (customer == null)
                        {
                            customer = new Customer { CustomerID = customerID, FacebookName = customerName, ImageURL = avatar };
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
                                LiveStreamCustomerID = liveStreamCustomerId,
                            });
                            await _context.SaveChangesAsync();
                        }
                        var comment = await _context.Comments.FirstOrDefaultAsync(s => s.CommentID == commentID);
                        comments.Add(new CommentModel
                        {
                            CommentID = comment.CommentID,
                            CustomerId = customerID,
                            CustomerName = customerName,
                            Content = comment.Content,
                            CommentTime = commentTime,
                            CustomerAvatar = avatar
                        });
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Error saving data: {ex.InnerException?.Message}");
                    }

                }
            }

            return comments;
        }


    }
}