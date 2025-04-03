﻿using System.Text.Json;
using Azure.Core;
using LOMSAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LOMSAPI.Repositories.LiveStreams
{
    public class LiveStreamRepository : ILiveStreamRepostitory
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;
        private string _pageId;

        public LiveStreamRepository(LOMSDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
            _accessToken = _configuration["Facebook:AccessToken"] ?? throw new ArgumentNullException("Access token không được cấu hình.");
            _pageId = _configuration["Facebook:PageId"] ?? throw new ArgumentNullException("Page ID không được cấu hình.");
        }

        public async Task<int> DeleteLiveStream(string liveStreamId)
        {
            if (string.IsNullOrEmpty(liveStreamId))
                throw new ArgumentNullException(nameof(liveStreamId));

            var liveStream = await _context.LiveStreams
                .FirstOrDefaultAsync(ls => ls.LivestreamID == liveStreamId);

            if (liveStream == null)
                return 0;

            liveStream.StatusDelete = true;
            return await _context.SaveChangesAsync();
        }

        // Updated to take userId from token
        public async Task<IEnumerable<LiveStream>> GetAllLiveStreams(string userId)
        {
            if (string.IsNullOrEmpty(userId) || !await _context.Users.AnyAsync(u => u.Id == userId))
                throw new ArgumentException("Invalid or non-existent UserID", nameof(userId));

            var liveStreams = new List<LiveStream>();
            string apiUrl = $"https://graph.facebook.com/v22.0/{_pageId}/live_videos?fields=id,title,creation_time,status,embed_html,permalink_url&access_token={_accessToken}";

            try
            {
                while (!string.IsNullOrEmpty(apiUrl))
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        if (errorResponse.Contains("OAuthException") && errorResponse.Contains("code\":190"))
                            throw new UnauthorizedAccessException("Access token has expired. Please update the token.");
                        throw new Exception($"API call failed: {response.StatusCode} - {errorResponse}");
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    liveStreams.AddRange(ParseLiveStreams(jsonResponse, userId)); // Pass userId

                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    apiUrl = doc.RootElement.TryGetProperty("paging", out JsonElement paging) &&
                            paging.TryGetProperty("next", out JsonElement next)
                            ? next.GetString() : null;
                }

                await SaveLiveStreamsToDb(liveStreams); // Uncommented to save to DB
                return liveStreams;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching live streams: {ex.Message}", ex);
            }
        }

        // Updated to take userId from token
        public async Task<LiveStream> GetLiveStreamById(string liveStreamId, string userId)
        {
            if (string.IsNullOrEmpty(liveStreamId))
                throw new ArgumentNullException(nameof(liveStreamId));
            if (string.IsNullOrEmpty(userId) || !await _context.Users.AnyAsync(u => u.Id == userId))
                throw new ArgumentException("Invalid or non-existent UserID", nameof(userId));

            var liveStream = await _context.LiveStreams
                .FirstOrDefaultAsync(ls => ls.LivestreamID == liveStreamId && !ls.StatusDelete);

            if (liveStream != null)
                return liveStream;

            string apiUrl = $"https://graph.facebook.com/v22.0/{liveStreamId}?fields=id,title,creation_time,status,embed_html,permalink_url&access_token={_accessToken}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    if (errorResponse.Contains("OAuthException") && errorResponse.Contains("code\":190"))
                        throw new UnauthorizedAccessException("Access token has expired. Please update the token.");
                    throw new Exception($"API call failed: {response.StatusCode} - {errorResponse}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;

                if (!root.TryGetProperty("id", out JsonElement idElement) ||
                    !root.TryGetProperty("creation_time", out JsonElement creationTimeElement) ||
                    !root.TryGetProperty("status", out JsonElement statusElement))
                {
                    throw new Exception("Incomplete livestream data from API.");
                }

                string status = statusElement.GetString() ?? throw new Exception("Status is missing from API response");
                string permalink = root.TryGetProperty("permalink_url", out JsonElement perm) ? perm.GetString() : null;

                liveStream = new LiveStream
                {
                    LivestreamID = idElement.GetString() ?? liveStreamId,
                    StreamTitle = root.TryGetProperty("title", out JsonElement title) ? title.GetString() : "Untitled",
                    StreamURL = permalink != null ? $"https://www.facebook.com{permalink}" : "Unknown",
                    StartTime = DateTime.Parse(creationTimeElement.GetString().Replace("+0000", "Z")),
                
                    Status = status,
                    UserID = userId, // Use UserID from token
                    ListProductID = null,
                    StatusDelete = false
                };

                await _context.LiveStreams.AddAsync(liveStream);
                await _context.SaveChangesAsync();

                return liveStream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching live stream: {ex.Message}", ex);
            }
        }

        // Updated to take userId
        private List<LiveStream> ParseLiveStreams(string jsonResponse, string userId)
        {
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            var liveStreams = new List<LiveStream>();

            if (!doc.RootElement.TryGetProperty("data", out JsonElement dataElement))
                return liveStreams;

            foreach (JsonElement item in dataElement.EnumerateArray())
            {
                if (!item.TryGetProperty("id", out JsonElement idElement) ||
                    !item.TryGetProperty("creation_time", out JsonElement creationTimeElement) ||
                    !item.TryGetProperty("status", out JsonElement statusElement))
                {
                    continue;
                }

                string status = statusElement.GetString() ?? "Unknown";
                string permalink = item.TryGetProperty("permalink_url", out JsonElement perm) ? perm.GetString() : null;

                liveStreams.Add(new LiveStream
                {
                    LivestreamID = idElement.GetString() ?? Guid.NewGuid().ToString(),
                    StreamTitle = item.TryGetProperty("title", out JsonElement title) ? title.GetString() : "Untitled",
                    StreamURL = permalink != null ? $"https://www.facebook.com{permalink}" : "Unknown",
                    StartTime = DateTime.Parse(creationTimeElement.GetString().Replace("+0000", "Z")),
                  
                    Status = status,
                    UserID = userId, // Use UserID from token
                    ListProductID = null,
                    StatusDelete = false
                });
            }
            return liveStreams;
        }

        // Kept for reference but not used for UserID
        private string pageIdFromPermalink(string permalinkUrl)
        {
            if (string.IsNullOrEmpty(permalinkUrl)) return null;
            var parts = permalinkUrl.Split('/');
            return parts.Length > 1 ? parts[1] : null;
        }

        private async Task SaveLiveStreamsToDb(List<LiveStream> liveStreams)
        {
            if (liveStreams == null || !liveStreams.Any())
                return;

            foreach (var liveStream in liveStreams)
            {
                if (!await _context.LiveStreams.AnyAsync(ls => ls.LivestreamID == liveStream.LivestreamID))
                {
                    await _context.LiveStreams.AddAsync(liveStream);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}