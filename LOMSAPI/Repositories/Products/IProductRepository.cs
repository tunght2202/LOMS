using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Products
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
        Task<IEnumerable<ProductModel>> GetAllProductsByName(string name);
        Task<IEnumerable<ProductModel>> GetAllProductsByUser (string userId);
        Task<ProductModel> GetProductById(int id);
        Task<int> AddProduct(PostProductModel product, IFormFile image, string userId);
        Task<int> UpdateProduct(int productId, PutProductModel product);
        Task<int> UpdatePriceProduct(int productId, int price);
        Task<int> UpdateStockProduct(int productId, int reduceProduct);
        Task<int> DeleteProduct(int id);
    }
}
