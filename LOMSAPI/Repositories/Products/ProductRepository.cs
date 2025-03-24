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
                LiveStreamID = x.LiveStreamID,
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
                LiveStreamID = getProduct.LiveStreamID,
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
                ProductID= x.ProductID,
                LiveStreamID = x.LiveStreamID,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                Stock = x.Stock,
                Status = x.Status
            }).ToList();
            return getProductModels;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsByLiveStream(string liveStreamId)
        {
            var getLiveStreamId = await _context.LiveStreams
                .FirstOrDefaultAsync(x => x.LivestreamID.Equals(liveStreamId));
            if(getLiveStreamId == null)
            {
                throw new Exception($"Can't find livestream id {liveStreamId}");
            }
            var getProductByLiveStream = await _context.Products
                .Where(p => p.LiveStreamID.Equals(liveStreamId))
                .ToListAsync();
            var getProductModels = getProductByLiveStream.Select(x => new ProductModel()
            {
                ProductID = x.ProductID,
                LiveStreamID = x.LiveStreamID,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                Stock = x.Stock,
                Status = x.Status
            }).ToList();
            return getProductModels;
        }
        public async Task<int> AddProduct(ProductModel postProduct)
        {
            var product = new Product()
            {
                Name = postProduct.Name,
                LiveStreamID = postProduct.LiveStreamID,
                ProductID = 0,
                Price = postProduct.Price,
                Description = postProduct.Description,
                Stock = postProduct.Stock,
                Status = postProduct.Status
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
            productById.LiveStreamID = product.LiveStreamID;
            productById.Price = product.Price;
            productById.Description = product.Description;
            productById.Stock = product.Stock;
            productById.Status = product.Status;

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

            if(reduceProduct < 0)
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
