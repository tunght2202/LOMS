using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LOMSAPI.Repositories.ListProducts
{
    public class ListProductRepository : IListProductRepository
    {
        private readonly LOMSDbContext _context;

        public ListProductRepository(LOMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListProductModel>> GetAllListProduct(string userId)
        {
            var listProduct = await _context.ListProducts
                .Where(x => x.UserID.Equals(userId))
                .Select(x => new ListProductModel()
                {
                    ListProductId = x.ListProductId,
                    ListProductName = x.ListProductName
                }).ToListAsync();
            return listProduct;
        }

        public async Task<IEnumerable<ListProductModel>> GetListProductByName(string listProductName)
        {
            var listProduct = await _context.ListProducts
                .Where(x => x.ListProductName.ToLower().Contains(listProductName.ToLower()))
                .Select(x => new ListProductModel()
                {
                    ListProductId = x.ListProductId,
                    ListProductName = x.ListProductName
                })
                .ToListAsync();
            return listProduct;
        }

        public async Task<ListProduct> GetListProductById(int listProductId)
        {
            var result = await _context.ListProducts
                .FirstOrDefaultAsync(x => x.ListProductId == listProductId);

            if (result == null)
            {
                throw new Exception($"List product ID:{listProductId} not exit!");
            }
            return result;
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


        public async Task<bool> CheckExitListProductByName(string listProductName)
        {
            var result = await _context.ListProducts
                .AnyAsync(x => x.ListProductName.ToLower().Equals(listProductName.ToLower()));
            return result;
        }

        public async Task<bool> CheckExitListProductById(int listProductId)
        {
            var result = await _context.ListProducts
                .AnyAsync(x => x.ListProductId.Equals(listProductId));
            return result;
        }

        public async Task<int> AddNewListProduct(string listProductName, string userId)
        {
            if (listProductName == null)
            {
                throw new Exception("List product name can't null");
            }
            var listProduct = new ListProduct()
            {
                ListProductName = listProductName,
                UserID = userId
            };
            await _context.ListProducts.AddAsync(listProduct);
            return await _context.SaveChangesAsync();
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
                    Stock = plp.Product.Stock,
                    ImageURL = plp.Product.ImageURL                  
                })) // Lấy danh sách Product
                .ToListAsync();
            return productListProduct;
        }

        public async Task<int> AddProductIntoListProduct(int listProductId, List<int> listProduct)
        {
            var checkExitListProduct = await CheckExitListProductById(listProductId);
            if (!checkExitListProduct)
            {
                throw new Exception("This listProduct not exists!");
            }

            // Lấy danh sách ProductID đã tồn tại trong listProduct
            var existingProductIds = await _context.ProductListProducts
                .Where(plp => plp.ListProductID == listProductId)
                .Select(plp => plp.ProductID)
                .ToListAsync();

            var listproductlistproduct = new List<ProductListProduct>();

            foreach (var productId in listProduct)
            {
                // Chỉ thêm sản phẩm nếu chưa tồn tại
                if (!existingProductIds.Contains(productId))
                {
                    listproductlistproduct.Add(new ProductListProduct
                    {
                        ListProductID = listProductId,
                        ProductID = productId
                    });
                }
            }

            if (listproductlistproduct.Any())
            {
                await _context.AddRangeAsync(listproductlistproduct);
                return await _context.SaveChangesAsync();
            }

            return 0; // Không có gì để thêm
        }

        public async Task<int> DeleteProductOutListProduct(int listProductId, List<int> listProductIds)
        {
            if (listProductIds.Count == 0)
            {
                throw new Exception("list product null");
            }
            var listProductListProduct = new List<ProductListProduct>();
            foreach (var item in listProductIds)
            {
                var productListProduct = await _context.ProductListProducts
                    .FirstOrDefaultAsync(x => x.ListProductID == listProductId && x.ProductID == item);
                listProductListProduct.Add(productListProduct);
            }
            _context.ProductListProducts.RemoveRange(listProductListProduct);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteListProduct(int listProductId)
        {
            var relatedProducts = _context.ProductListProducts
    .Where(p => p.ListProductID == listProductId);
            _context.ProductListProducts.RemoveRange(relatedProducts);

            // Sau đó mới xóa ListProduct
            var listProduct = _context.ListProducts.Find(listProductId);
            if (listProduct != null)
            {
                _context.ListProducts.Remove(listProduct);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddListProductInToLiveStream(string liveStreamId, int listProductId, decimal maxPrice)
        {
            var liveStream = await _context.LiveStreams
                .FirstOrDefaultAsync(x => x.LivestreamID.Equals(liveStreamId));

            if (liveStream == null)
            {
                throw new Exception("This live stream does not exist.");
            }

            var listProduct = await _context.ListProducts
                .FirstOrDefaultAsync(lp => lp.ListProductId == listProductId);

            if (listProduct == null || listProductId == 0)
            {
                liveStream.ListProductID = null;
            }
            else
            {
                liveStream.ListProductID = listProductId;
                liveStream.PriceMax = maxPrice;
            }

            var result = await _context.SaveChangesAsync();
            return result;
        }

        public Task<bool> CheckListProductExitInLiveStream(string liveStreamId)
        {
            var result = _context.LiveStreams
                .AnyAsync(x => x.LivestreamID.Equals(liveStreamId) && x.ListProductID != null);
            return result;
        }
    }
}
