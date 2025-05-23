﻿using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderFromComments(string liveStreamId, string TokenFacbook);
        Task<bool> OrderExistsAsync(int orderId);
        Task<bool> AddOrderAsync(string commentId, string TokenFacbook);
        Task<int> UpdateOrderAsync(OrderModel order);
        Task<int> UpdateStatusOrderAsync(int orderID, OrderStatus newStatus);
        Task<bool> PrinTest();
        Task<OrderByProductCodeModel> OrderByProductCodeModel (int LiveStreamCustomerID, int productID);
        Task<OrderByLiveStreamCustoemrModel> GetOrderByLiveStreamCustoemrModel(int LiveStreamCustomerID);
        Task<IEnumerable<OrderCustomerModel>> GetAllOrdersByUserIdAsync(string userID);
        Task<OrderCustomerModel?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderCustomerModel>> GetOrdersByCustomerIdAsync(string customerId);
        Task<IEnumerable<OrderCustomerModel>> GetOrdersByLiveStreamCustomerIdAsync(int liveStreamCustomerID);
        Task<IEnumerable<OrderCustomerModel>> GetAllOrdersByLiveStreamIdAsync(string liveStreamId);
        Task<IEnumerable<OrderCustomerModel>> GetAllOrdersAsync();
        Task<int> UpdateStatusCheckOrderAsync(int orderId, bool newStatusCheck);
        Task<int> UpdateStatusOrderByLiveStreamCustoemrAsync(int livestreamCustomerID, OrderStatus newStatus);
        Task<IEnumerable<OrderByLiveStreamCustoemrModel>> GetListOrderByLiveStreamCustoemrModel(string userID);
        Task<int> UpdateStatusCalldAsync(int livestreamCustomerID, bool statusCheck);
        Task<int> UpdateStatusTrackingNumberdAsync(int livestreamCustomerID, string trackingNumber, string note);
        Task<IEnumerable<OrderByLiveStreamCustoemrModel>> GetListOrderByLiveStreamCustoemrByCustoemrModel(string customerId);
        Task<IEnumerable<OrderByLiveStreamCustoemrModel>> GetListOrderByLiveStreamCustoemrLiveStremModel(string liveStreamId);
    }
}
