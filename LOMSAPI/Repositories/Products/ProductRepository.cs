using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LOMSAPI.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly LOMSDbContext _context;

        public ProductRepository(LOMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            var listProduct = await _context.Products.ToListAsync();

            var getProductList =  listProduct
                .Select(x => new ProductModel()
            {
                ProductID = x.ProductID,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock
            })
                .ToList();
            return getProductList;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var getProduct = await _context.Products.FindAsync(id);
            var product = new ProductModel()
            {
                ProductID = getProduct.ProductID,
                Price = getProduct.Price,
                Stock = getProduct.Stock,
                Description = getProduct.Description,
                Name = getProduct.Name,
                
            };
            return product;
        }


        public async Task<IEnumerable<ProductModel>> GetAllProductsByName(string name)
        {
            var getProducts = await _context.Products
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            var getProductModels = getProducts.Select(x => new ProductModel()
            {
                ProductID= x.ProductID,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                Stock = x.Stock
            }).ToList();
            return getProductModels;
        }
        public async Task<int> AddProduct(ProductModel postProduct)
        {
            var product = new Product()
            {
                Name = postProduct.Name,
                ProductID = 0,
                Price = postProduct.Price,
                Description = postProduct.Description,
                Stock = postProduct.Stock
            };
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
           
        }
        public async Task<int> UpdateProduct(int productId, ProductModel product)
        {
            var productById = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (productById == null) 
            {
                throw new Exception($"Can't find a product with id = {productId}");
            }
            productById.Name = product.Name;
            productById.Price = product.Price;
            productById.Description = product.Description;
            productById.Stock = product.Stock;

            return await _context.SaveChangesAsync();
        }
        //public async Task DeleteProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product != null)
        //    {
        //        _context.Products.Remove(product);
        //        await _context.SaveChangesAsync();
        //    }
        //}




    }
}
