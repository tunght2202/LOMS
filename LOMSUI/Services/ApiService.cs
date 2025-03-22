using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LOMSUI.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace LOMSUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://10.0.2.2:7112/api/Auth";

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
    }
}
