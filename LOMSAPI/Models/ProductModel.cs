﻿namespace LOMSAPI.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
