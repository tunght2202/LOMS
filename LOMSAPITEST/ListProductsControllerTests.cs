using System.Security.Claims;
using LOMSAPI.Controllers;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.ListProducts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace LOMSAPITEST
{
    public class ListProductsControllerTests
    {
        private readonly ListProductsController _controller;
        private readonly Mock<IListProductRepository> _mockListProductRepository;
        public ListProductsControllerTests()
        {
            _mockListProductRepository = new Mock<IListProductRepository>();
            _controller = new ListProductsController(_mockListProductRepository.Object);
            // Mock user claims for authenticated endpoints
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "test-user-id") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }
        [Fact]
        public async Task GetAllListProduct_ListProductsExist_ReturnsOkResult()
        {
            // Arrange
            var listProducts = new List<ListProductModel>
            {
                new ListProductModel { ListProductId = 1, ListProductName = "List 1" },
                new ListProductModel { ListProductId = 2, ListProductName = "List 2" }
            };
            // Replace the problematic line with the following:
            var userId = _controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _mockListProductRepository
                .Setup(repo => repo.GetAllListProduct(userId))
                .ReturnsAsync(listProducts);

            // Act
            var result = await _controller.GetAllListProduct();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<ListProductModel>>(serialized);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal(1, returnValue[0].ListProductId);
            Assert.Equal("List 1", returnValue[0].ListProductName);
            Assert.Equal(2, returnValue[1].ListProductId);
            Assert.Equal("List 2", returnValue[1].ListProductName);
        }

        [Fact]
        public async Task GetAllListProduct_NoListProducts_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = _controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _mockListProductRepository
                .Setup(repo => repo.GetAllListProduct(userId))
                .ReturnsAsync((List<ListProductModel>)null);

            // Act
            var result = await _controller.GetAllListProduct();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("not found any list product", notFoundResult.Value);
        }   

        [Fact]
        public async Task GetListProductByName_ListProductExists_ReturnsOkResult()
        {
            // Arrange
            var listProductName = "List 1";
            var listProducts = new List<ListProductModel>
    {
        new ListProductModel { ListProductId = 1, ListProductName = listProductName }
    };

            _mockListProductRepository
                .Setup(repo => repo.GetListProductByName(listProductName))
                .ReturnsAsync(listProducts);

            // Act
            var result = await _controller.GetListProductByName(listProductName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<ListProductModel>>(serialized);

            Assert.Single(returnValue);
            Assert.Equal(1, returnValue[0].ListProductId);
            Assert.Equal(listProductName, returnValue[0].ListProductName);
        }

        [Fact]
        public async Task GetListProductByName_ListProductNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var listProductName = "List 1";

            _mockListProductRepository
                .Setup(repo => repo.GetListProductByName(listProductName))
                .ReturnsAsync((IEnumerable<ListProductModel>)null);

            // Act

            // Act
            var result = await _controller.GetListProductByName(listProductName);


            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("not found any list product", notFoundResult.Value);
        }

        [Fact]
        public async Task GetListProductById_ListProductExists_ReturnsOkResult()
        {
            // Arrange
            var listProductId = 1;
            var listProduct = new ListProduct { ListProductId = listProductId, ListProductName = "List 1" };

            _mockListProductRepository
                .Setup(repo => repo.GetListProductById(listProductId))
                .ReturnsAsync(listProduct);

            // Act
            var result = await _controller.GetListProductById(listProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ListProductModel>(okResult.Value);
            Assert.Equal(listProductId, returnValue.ListProductId);
            Assert.Equal("List 1", returnValue.ListProductName);
        }

        [Fact]
        public async Task GetListProductById_ListProductNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var listProductId = 1;

            _mockListProductRepository
                .Setup(repo => repo.GetListProductById(listProductId))
                .ReturnsAsync((ListProduct)null);

            // Act
            var result = await _controller.GetListProductById(listProductId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("not found any list product", notFoundResult.Value);
        }

        [Fact]
        public async Task GetProductFromListProductById_ProductsExist_ReturnsOkResult()
        {
            // Arrange
            var listProductId = 1;
            var products = new List<ProductModel>
            {
                 new ProductModel
                {
                    ProductID = 101,
                    ProductCode = "P101",
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 10.99m,
                    Stock = 100,
                    Status = true,
                    ImageURL = "https://example.com/product1.jpg"
                },
                new ProductModel
                {
                    ProductID = 102,
                    ProductCode = "P102",
                    Name = "Product 2",
                    Description = "Description 2",
                    Price = 20.99m,
                    Stock = 50,
                    Status = false,
                    ImageURL = "https://example.com/product2.jpg"
                }
            };

            _mockListProductRepository
                .Setup(repo => repo.GetProductListProductById(listProductId))
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProductFromListProductById(listProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<ProductModel>>(serialized);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal(101, returnValue[0].ProductID);
            Assert.Equal("Product 1", returnValue[0].Name);
            Assert.Equal(102, returnValue[1].ProductID);
            Assert.Equal("Product 2", returnValue[1].Name);
        }

        [Fact]
        public async Task GetProductFromListProductById_NoProducts_ReturnsNotFoundResult()
        {
            // Arrange
            var listProductId = 1;

            _mockListProductRepository
                .Setup(repo => repo.GetProductListProductById(listProductId))
                .ReturnsAsync((List<ProductModel>)null);

            // Act
            var result = await _controller.GetProductFromListProductById(listProductId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("not found any list product", notFoundResult.Value);
        }

        [Fact]
        public async Task AddNewListProduct_Success_ReturnsOkResult()
        {
            // Arrange
            var listProductName = "New List";
            var userId = _controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _mockListProductRepository
                .Setup(repo => repo.AddNewListProduct(listProductName,userId))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddNewListProduct(listProductName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Add Sucessfull", okResult.Value);
        }

        [Fact]
        public async Task AddNewListProduct_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var listProductName = "New List";
            var userId = _controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _mockListProductRepository
                .Setup(repo => repo.AddNewListProduct(listProductName,userId))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.AddNewListProduct(listProductName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't create list product", badRequestResult.Value);
        }

        [Fact]
        public async Task AddMoreProductIntoListProduct_Success_ReturnsOkResult()
        {
            // Arrange
            var listProductId = 1;
            var listProduct = new List<int> { 101, 102 };

            _mockListProductRepository
                .Setup(repo => repo.AddProductIntoListProduct(listProductId, listProduct))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddMoreProductIntoListProduct(listProductId, listProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Add successfull", okResult.Value);
        }

        [Fact]
        public async Task AddMoreProductIntoListProduct_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var listProductId = 1;
            var listProduct = new List<int> { 101, 102 };

            _mockListProductRepository
                .Setup(repo => repo.AddProductIntoListProduct(listProductId, listProduct))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.AddMoreProductIntoListProduct(listProductId, listProduct);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't create list product", badRequestResult.Value);
        }

        [Fact]
        public async Task AddListProductInToLiveStream_Success_ReturnsOkResult()
        {
            // Arrange
            var listProductId = 1;
            var liveStreamID = "stream123";

            _mockListProductRepository
                .Setup(repo => repo.AddListProductInToLiveStream(listProductId, liveStreamID))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddListProductInToLiveStream(listProductId, liveStreamID);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Add successfull", okResult.Value);
        }

        [Fact]
        public async Task AddListProductInToLiveStream_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var listProductId = 1;
            var liveStreamID = "stream123";

            _mockListProductRepository
                .Setup(repo => repo.AddListProductInToLiveStream(listProductId, liveStreamID))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.AddListProductInToLiveStream(listProductId, liveStreamID);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't add list product into livestream", badRequestResult.Value);
        }
  
        [Fact]
        public async Task DeleteProducInListProduct_Success_ReturnsOkResult()
        {
            // Arrange
            var listProductId = 1;
            var listProductIds = new List<int> { 101, 102 };

            _mockListProductRepository
                .Setup(repo => repo.DeleteProductOutListProduct(listProductId, listProductIds))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteProducInListProduct(listProductId, listProductIds);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Delete successfull", okResult.Value);
        }

        [Fact]
        public async Task DeleteProducInListProduct_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var listProductId = 1;
            var listProductIds = new List<int> { 101, 102 };

            _mockListProductRepository
                .Setup(repo => repo.DeleteProductOutListProduct(listProductId, listProductIds))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.DeleteProducInListProduct(listProductId, listProductIds);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't delete list product", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteListProduct_Success_ReturnsOkResult()
        {
            // Arrange
            var listProductId = 1;

            _mockListProductRepository
                .Setup(repo => repo.DeleteListProduct(listProductId))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteListProduct(listProductId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Delete successfull", okResult.Value);
        }

        [Fact]
        public async Task DeleteListProduct_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var listProductId = 1;

            _mockListProductRepository
                .Setup(repo => repo.DeleteListProduct(listProductId))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.DeleteListProduct(listProductId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't delete list product", badRequestResult.Value);
        }
      
    }
}
