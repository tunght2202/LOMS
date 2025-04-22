using LOMSAPI.Data.Entities;
using LOMSAPI.Models;

namespace LOMSAPI.Repositories.ListProducts
{
    public interface IListProductRepository
    {
        Task<bool> CheckExitListProductByName (string listProductName);
        Task<bool> CheckExitListProductById (int listProductId);
        Task<bool> CheckListProductExitInLiveStream (String liveStreamId);
        Task<IEnumerable<ListProductModel>> GetAllListProduct( string userId);
        Task<IEnumerable<ListProductModel>> GetListProductByName(string listProductName);
        Task<ListProduct> GetListProductById(int listProductId);
        Task<IEnumerable<ProductModel>> GetProductListProductById(int listProductId);
        Task<int> AddNewListProduct(string listProductName, string userId);
        Task<int> AddProductIntoListProduct(int listProductId, List<int> listProduct);
        Task<int> AddListProductInToLiveStream(int listProductId, string liveStreamId);
        Task<int> DeleteProductOutListProduct(int listProductId, List<int> listProductIds);
        Task<int> DeleteListProduct(int listProductId);
    }
}
