using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Products
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
        Task<IEnumerable<ProductModel>> GetAllProductsByName(string name);
        Task<ProductModel> GetProductById(int id);
        Task<int> AddProduct(ProductModel product);
        Task<int> UpdateProduct(int productId, ProductModel product);
        //Task DeleteProduct(int id);
    }
}
