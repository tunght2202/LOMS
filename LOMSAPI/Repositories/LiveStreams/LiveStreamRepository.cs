using System.Text.Json;
using LOMSAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LOMSAPI.Repositories.LiveStreams
{
    public class LiveStreamRepository : ILiveStreamRepostitory
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        private const string ACCESS_TOKEN = "EAAIYLfie53cBO4DIZCUbRZAtBGhgLQQDu6VBCoYfQnAWP0b9S3UyAdvSim9Ldp5huJDRiLDIcjpcNokNjGcsW1oyiE8zHDRmdnNZAby3Txfcz1jbcZA073BvTBSQ9ZC6McarJKRDIzVohZBth53Yo1ilkf8Bou9546qia6LU8En5Glf9ZAqnYA1z4ZCtl6bSg17MEdJFuKFhcKp4Rh1gpAZDZD"; // Nên chuyển vào config
        
        public LiveStreamRepository(LOMSDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }
        public async Task<int> DeleteLiveStream(string liveStreamId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LiveStream>> GetAllLiveStreams(string pageId)
        {
            string apiUrl = $"https://graph.facebook.com/v22.0/{pageId}/live_videos?fields=id,title,description,created_time,live_views,status,embed_html,video&access_token={ACCESS_TOKEN}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Lỗi khi gọi API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var liveStreams = ParseLiveStreams(jsonResponse, pageId);

            // Lưu vào cơ sở dữ liệu nếu cần
        //    await SaveLiveStreamsToDb(liveStreams);

            return liveStreams;
        }

        public async Task<LiveStream> GetLiveStreamById(string liveStreamId)
        {
            throw new NotImplementedException();
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
                        EndTime = status == "VOD" ? DateTime.UtcNow : null,
                        Status = status == "LIVE",
                        UserID = pageId,
                        ListProductID = 0
                    };
                    liveStreams.Add(liveStream);
                }
            }
            return liveStreams;
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
