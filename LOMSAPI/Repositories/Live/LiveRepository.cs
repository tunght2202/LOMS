using LOMSAPI.Data;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LOMSAPI.Repositories.Live
{
    public class LiveRepository : ILiveRepository
    {
        private readonly LOMSDbContext _context;
        private readonly HttpClient _httpClient;
        private const string AccessToken = "EAAIYLfie53cBO4IhzpgN3jipfyZC6aFIOZClhpVPgunvbfJagIKmyjWcLD1ZBedEPBxC8s1WNs7IXstJsd75ZBH3cU5O2kzZBXFxATnU3aDM7wUtOJl4xONk15zNHa8oIpDsXMBAjzPV3JC7WFU6UjMU20TMfmFmTadI1GJqaZBeV5W9934nJzeVy6vLkyo5SgL7ctYjuz2fGQYvrlO8gZD";
        private const string PageId = "266349363239226";
        private const string BaseUrl = "https://graph.facebook.com/v22.0";

        public LiveRepository(LOMSDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<List<LiveStream>> GetLiveStreamsAsync()
        {
            return await _context.LiveStreams.ToListAsync();
        }

        public async Task<List<LiveStream>> FetchLiveStreamsFromFacebookAsync()
        {
            string url = $"{BaseUrl}/{PageId}/live_videos?fields=id,title,permalink_url,creation_time,status,broadcast_status&access_token={AccessToken}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Facebook API Response: {json}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return new List<LiveStream>();
                }

                var result = JsonConvert.DeserializeObject<FacebookLiveResponse>(json);

                if (result?.Data == null || !result.Data.Any())
                {
                    Console.WriteLine("Không có livestream nào được trả về từ Facebook.");
                    return new List<LiveStream>();
                }

                List<LiveStream> liveStreams = result.Data
                    .Select(l => new LiveStream
                    {
                        LivestreamID = l.Id,
                        UserID = PageId,
                        StreamURL = l.PermalinkUrl,
                        StreamTitle = l.Title,
                        StartTime = l.CreationTime,
                        EndTime = l.Status == "ENDED" ? DateTime.UtcNow : (DateTime?)null,
                        Status = l.Status == "LIVE"
                    }).ToList();

                Console.WriteLine($"Số livestream lấy được: {liveStreams.Count}");

                    return liveStreams;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy livestream từ Facebook: {ex.Message}");
                return new List<LiveStream>();
            }
        }

    }
}
