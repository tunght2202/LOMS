using System.Text.Json;
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
            throw new NotImplementedException();
        }

        // Lấy tất cả livestream từ Facebook theo pageId, hỗ trợ phân trang
        public async Task<IEnumerable<LiveStream>> GetAllLiveStreams()
        {
          
            var liveStreams = new List<LiveStream>();
            string apiUrl = $"https://graph.facebook.com/v22.0/{_pageId}/live_videos?fields=id,title,creation_time,status,embed_html,permalink_url&access_token={_accessToken}";

            while (!string.IsNullOrEmpty(apiUrl))
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    if (errorResponse.Contains("OAuthException") && errorResponse.Contains("code\":190"))
                    {
                        throw new Exception("Access token đã hết hạn. Vui lòng cập nhật token mới.");
                    }
                    throw new Exception($"Lỗi khi gọi API: {response.StatusCode} - {errorResponse}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                liveStreams.AddRange(ParseLiveStreams(jsonResponse, _pageId));

                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                JsonElement root = doc.RootElement;
                apiUrl = root.TryGetProperty("paging", out JsonElement paging) &&
                         paging.TryGetProperty("next", out JsonElement next)
                         ? next.GetString() : null;
            }

            //      await SaveLiveStreamsToDb(liveStreams);
            return liveStreams;
        }

        // Lấy thông tin chi tiết của một livestream theo ID
        public async Task<LiveStream> GetLiveStreamById(string liveStreamId)
        {
            var liveStream = await _context.LiveStreams.FirstOrDefaultAsync(ls => ls.LivestreamID == liveStreamId);
            if (liveStream != null)
                return liveStream;

            string apiUrl = $"https://graph.facebook.com/v22.0/{liveStreamId}?fields=id,title,creation_time,status,embed_html,permalink_url&access_token={_accessToken}";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                if (errorResponse.Contains("OAuthException") && errorResponse.Contains("code\":190"))
                {
                    throw new Exception("Access token đã hết hạn. Vui lòng cập nhật token mới.");
                }
                throw new Exception($"Lỗi khi gọi API: {response.StatusCode} - {errorResponse}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;

            if (!root.TryGetProperty("id", out JsonElement idElement) ||
                !root.TryGetProperty("creation_time", out JsonElement creationTimeElement) ||
                !root.TryGetProperty("status", out JsonElement statusElement))
            {
                throw new Exception("Dữ liệu livestream từ API không đầy đủ.");
            }

            string status = statusElement.GetString();
            liveStream = new LiveStream
            {
                LivestreamID = idElement.GetString(),
                StreamTitle = root.TryGetProperty("title", out JsonElement title) ? title.GetString() : "Untitled",
                StreamURL = root.TryGetProperty("permalink_url", out JsonElement permalink)
                    ? $"https://www.facebook.com{permalink.GetString()}"
                    : "Unknown",
                StartTime = DateTime.Parse(creationTimeElement.GetString().Replace("+0000", "Z")),
                //  EndTime = status == "VOD" ? DateTime.UtcNow : null,
                Status = status,
                UserID = pageIdFromPermalink(root.TryGetProperty("permalink_url", out JsonElement perm) ? perm.GetString() : null) ?? "Unknown",
                ListProductID = null,
                StatusDelete = false

            };
            return liveStream;
        }

        // Phân tích JSON từ API
        private List<LiveStream> ParseLiveStreams(string jsonResponse, string pageId)
        {
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;

            var liveStreams = new List<LiveStream>();

            if (root.TryGetProperty("data", out JsonElement dataElement))
            {
                foreach (JsonElement item in dataElement.EnumerateArray())
                {
                    if (!item.TryGetProperty("id", out JsonElement idElement) ||
                        !item.TryGetProperty("creation_time", out JsonElement creationTimeElement) ||
                        !item.TryGetProperty("status", out JsonElement statusElement))
                    {
                        continue;
                    }

                    string status = statusElement.GetString();
                    var liveStream = new LiveStream
                    {
                        LivestreamID = idElement.GetString(),
                        StreamTitle = item.TryGetProperty("title", out JsonElement title) ? title.GetString() : "Untitled",
                        StreamURL = item.TryGetProperty("permalink_url", out JsonElement permalink)
                            ? $"https://www.facebook.com{permalink.GetString()}"
                            : "Unknown",
                        StartTime = DateTime.Parse(creationTimeElement.GetString().Replace("+0000", "Z")),
                        // EndTime = status == "VOD" ? DateTime.UtcNow : null,
                        Status = status,
                        UserID = pageIdFromPermalink(item.TryGetProperty("permalink_url", out JsonElement perm) ? perm.GetString() : null) ?? pageId,
                        ListProductID = null,
                        StatusDelete = false
                    };
                    liveStreams.Add(liveStream);
                }
            }
            return liveStreams;
        }

        // Trích xuất pageId từ permalink_url
        private string pageIdFromPermalink(string permalinkUrl)
        {
            if (string.IsNullOrEmpty(permalinkUrl)) return null;
            var parts = permalinkUrl.Split('/');
            return parts.Length > 1 ? parts[1] : null; // Lấy phần pageId từ "/pageId/videos/videoId"
        }

        // Lưu livestream vào cơ sở dữ liệu
        private async Task SaveLiveStreamsToDb(List<LiveStream> liveStreams)
        {
            foreach (var liveStream in liveStreams)
            {
                if (!await _context.LiveStreams.AnyAsync(ls => ls.LivestreamID == liveStream.LivestreamID))
                {
                    _context.LiveStreams.Add(liveStream);
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}
