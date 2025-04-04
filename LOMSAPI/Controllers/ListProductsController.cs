﻿using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.ListProducts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListProductsController : ControllerBase
    {
        private readonly IListProductRepository _context;

        public ListProductsController(IListProductRepository context)
        {
            _context = context;
        }
        // Thanh Tùng
        // Get all List Product 
        [HttpGet("GetAllListProduct")]
        public async Task<IActionResult> GetAllListProduct()
        {
            var listListProduct = await _context.GetAllListProduct();
            if (listListProduct == null)
            {
                return NotFound("not found any list product");
            }
            return Ok(listListProduct);
        }
        // Thanh Tùng
        // Get List Product by Name 

        [HttpGet("GetListProductByName/{listProductName}")]
        public async Task<IActionResult> GetListProductByName(string listProductName)
        {
            var listListProduct = await _context.GetListProductByName(listProductName);
            if (listListProduct == null)
            {
                return NotFound("not found any list product");
            }
            return Ok(listListProduct);
        }
        // Thanh Tùng
        // Get List Product By ID 

        [HttpGet("GetListProductById/{listProductId}")]
        public async Task<IActionResult> GetListProductById(int listProductId)
        {
            var listListProduct = await _context.GetListProductById(listProductId);
            if (listListProduct == null)
            {
                return NotFound("not found any list product");
            }
            var result = new ListProductModel()
            {
                ListProductId = listProductId,
                ListProductName = listListProduct.ListProductName
            };
            return Ok(result);
        }

        // Thanh Tùng
        // Get all List Product 
        [HttpGet("GetProductFromListProductById/{listProductId}")]
        public async Task<IActionResult> GetProductFromListProductById(int listProductId)
        {
            var listListProduct = await _context.GetProductListProductById(listProductId);
            if (listListProduct == null)
            {
                return NotFound("not found any list product");
            }
            return Ok(listListProduct);
        }

        // Thanh Tùng
        // Add new list product
        [HttpPost("AddNewListProduct/{listProductName}")]
        public async Task<IActionResult> AddNewListProduct(string listProductName)
        {
            var result = await _context.AddNewListProduct(listProductName);
            if (result == 0)
            {
                return BadRequest("Can't create list product");
            }
            return Ok("Add Sucessfull");
        }

        // Thanh Tùng
        // Get List Product into list product 
        [HttpPost("AddMoreProductIntoListProduct/{listProductId}")]
        public async Task<IActionResult> AddMoreProductIntoListProduct(int listProductId,[FromBody] List<int> listProduct)
        {
            var result = await _context.AddProductIntoListProduct(listProductId, listProduct);
            if (result == 0)
            {
                return BadRequest("Can't create list product");
            }
            return Ok("Add successfull");
        }

        // Thanh Tùng
        // Delete list product out List Product 
        [HttpDelete("DeleteProducInListProduct/{listProductId}")]
        public async Task<IActionResult> DeleteProducInListProduct(int listProductId, [FromBody] List<int> listProductIds)
        {
            var rerult = await _context.DeleteProductOutListProduct(listProductId, listProductIds);
            if (rerult == 0)
            {
                return BadRequest("Can't delete list product");
            }
            return Ok("Delete successfull");
        }

        // Thanh Tùng
        // Delete List Product 
        [HttpDelete("DeleteListProduct/{listProductId}")]
        public async Task<IActionResult> DeleteListProduct(int listProductId)
        {
            var rerult = await _context.DeleteListProduct(listProductId);
            if (rerult == 0)
            {
                return BadRequest("Can't delete list product");
            }
            return Ok("Delete successfull");
        }

    }
}
