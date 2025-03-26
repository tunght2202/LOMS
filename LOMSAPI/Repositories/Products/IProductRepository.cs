using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.Products
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllProducts();
        Task<IEnumerable<ProductModel>> GetAllProductsByName(string name);
        Task<IEnumerable<ProductModel>> GetAllProductsByUser (string userId);
        Task<IEnumerable<ProductModel>> GetProductListProductById(int listProductId);
        Task<IEnumerable<ListProduct>> GetAllListProduct();
        Task<IEnumerable<ListProduct>> GetListProductByName(string listProductName);
        Task<ProductModel> GetProductById(int id);
//        Task<int> AddListProduct(ListProductModel listProduct);
        Task<int> AddProductListProduct(string listProductName, List<int> listProduct);
        Task<int> AddProduct(ProductModel product);
        Task<int> UpdateProduct(int productId, UpdateProductModel product);
        Task<int> UpdatePriceProduct(int productId, int price);
        Task<int> UpdateStockProduct(int productId, int reduceProduct);
        Task<int> DeleteProduct(int id);
    }
}
