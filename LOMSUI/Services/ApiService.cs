using System;
using System.Buffers.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Android.Util;
using LOMSUI.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Collections.Generic;

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
        public void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private HttpClient CreateDefaultHttpClient()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
            return new HttpClient(handler);
        }

        public async Task<string> LoginAsync(LoginModel login)
        {
            try
            {
                string json = JsonConvert.SerializeObject(login);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await _httpClient.PostAsync($"{BASE_URL}/login-account-request", content))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode) return null;

                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    string token = responseData?.token;

                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        return token;
                    }

                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<bool> RequestOtpAsync(ForgotPasswordModel model) => await SendPostRequestAsync("reset-password-request", model);
        public async Task<bool> VerifyOtpAsync(VerifyOtpModel model) => await SendPostRequestAsync("reset-password-verify-otp", model, checkMessage: "OTP hợp lệ");
        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model) => await SendPostRequestAsync("reset-password", model);

        // add RegisterAsync
        public async Task<bool> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                string json = JsonConvert.SerializeObject(registerModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await _httpClient.PostAsync($"{BASE_URL}/register-account-request", content))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API] register-account-request Response: {response.StatusCode} - {responseBody}");

                    if (!response.IsSuccessStatusCode) return false;

                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    return responseData?.success ?? true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] register-account-request: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SendPostRequestAsync(string endpoint, object model, string checkMessage = null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await _httpClient.PostAsync($"{BASE_URL}/{endpoint}", content))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

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
                return false;
            }
        }
        public async Task<List<CommentModel>> GetComments(string liveStreamId)
        {
            try
            {
                string url = $"{BASE_URLL}/Comment/get-all-comment?liveStreamId={liveStreamId}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status code {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {json}");

                var apiResponse = JsonConvert.DeserializeObject<List<dynamic>>(json);
                List<CommentModel> comments = new List<CommentModel>();

                if (apiResponse != null && apiResponse.Any())
                {
                    foreach (var item in apiResponse)
                    {
                        try
                        {
                            comments.Add(new CommentModel
                            {
                                CommentID = item.commentID?.ToString() ?? "",
                                Content = item.content?.ToString() ?? "",
                                CommentTime = item.commentTime != null ? (DateTime)item.commentTime : DateTime.MinValue,
                                CustomerID = item.liveStreamCustomer?.customer?.customerID?.ToString() ?? "",
                                CustomerName = item.liveStreamCustomer?.customer?.facebookName?.ToString() ?? "Ẩn danh",
                                LiveStreamID = item.liveStreamCustomer?.livestreamID?.ToString() ?? "",
                                AvatarUrl = item.liveStreamCustomer?.customer?.imageURL?.ToString() ?? ""
                            });

                            var nestedComments = item.liveStreamCustomer?.comments?["$values"];
                            if (nestedComments != null)
                            {
                                foreach (var nestedComment in nestedComments)
                                {
                                    comments.Add(new CommentModel
                                    {
                                        CommentID = nestedComment.commentID?.ToString() ?? "",
                                        Content = nestedComment.content?.ToString() ?? "",
                                        CommentTime = nestedComment.commentTime != null ? (DateTime)nestedComment.commentTime : DateTime.MinValue,
                                        CustomerID = item.liveStreamCustomer?.customer?.customerID?.ToString() ?? "",
                                        CustomerName = item.liveStreamCustomer?.customer?.facebookName?.ToString() ?? "Ẩn danh",
                                        LiveStreamID = item.liveStreamCustomer?.livestreamID?.ToString() ?? "",
                                        AvatarUrl = item.liveStreamCustomer?.customer?.imageURL?.ToString() ?? ""
                                    });
                                }
                            }
                        }
                        catch (Exception innerEx)
                        {
                            Console.WriteLine($"Error processing item: {innerEx.Message}");
                        }
                    }

                    comments = comments.GroupBy(c => c.CommentID).Select(g => g.First()).ToList();

                    comments = comments.Where(c => !string.IsNullOrEmpty(c.Content)).ToList();

                    Console.WriteLine($"Total comments after processing: {comments.Count}");
                }
                else
                {
                    Console.WriteLine("No comments were returned by the API.");
                }

                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching comments: {ex.Message}");
                return new List<CommentModel>();
            }
        }




        /* public async Task<List<CommentModel>> GetCommentsByProductCode(string liveStreamURL, string productCode)
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
 */

        public async Task<List<LiveStreamModel>> GetAllLiveStreams()
        {
            string url = $"{BASE_URLL}/LiveStreams/facebook";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<LiveStreamModel>>(json);
                    return data ?? new List<LiveStreamModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching live streams from Facebook: {ex.Message}");
            }
            return new List<LiveStreamModel>();
        }



        // Lấy chi tiết livestream theo ID
        public async Task<LiveStreamModel> GetLiveStreamByIdAsync(string livestreamId)
        {
            string url = $"{BASE_URLL}/LiveStreams/{livestreamId}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LiveStreamModel>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching livestream details: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> DeleteLiveStreamAsync(string livestreamId)
        {
            string url = $"{BASE_URLL}/LiveStreams/{livestreamId}";

            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting livestream: {ex.Message}");
            }
            return false;
        }

        public async Task<CustomerModel> GetCustomerByIdAsync(string customerId)
        {
            var response = await _httpClient.GetAsync($"{BASE_URLL}/Customers/GetCustomerById/{customerId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomerModel>(content);
            }
            return null;
        }

        public async Task<bool> UpdateCustomerAsync(string customerId, CustomerModel customer)
        {
            var json = JsonConvert.SerializeObject(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BASE_URLL}/Customers/UpdateCustomerByID/{customerId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CustomerModel>> GetCustomersByLiveStreamIdAsync(string liveStreamID)
        {
            var url = $"{BASE_URLL}/Customers/LiveStream/{liveStreamID}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CustomerModel>>(json);
            }

            throw new Exception($"Unable to load customer list: {response.StatusCode} - {response.ReasonPhrase}");
        }

        public async Task<List<OrderModel>> GetOrdersByCustomerIdAsync(string customerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BASE_URLL}/Orders/customer/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<OrderModel>>(json);
                    return orders ?? new List<OrderModel>();
                }
                else
                {
                    return new List<OrderModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when calling API to get orders: {ex.Message}");
                return new List<OrderModel>();
            }
        }

        public async Task<List<OrderModel>> GetOrdersByLiveStreamIdAsync(string liveStreamId)
        {
            var url = $"{BASE_URLL}/Orders/livestream/{liveStreamId}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrderModel>>(json);
        }



    }
}