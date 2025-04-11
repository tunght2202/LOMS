﻿using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace LOMSAPI.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly LOMSDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public ProductRepository(LOMSDbContext context
            , CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        private ProductModel MapToModel(Product product)
        {
            return new ProductModel
            {
                ProductID = product.ProductID,
                ProductCode = product.ProductCode,
                Description = product.Description,
                UserID = product.UserID,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Status = product.Status,
                ImageURL = product.ImageURL
            };
        }

        private Product MapToEntity(ProductModel product)
        {
            return new Product
            {
                ProductID = product.ProductID,
                ProductCode = product.ProductCode,
                Description = product.Description,
                UserID = product.UserID,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Status = product.Status,
                ImageURL = product.ImageURL
            };
        }
        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            var listProduct = await _context.Products.ToListAsync();

            var getProductList = listProduct.ToList();
            return getProductList.Select(x => MapToModel(x));
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var getProduct = await _context.Products.FindAsync(id);
            if (getProduct == null) { throw new Exception($"This id {id} invite "); }

            var productModel = MapToModel(getProduct);
            return productModel;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsByName(string name)
        {
            var getProducts = await _context.Products
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            var getProductModels = getProducts.Select(x => MapToModel(x)).ToList();
            return getProductModels;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsByUser(string userId)
        {
            var getUser = await _context.Users
                .FindAsync(userId);
            if (getUser == null) { throw new Exception("this user not exit!"); }
            var getProductByUser = await _context.Products
                .Where( p => p.UserID == userId)
                .ToListAsync();
            var productList =  getProductByUser.Select(x => MapToModel(x)).ToList();
            return productList;
        }

        public async Task<int> AddProduct(PostProductModel postProduct, IFormFile imge)
        {
            if (postProduct == null)
            {
                throw new Exception("Product can't null");
            }
            string imageUrl = await _cloudinaryService.UploadImageAsync(imge);
            postProduct.ImageURL = imageUrl;
            var product = new Product()
            {
                ProductCode = postProduct.ProductCode,
                Description = postProduct.Description,
                UserID = postProduct.UserID,
                Name = postProduct.Name,
                Price = postProduct.Price,
                Stock = postProduct.Stock,
                Status = postProduct.Status,
                ImageURL = postProduct.ImageURL
            };
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();

        }
        public async Task<int> UpdateProduct(int productId, PutProductModel product)
        {
            var productById = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (productById == null)
            {
                throw new Exception($"Can't find a product with id = {productId}");
            }
            productById.Name = product.Name;
            productById.ProductCode = product.ProductCode;
            productById.Price = product.Price;
            productById.Description = product.Description;
            productById.Stock = product.Stock;
            productById.ImageURL = product.ImageURL;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdatePriceProduct(int productId, int price)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);

            if (product == null)
            {
                throw new Exception($"Can't find a product with id = {productId}");
            }

            product.Price = price;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStockProduct(int productId, int reduceProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);

            if (product == null)
            {
                throw new Exception($"Can't find a product with id = {productId}");
            }
            var stock = product.Stock;

            if (stock < reduceProduct)
            {
                throw new Exception("stock not enough !");
            }

            if (reduceProduct < 0)
            {
                throw new Exception("reduce product can't is  negative number");
            }

            product.Stock = product.Stock - reduceProduct;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception($"Can't find product have id is {id}");
            }
            product.Status = false; return await _context.SaveChangesAsync();
        }


    }
}
