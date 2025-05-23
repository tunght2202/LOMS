﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMSUI.Models
{

    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Shipped = 2,
        Delivered = 3,
        Canceled = 4,
        Returned = 5
    }
    public class OrderModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } 
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public string CommentID { get; set; }
        public string? Note { get; set; }
        public decimal? CurrentPrice { get; set; }
        public bool StatusCheck { get; set; } = false;
        [StringLength(50, ErrorMessage = "TrackingNumber cannot exceed 50 characters.")]
        public string? TrackingNumber { get; set; }
        public string FacebookName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public ProductModel Product { get; set; }

    }
    public class ApiErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

}
