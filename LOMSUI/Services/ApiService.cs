using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Android.Util;
using LOMSUI.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace LOMSUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://10.0.2.2:7112/api/Auth";
        private const string BASE_URLL = "https://10.0.2.2:7112/api";

        public ApiService(HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? CreateDefaultHttpClient();
        }

        private HttpClient CreateDefaultHttpClient()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
            return new HttpClient(handler);
        }

        public async Task<bool> LoginAsync(LoginModel login) => await SendPostRequestAsync("login-account-request", login);
        public async Task<bool> RequestOtpAsync(ForgotPasswordModel model) => await SendPostRequestAsync("reset-password-request", model);
        public async Task<bool> VerifyOtpAsync(VerifyOtpModel model) => await SendPostRequestAsync("reset-password-verify-otp", model, checkMessage: "OTP hợp lệ");
        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model) => await SendPostRequestAsync("reset-password", model);

        private async Task<bool> SendPostRequestAsync(string endpoint, object model, string checkMessage = null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await _httpClient.PostAsync($"{BASE_URL}/{endpoint}", content))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API] {endpoint} Response: {response.StatusCode} - {responseBody}");

                    if (!response.IsSuccessStatusCode) return false;

                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    if (checkMessage != null)
                    {
                        string message = responseData?.message;
                        return !string.IsNullOrEmpty(message) && message.Contains(checkMessage);
                    }

                    return responseData?.success ?? true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {endpoint}: {ex.Message}");
                return false;
            }
        }
        public async Task<List<CommentModel>> GetComments(string liveStreamURL)
        {
            try
            {
                string fullUrl = $"{BASE_URLL}/Comment/get-all-comment?liveStreamURL={liveStreamURL}";
                var response = await _httpClient.GetAsync(fullUrl);
                var json = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<CommentModel>>(json);

                return comments ?? new List<CommentModel>();
            }
            catch (Exception)
            {
                return new List<CommentModel>();
            }
        }

        public async Task<List<CommentModel>> GetCommentsByProductCode(string liveStreamURL, string productCode)
        {
            try
            {
                string fullUrl = $"{BASE_URLL}/Comment/get-comments-productcode?liveStreamURL={liveStreamURL}&ProductCode={productCode}";
                var response = await _httpClient.GetAsync(fullUrl);
                var json = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<List<CommentModel>>(json);

                return comments ?? new List<CommentModel>();
            }
            catch (Exception)
            {
                return new List<CommentModel>();
            }
        }


        public class FacebookLiveService
        {
            private const string AccessToken = "EAAIYLfie53cBOyeyevc6OvgUSNYinoDI7Iy4EfsgNXjPq1I4VmB7ZBuLCK9iZA6MPACGroRenMPe8wM4uSHhEZB4pTcBXIARaiXxzTjZBDG9s7aaYeOrbL6IDdj5cIOqG1ZAKUzd6owhkCmYHUbPXw4dK9uLTnrXhuMH0vRgTK14GoQkGh3Y6OFwy2gaZB0MfFVIBwG9ewZBXeQ9YiNNWZCRPXHM";
            private const string PageId = "266349363239226";
            private const string BaseUrl = "https://graph.facebook.com/v22.0";

            private readonly HttpClient _httpClient = new HttpClient();

            public async Task<List<LiveVideo>> GetLiveStreamsAsync()
            {
                string url = $"{BaseUrl}/{PageId}/live_videos?fields=id,title,permalink_url,creation_time,status&access_token={AccessToken}";

                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    string json = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode) return new List<LiveVideo>();

                    var result = JsonConvert.DeserializeObject<FacebookLiveResponse>(json);
                    return result?.Data ?? new List<LiveVideo>();
                }
                catch (Exception ex)
                {
                    return new List<LiveVideo>();
                }
            }


        }
    }
}
