using System.Text.Json;
using LOMSAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<LiveStream>> GetAllLiveStreamsFromFacebook(string userId)
        {
            if (string.IsNullOrEmpty(userId) || !await _context.Users.AnyAsync(u => u.Id == userId))
                throw new ArgumentException("Invalid or non-existent UserID", nameof(userId));

            var liveStreamsFromFacebook = new List<LiveStream>();
            string apiUrl = $"https://graph.facebook.com/v22.0/{_pageId}/live_videos?fields=id,title,creation_time,status,embed_html,permalink_url&access_token={_accessToken}";

            try
            {
                // Lấy tất cả LivestreamID hiện có trong DB với StatusDelete = false
                var existingLiveStreams = await _context.LiveStreams
                    .Where(ls => ls.UserID == userId && !ls.StatusDelete)
                    .ToListAsync();

                var existingIds = existingLiveStreams.Select(ls => ls.LivestreamID).ToList();

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
                    var parsedLiveStreams = ParseLiveStreams(jsonResponse, userId);

                    foreach (var liveStream in parsedLiveStreams)
                    {
                        // Kiểm tra xem livestream đã tồn tại trong DB chưa
                        var existingLiveStream = existingLiveStreams.FirstOrDefault(ls => ls.LivestreamID == liveStream.LivestreamID);
                        if (existingLiveStream != null)
                        {
                            // Nếu đã tồn tại, cập nhật trạng thái nếu cần
                            UpdateLiveStreamStatus(existingLiveStream, liveStream);
                        }
                        else if (!existingIds.Contains(liveStream.LivestreamID))
                        {
                            // Nếu chưa tồn tại, thêm mới
                            liveStreamsFromFacebook.Add(liveStream);
                            existingIds.Add(liveStream.LivestreamID);
                        }
                    }

                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    apiUrl = doc.RootElement.TryGetProperty("paging", out JsonElement paging) &&
                             paging.TryGetProperty("next", out JsonElement next)
                             ? next.GetString() : null;
                }

                // Lưu các livestream mới vào DB
                await SaveLiveStreamsToDb(liveStreamsFromFacebook);

                // Trả về danh sách kết hợp: livestream từ DB (chưa xóa) + livestream mới từ Facebook
                return existingLiveStreams.Concat(liveStreamsFromFacebook);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching live streams from Facebook: {ex.Message}", ex);
            }
        }

        // Hàm mới để cập nhật trạng thái livestream
        private async Task UpdateLiveStreamStatus(LiveStream existingLiveStream, LiveStream facebookLiveStream)
        {
            // Trong DB: "1" là LIVE, "2" là VOD
            string facebookStatus = facebookLiveStream.Status.ToUpper();
            if (facebookStatus == "VOD" && existingLiveStream.Status == "LIVE")
            {
                existingLiveStream.Status = "VOD"; // Cập nhật thành VOD
                await _context.SaveChangesAsync();
            }
          
        }

        // Điều chỉnh ParseLiveStreams để gán status đúng định dạng
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

                string facebookStatus = statusElement.GetString()?.ToUpper() ?? "Unknown";
                string status = facebookStatus == "LIVE" ? "1" : facebookStatus == "VOD" ? "2" : "Unknown";
                string permalink = item.TryGetProperty("permalink_url", out JsonElement perm) ? perm.GetString() : null;

                liveStreams.Add(new LiveStream
                {
                    LivestreamID = idElement.GetString() ?? Guid.NewGuid().ToString(),
                    StreamTitle = item.TryGetProperty("title", out JsonElement title) ? title.GetString() : "Untitled",
                    StreamURL = permalink != null ? $"https://www.facebook.com{permalink}" : "Unknown",
                    StartTime = DateTime.Parse(creationTimeElement.GetString().Replace("+0000", "Z")),
                    UserID = userId,
                    ListProductID = null,
                    Status = facebookStatus, // theo FB
                    StatusDelete = false
                });
            }
            return liveStreams;
        }

        // Điều chỉnh SaveLiveStreamsToDb để không thêm trùng lặp
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

        public async Task<LiveStream> GetLiveStreamById(string liveStreamId, string userid)
        {
            if (string.IsNullOrEmpty(liveStreamId))
                throw new ArgumentNullException(nameof(liveStreamId));
            if (string.IsNullOrEmpty(userid) || !await _context.Users.AnyAsync(u => u.Id == userid))
                throw new ArgumentException("Invalid or non-existent UserID", nameof(userid));
            // Tìm kiếm trong DB
            var liveStream = await _context.LiveStreams
                .FirstOrDefaultAsync(ls => ls.LivestreamID == liveStreamId && ls.UserID == userid && !ls.StatusDelete);

            return liveStream; // Trả về null nếu không tìm thấy
        }
    }   
}
