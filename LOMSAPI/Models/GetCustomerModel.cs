﻿using System.ComponentModel.DataAnnotations;

namespace LOMSAPI.Models
{
    public class GetCustomerModel
    {
        public string CustomerID { get; set; }
        [Required, MaxLength(255)]
        public string FacebookName { get; set; }
        public string? ImageURL { get; set; }

        public string? FullName { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public int SuccessfulDeliveries { get; set; } = 0;
        public int FailedDeliveries { get; set; } = 0;


    }
}
