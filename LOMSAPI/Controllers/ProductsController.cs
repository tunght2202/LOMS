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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetAllProducts());
        }

        // Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
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
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
            }
            return BadRequest("Create Producr Fail");
        }

        // Cập nhật sản phẩm
        [HttpPut("updateProduct/{id}")]
        public async Task<ActionResult> UpdateProduct(int id,[FromForm] ProductModel product)
        {
            var result = await _productRepository.UpdateProduct(id,product);
            if (result == 1)
            {
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
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
                var product = await _productRepository.GetProductById(id);
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
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

                var product = await _productRepository.GetProductById(id);

                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
            }
            return BadRequest("Update Price  Producr Fail");
        }

    }
}
