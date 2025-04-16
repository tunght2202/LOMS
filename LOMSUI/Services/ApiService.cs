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
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

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

        public async Task<bool> UpdateFacebookTokenAsync(string token)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{BASE_URL}/update-token-facebook?token={token}", null);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Update Token Response: {responseBody}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Facebook token: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RequestOtpAsync(ForgotPasswordModel model)
        {
             return await SendPostRequestAsync("reset-password-request", model);
        }
        public async Task<bool> VerifyOtpAsync(VerifyOtpModel model)
        {
            return await SendPostRequestAsync("reset-password-verify-otp", model, checkMessage: "OTP valid. You can reset your password.");
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model) 
        {

            return await SendPostRequestAsync("reset-password", model);
        }

        private async Task<bool> SendPostRequestAsync(string endpoint, object model, string checkMessage = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{BASE_URL.TrimEnd('/')}/{endpoint.TrimStart('/')}";

                using (var response = await _httpClient.PostAsync(url, content))
                {
                    if (!response.IsSuccessStatusCode) return false;

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);

                    if (!string.IsNullOrEmpty(checkMessage))
                        return (responseData?.message ?? "").ToString().Contains(checkMessage);

                    return responseData?.success ?? true;
                }
            }
            catch
            {
                return false;
            }
        }


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

        public async Task<UserModels> GetUserProfileAsync(string token)
        {
            string url = $"{BASE_URL}/user-profile";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserModels>(json);
            }
            else
            {
                throw new Exception("Unable to get user information");
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
                    Console.WriteLine($"API error: {response.StatusCode}");
                    return new List<CommentModel>();
                }

                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {json}");

                var comments = JsonConvert.DeserializeObject<List<CommentModel>>(json);

                if (comments == null || comments.Count == 0)
                {
                    Console.WriteLine("No comments found or parsing failed.");
                    return new List<CommentModel>();
                }

                comments = comments.Where(c => !string.IsNullOrEmpty(c.Content)).ToList();

                Console.WriteLine($"Total comments: {comments.Count}");

                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching comments: {ex.Message}");
                return new List<CommentModel>();
            }
        }



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

        public async Task<bool> SetupListProductAsync(string livestreamId, int listProductId)
        {
            var response = await _httpClient.PutAsync(
                $"{BASE_URLL}/ListProducts/AddListProductInToLiveStream/listProductID/{listProductId}/liveStreamID/{livestreamId}",
                null); 
                
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ListProductModel>> GetListProductsAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASE_URLL}/ListProducts/GetAllListProduct");

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ListProductModel>>(responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetListProductsAsync: {ex.Message}");
                throw;
            }
        }




        public async Task<CustomerModel> GetCustomerByIdAsync(string customerId)
        {
            var response = await _httpClient.GetAsync($"{BASE_URLL}/Customers/GetCustomerById/{customerId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomerModel>(content);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error {response.StatusCode}: {error}");
            }
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

        public async Task<List<CustomerModel>> GetCustomersByUserIdAsync()
        {
            try
            {
                var url = $"{BASE_URLL}/Customers/User";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var customers = JsonConvert.DeserializeObject<List<CustomerModel>>(json);
                    return customers ?? new List<CustomerModel>();
                }
                else
                {
                    return new List<CustomerModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gọi API GetCustomersByUserIdAsync: {ex.Message}");
                return new List<CustomerModel>();
            }
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

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            try
            {
                var url = $"{BASE_URLL}/Orders/{id}";
                var response = await _httpClient.GetAsync(url);

                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Failed to fetch order. Status Code: {response.StatusCode}");
                        return null;
                    }

                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response: {responseBody}");

                    return JsonConvert.DeserializeObject<OrderModel>(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateOrderFromCommentAsync(string commentId)
        {
            var url = $"{BASE_URLL}/Orders?commentId={commentId}";

            var response = await _httpClient.PostAsync(url,null);
            return response.IsSuccessStatusCode;
        }


        public async Task<(bool isSuccess, string message)> CreateOrdersFromCommentsAsync(string liveStreamId)
        {
            var url = $"{BASE_URLL}/Orders/CreateOrderFromComments/LiveStreamID/{liveStreamId}";

            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return (true, $"Order {json} created successfully!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"{error}");
            }
        }


        public async Task<string> UpdateUserProfileRequestAsync(UserModels model, string token)
        {
            var content = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(model.UserName))
                content.Add(new StringContent(model.UserName), "UserName");

            if (!string.IsNullOrEmpty(model.PhoneNumber))
                content.Add(new StringContent(model.PhoneNumber), "PhoneNumber");

            if (!string.IsNullOrEmpty(model.Email))
                content.Add(new StringContent(model.Email), "Email");

            if (!string.IsNullOrEmpty(model.Address))
                content.Add(new StringContent(model.Address), "Address");

            if (!string.IsNullOrEmpty(model.Gender))
                content.Add(new StringContent(model.Gender), "Gender");

            if (!string.IsNullOrEmpty(model.Password))
                content.Add(new StringContent(model.Password), "Password");

            var response = await _httpClient.PutAsync($"{BASE_URL}/update-userProfile-request", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }


        public async Task<bool> VerifyOtpAndUpdateProfileAsync(VerifyOtpModel otpModel, string token)
        {
            try
            {
                var json = JsonConvert.SerializeObject(otpModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsync($"{BASE_URL}/update-userProfie", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {responseContent}");

                var jsonResponse = JObject.Parse(responseContent);
                var message = jsonResponse["message"]?.ToString();

                return !string.IsNullOrEmpty(message) && message.Contains("Information edited successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in VerifyOtpAndUpdateProfileAsync: {ex.Message}");
                return false;
            }
        }

        //GetAllproduct
        public async Task<List<ProductModel>> GetAllproduct()
        {
            string url = $"{BASE_URLL}/Products/GetProducts";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<ProductModel>>(json);
                    return data ?? new List<ProductModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching live streams from Facebook: {ex.Message}");
            }
            return new List<ProductModel>();
        }

    }
}