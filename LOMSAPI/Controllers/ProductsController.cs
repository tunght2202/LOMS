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

        // Thanh Tùng
        // Get all Product 
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetAllProducts());
        }

        // Thanh Tùng
        // Get Product by ID 
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


        // Thanh Tùng
        // Get List Product By User 
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

        // Thanh Tùng
        // Add new product 
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductModel product, IFormFile image)
        {
            var result = await _productRepository.AddProduct(product, image);
            if (result == 1)
            {
                return Ok();
            }
            return BadRequest("Create Producr Fail");
        }

        // Thanh Tùng
        // Update product by ID 
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
        // Thanh Tùng
        // Update stock after customer order 
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

        // Thanh Tùng
        // Update Price Product by id 
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
        // Thanh Tùng
        // Delete product by id using status 
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
