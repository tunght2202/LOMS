using System.Net.Http.Headers;
using System.Text;
using LOMSUI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LOMSAPI.Models;
using System.Buffers.Text;
using Android.Media.TV;
using System.Net;
using Java.Util.Streams;

namespace LOMSUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
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

                using (HttpResponseMessage response = await _httpClient.PostAsync($"{BASE_URLL}/Auth/login-account-request", content))
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


        public async Task<RegisterResult> RegisterAsync(RegisterModel model, Android.Net.Uri imageUri)
        {
            var result = new RegisterResult();

            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(model.Username), "Username");
                    content.Add(new StringContent(model.PhoneNumber), "PhoneNumber");
                    content.Add(new StringContent(model.Email), "Email");
                    content.Add(new StringContent(model.Password), "Password");
                    content.Add(new StringContent(model.Address), "Address");
                    content.Add(new StringContent(model.Gender), "Gender");
                    content.Add(new StringContent(model.FullName), "FullName");

                    if (imageUri != null)
                    {
                        using var stream = Application.Context.ContentResolver.OpenInputStream(imageUri);
                        var imageContent = new StreamContent(stream);
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        content.Add(imageContent, "Avatar", "avatar.jpg");
                    }

                    using var response = await _httpClient.PostAsync($"{BASE_URLL}/Auth/register-account-request", content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<RegisterResult>(responseBody);
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorResponse = JsonConvert.DeserializeObject<RegisterResult>(responseBody);
                        return errorResponse;
                    }
                    else
                    {
                        return new RegisterResult
                        {
                            IsSuccess = false,
                            Message = $"Unexpected status code: {response.StatusCode}"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Unexpected error: {ex.Message}";
            }
            return result;
        }
            

        public async Task<bool> UpdateFacebookTokenAsync(string token)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{BASE_URLL}/Auth/update-token-facebook?token={token}", null);
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

        public async Task<bool> UpdateFacebookPageAsync(string pageid)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{BASE_URLL}/Auth/update-page-id?pageId={pageid}", null);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Update Page Response: {responseBody}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Facebook Page: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RequestOtpAsync(ForgotPasswordModel model)
        {
             return await SendPostRequestAsync("Auth/reset-password-request", model);
        }
        public async Task<bool> VerifyOtpAsync(VerifyOtpModel model)
        {
            return await SendPostRequestAsync("Auth/reset-password-verify-otp", model, checkMessage: "OTP valid. You can reset your password.");
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model) 
        {

            return await SendPostRequestAsync("Auth/reset-password", model);
        }

        private async Task<bool> SendPostRequestAsync(string endpoint, object model, string checkMessage = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{BASE_URLL.TrimEnd('/')}/{endpoint.TrimStart('/')}";

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


    
        public async Task<bool> VerifyOtpRegisterAsync(VerifyOtpRegisModel model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BASE_URLL}/Auth/register-account", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API Error (Verify OTP): {ex.Message}");
                return false;
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine($"JSON Error (Verify OTP): {ex.Message}");
                return false;
            }
        }

        private class ApiResponse
        {

            [JsonProperty("message")]
            public string Message { get; set; }
        }

        public async Task<RevenueData> GetRevenueDataAsync()
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-revenue"))
                {
                    if (!response.IsSuccessStatusCode) return null;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RevenueData>(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching revenue data: {ex.Message}");
                return null;
            }
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders")) 
                {
                    if (!response.IsSuccessStatusCode) return -1;
                        
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var orderResponse = JsonConvert.DeserializeObject<OrderResponse>(responseBody);
                    return orderResponse?.TotalOrders ?? -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total orders: {ex.Message}");
                return -1;
            }
        }

        public async Task<int> GetTotalOrdersDeliveredAsync()
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders-delivered"))
                {
                    if (!response.IsSuccessStatusCode) return -1;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var returnedResponse = JsonConvert.DeserializeObject<DeliveredOrderResponse>(responseBody);
                    return returnedResponse?.TotalOrdersDelivered ?? -1;
                }
            }
            catch (Exception ex)    
            {
                Console.WriteLine($"Error fetching returned orders: {ex.Message}");
                return -1;
            }
        }

        public async Task<int> GetTotalOrdersCancelledAsync()
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders-cancelled"))
                {
                    if (!response.IsSuccessStatusCode) return -1;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var cancelledResponse = JsonConvert.DeserializeObject<CancelledOrderResponse>(responseBody);
                    return cancelledResponse?.TotalOrdersCancelled ?? -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cancelled orders: {ex.Message}");
                return -1;
            }
        }

        public async Task<int> GetTotalOrdersReturnedAsync()
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders-returned"))
                {
                    if (!response.IsSuccessStatusCode) return -1;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var returnedResponse = JsonConvert.DeserializeObject<ReturnedOrderResponse>(responseBody);
                    return returnedResponse?.TotalOrdersReturned ?? -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching returned orders: {ex.Message}");
                return -1;
            }
        }

        public async Task<RevenueData> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                string formattedStart = startDate.ToString("yyyy-MM-dd");
                string formattedEnd = endDate.ToString("yyyy-MM-dd");
                string url = $"{BASE_URLL}/Revenues/revenue-by-date?startDate={formattedStart}&endDate={formattedEnd}";

                using (HttpResponseMessage response = await _httpClient.GetAsync(url))
                {
                    if (!response.IsSuccessStatusCode) return null;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RevenueData>(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching revenue by date: {ex.Message}");
                return null;
            }
        }
        //RevenueByLivestream
        public async Task<RevenueLivestream> GetRevenueByLivestream(string livestreamId)
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/livestream-revenue/{livestreamId}"))
                {
                    if (!response.IsSuccessStatusCode) return null;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RevenueLivestream>(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching revenue data: {ex.Message}");
                return null;
            }
        }

        public async Task<int> GetTotalOrdersByLivestreamIdAsync(string livestreamId)
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders-by-livestream/{livestreamId}"))
                {
                    if (!response.IsSuccessStatusCode) return -1;

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var cancelledResponse = JsonConvert.DeserializeObject<OrderLive>(responseBody);
                    return cancelledResponse?.totalOrders ?? -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching revenue data: {ex.Message}");
                return -1;
            }
        }

        public async Task<int> GetTotalOrdersCancelledByLivestreamIdAsync(string livestreamId)
        {
            var response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders-cancelled-by-livestream/{livestreamId}");
            if (!response.IsSuccessStatusCode)
                return 0;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CancelledOrderLive>(content);
            return result?.totalOrdersCancelled ?? 0;
        }

        public async Task<int> GetTotalOrdersReturnedByLivestreamIdAsync(string livestreamId)
        {
            var response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders-returned-by-livestream/{livestreamId}");
            if (!response.IsSuccessStatusCode)
                return 0;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReturnedOrderLive>(content);
            return result?.totalOrdersReturned ?? 0;
        }

        public async Task<int> GetTotalOrdersDeliveredByLivestreamIdAsync(string livestreamId)
        {
            var response = await _httpClient.GetAsync($"{BASE_URLL}/Revenues/total-orders-delivered-by-livestream/{livestreamId}");
            if (!response.IsSuccessStatusCode)
                return 0;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DeliveredOrderLive>(content);
            return result?.totalOrdersDelivered ?? 0;
        }


        public async Task<UserModels> GetUserProfileAsync()
        {
            string url = $"{BASE_URLL}/Auth/user-profile";
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

        public async Task<bool> IsLiveStreamStillLiveAsync(string livestreamId)
        {
            try
            {
                string url = $"{BASE_URLL}/LiveStreams/IsLiveStreamStillLive/{livestreamId}";

                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {

                    var response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        bool isLive = JsonConvert.DeserializeObject<bool>(content);
                        return isLive;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IsLiveStreamStillLiveAsync] Error: {ex.Message}");
                return false;
            }
        }


        //List Product
        public async Task<bool> SetupListProductAsync(string livestreamId, int listProductId)
        {
            var response = await _httpClient.PutAsync(
                $"{BASE_URLL}/ListProducts/AddListProductInToLiveStream/listProductID/{listProductId}/liveStreamID/{livestreamId}",
                null); 
                
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ListProductModel>> GetListProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BASE_URLL}/ListProducts/GetAllListProduct"); 

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API call failed: {response.StatusCode}");
                    return new List<ListProductModel>(); 
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ListProductModel>>(responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetListProductsAsync: {ex.Message}");
                return new List<ListProductModel>(); 
            }
        }

        public async Task<bool> AddNewListProductAsync(string listProductName)
        {
            var response = await _httpClient.PostAsync($"{BASE_URLL}/ListProducts/AddNewListProduct/{listProductName}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddMoreProductIntoListProductAsync(int listProductId, List<int> productIds)
        {
            try
            {
                var json = JsonConvert.SerializeObject(productIds);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BASE_URLL}/ListProducts/AddMoreProductIntoListProduct/{listProductId}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddMoreProductIntoListProductAsync] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteListProductAsync(int listProductId)
        {
            var response = await _httpClient.DeleteAsync($"{BASE_URLL}/ListProducts/DeleteListProduct/{listProductId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductsInListAsync(int listProductId, List<int> listProductIds)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{BASE_URLL}/ListProducts/DeleteProducInListProduct/{listProductId}"),
                Content = new StringContent(JsonConvert.SerializeObject(listProductIds), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }


        public async Task<List<ProductModel>> GetProductsFromListProductAsync(int listProductId)
        {
            var response = await _httpClient.GetAsync($"{BASE_URLL}/ListProducts/GetProductFromListProductById/{listProductId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API failed: {response.StatusCode}");
                return new List<ProductModel>(); 
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductModel>>(responseContent);
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

        public async Task<List<OrderModel>> GetOrdersByUserIdAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BASE_URLL}/Orders/User");
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
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
          
                
                var url = $"{BASE_URLL}/Orders/status/{orderId}";

                var content = new MultipartFormDataContent
                  {
                      { new StringContent(((int)status).ToString()), "status" }
                  };

                var response = await _httpClient.PutAsync(url, content);
                return response.IsSuccessStatusCode;

        }

        public async Task<bool> CheckListProductExistsAsync(string liveStreamId)
        {
            var url = $"{BASE_URLL}/ListProducts/GetExitListProductByLiveStream/LiveStreamID/{liveStreamId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return bool.Parse(json); 
            }
            return false;
        }


        //Update User
        public async Task<UserProfileUpdateResult> UpdateUserProfileRequestAsync(UserModels model)
        {
            var content = new MultipartFormDataContent
                {
                   { new StringContent(model.UserName ?? ""), "UserName" },
                   { new StringContent(model.PhoneNumber ?? ""), "PhoneNumber" },
                   { new StringContent(model.Email ?? ""), "Email" },
                   { new StringContent(model.Address ?? ""), "Address" },
                   { new StringContent(model.Gender ?? ""), "Gender" },
                   { new StringContent(model.Password ?? ""), "Password" }
                };

            var response = await _httpClient.PutAsync($"{BASE_URLL}/Auth/update-userProfile-request", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(responseContent);
                    return new UserProfileUpdateResult { Errors = errorResponse?.Errors };
                }

                var messageResponse = JsonConvert.DeserializeObject<SimpleMessageResponse>(responseContent);
                return new UserProfileUpdateResult { Message = messageResponse?.Message };
            }
            catch
            {
                return new UserProfileUpdateResult { Message = responseContent };
            }
        }


        public async Task<bool> VerifyOtpAndUpdateProfileAsync(VerifyOtpModel otpModel, string token)
        {
            try
            {
                var json = JsonConvert.SerializeObject(otpModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsync($"{BASE_URLL}/Auth/update-userProfie", content);

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

        public async Task<List<ProductModel>> GetAllProductsByUserAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BASE_URLL}/Products/GetAllProductsByUser");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProductModel>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetAllProductsByUserAsync] Error: {ex.Message}");
                return null;
            }
        }



        public async Task<ProductModelRequest> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"{BASE_URLL}/Products/GetProductId/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductModelRequest>(content);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error {response.StatusCode}: {error}");
            }
        }

        public async Task<ValidationErrorResponse> UpdateProductAsync(int productId, ProductModelRequest product, Stream imageStream, string fileName)
        {
            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(product.Name ?? ""), "name");
                    form.Add(new StringContent(product.ProductCode ?? ""), "productCode");
                    form.Add(new StringContent(product.Description ?? ""), "description");
                    form.Add(new StringContent(product.Price ?? ""), "price");
                    form.Add(new StringContent(product.Stock ?? ""), "stock");

                    if (imageStream != null && !string.IsNullOrEmpty(fileName))
                    {
                        var imageContent = new StreamContent(imageStream);
                        imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        form.Add(imageContent, "image", fileName);
                    }

                    var response = await _httpClient.PutAsync($"{BASE_URLL}/Products/updateProduct/{productId}", form);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return JsonConvert.DeserializeObject<ValidationErrorResponse>(responseContent);
                    }
                    else if (!response.IsSuccessStatusCode) 
                    {
                        return new ValidationErrorResponse
                        {
                            Message = responseContent
                        };
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateProductAsync: {ex.Message}");
                return new ValidationErrorResponse
                {
                    Message = "Unexpected error: " + ex.Message
                };
            }
        }


        public async Task<ValidationErrorResponse> AddProductAsync(ProductModelRequest product, Stream imageStream, string fileName)
        {
               var form = new MultipartFormDataContent
               {
                      { new StringContent(product.ProductCode ?? ""), "productCode" },
                      { new StringContent(product.Name), "name" },
                      { new StringContent(product.Description), "description" },
                      { new StringContent(product.Price.ToString()), "price" },
                      { new StringContent(product.Stock.ToString()), "stock" },
               };

            var imageContent = new StreamContent(imageStream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(imageContent, "image", fileName);

            var response = await _httpClient.PostAsync($"{BASE_URLL}/Products", form);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.BadRequest)
            {
                return JsonConvert.DeserializeObject<ValidationErrorResponse>(responseContent);
            }

            return null;
        }


        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BASE_URLL}/Products/DeleteProductById/{productId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DeleteProductAsync: {ex.Message}");
                return false;
            }
        }


    }
}