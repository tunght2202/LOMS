using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [HttpGet("GetAllProductsByUser")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProductsByUser()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized("Missing user info");

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
        public async Task<ActionResult> AddProduct([FromForm] PostProductModel product,IFormFile image)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _productRepository.AddProduct(product, image, userId);
            if (result == 1)
            {
                return Ok();
            }
            return BadRequest("Create Producr Fail");
        }

        // Thanh Tùng
        // Update product by ID 
        [HttpPut("updateProduct/{id}")]
        public async Task<ActionResult> UpdateProduct(int id,[FromForm] PutProductModel product)
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
