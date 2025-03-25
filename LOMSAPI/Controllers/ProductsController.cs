using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Lấy danh sách sản phẩm
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetAllProducts());
        }

        // Lấy sản phẩm theo ID
        [HttpGet("GetProductId/{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }
            return Ok(product);
        }
        
        // Lấy sản phẩm theo LiveStream ID
        [HttpGet("GetProductByLiveStreamId/{liveStreamId}")]
        public async Task<ActionResult<ProductModel>> GetProductByLiveStreamId(string liveStreamId)
        {
            var product = await _productRepository.GetAllProductsByLiveStream(liveStreamId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }
            return Ok(product);
        }

        // Lấy sản phẩm theo User ID
        [HttpGet("GetAllProductsByUser/{userId}")]
        public async Task<ActionResult<ProductModel>> GetAllProductsByUser(string userId)
        {
            var product = await _productRepository.GetAllProductsByUser(userId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }
            return Ok(product);
        }

        // Thêm sản phẩm mới
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductModel product)
        {
            var result = await _productRepository.AddProduct(product);
            if (result == 1)
            {
                return Ok();
            }
            return BadRequest("Create Producr Fail");
        }

        // Cập nhật sản phẩm
        [HttpPut("updateProduct/{id}")]
        public async Task<ActionResult> UpdateProduct(int id,[FromForm] UpdateProductModel product)
        {
            var result = await _productRepository.UpdateProduct(id,product);
            if (result == 1)
            {
                return Ok();
            }
            return BadRequest("Update Producr Fail");
        }
        // Cập nhật số lượng hàng tồn kho sau khi order 
        [HttpPut("reduceStock/{id}/{reduceStockProduct}")]
        public async Task<ActionResult> UpdateStockProduct(int id,int reduceStockProduct)
        {
            var result = await _productRepository.UpdateStockProduct(id, reduceStockProduct);
            if (result == 1)
            {
                return Ok();
            }
            return BadRequest("Update Producr Fail");
        }

        // Cập nhật giá bán sản phẩm
        [HttpPut("updatePriceProduct/{id}/{newPrice}")]
        public async Task<ActionResult> UpdatePriceProduct(int id, int newPrice)
        {
            var result = await _productRepository.UpdatePriceProduct(id, newPrice);
            if (result == 1)
            {
                return Ok();
            }
            return BadRequest("Update Price  Product Fail");
        }

        // Xóa sản phẩm theo status
        [HttpDelete("DeleteProductById/{id}")]
        public async Task<ActionResult> DeleteProdutById(int id)
        {
            var result = await _productRepository.DeleteProduct(id);
            if(result == 1)
            {
                return Ok();
            }
            return BadRequest("Delete product Fail");
        }

    }
}
