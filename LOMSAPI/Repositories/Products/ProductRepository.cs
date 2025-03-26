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

            var getProductList = listProduct
                .Select(x => new ProductModel()
                {
                    ProductID = x.ProductID,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    Stock = x.Stock,
                    Status = x.Status
                })
                .ToList();
            return getProductList;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var getProduct = await _context.Products.FindAsync(id);
            if (getProduct == null) { throw new Exception($"This id {id} invite "); }

            var product = new ProductModel()
            {
                ProductID = getProduct.ProductID,
                Price = getProduct.Price,
                Stock = getProduct.Stock,
                Description = getProduct.Description,
                Name = getProduct.Name,
                Status = getProduct.Status

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
                ProductID = x.ProductID,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                Stock = x.Stock,
                Status = x.Status
            }).ToList();
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
            var productList =  getProductByUser.Select(x => new ProductModel()
            {
                ProductID = x.ProductID,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                Stock = x.Stock,
                Status = x.Status
            }).ToList();
            return productList;
        }

        public async Task<int> AddProduct(ProductModel postProduct)
        {
            var product = new Product()
            {
                Name = postProduct.Name,
                UserID = postProduct.UserID,
                ProductID = 0,
                Price = postProduct.Price,
                Description = postProduct.Description,
                Stock = postProduct.Stock,
                Status = postProduct.Status
            };
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();

        }
        public async Task<int> UpdateProduct(int productId, UpdateProductModel product)
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

        public async Task<IEnumerable<ProductModel>> GetProductListProductById(int ListProductId)
        {
            var productListProduct = await _context.ListProducts
                .Where(lp => lp.ListProductId == ListProductId)
                .Include(lp => lp.ProductListProducts)
                .ThenInclude(plp => plp.Product)
                .SelectMany(lp => lp.ProductListProducts
                .Select(plp => new ProductModel()
                {
                    ProductID = plp.ProductID,
                    Name = plp.Product.Name,
                    Price = plp.Product.Price,
                    Description = plp.Product.Description,
                    ProductCode = plp.Product.ProductCode,
                    Status = plp.Product.Status,
                    Stock = plp.Product.Stock
                })) // Lấy danh sách Product
                .ToListAsync();
            return productListProduct;
        }

        public async Task<IEnumerable<ListProduct>> GetAllListProduct()
        {
            var listProduct = await _context.ListProducts.ToListAsync();
            return listProduct;
        }

        public async Task<IEnumerable<ListProduct>> GetListProductByName(string listProductName)
        {
            var listProduct = await _context.ListProducts
                .Where(x => x.ListProductName.ToLower().Contains(listProductName.ToLower()))
                .ToListAsync();
            return listProduct;
        }

        public async Task<int> AddProductListProduct(string listProductName, List<int> listProduct)
        {
            var checkExit = await _context.ListProducts
                .AnyAsync(x => x.ListProductName.ToLower()
                .Equals(listProductName.ToLower()));
            if (checkExit)
            {
                throw new Exception($"{listProductName} exit");
            }
            return 1;
        }
    }
}
