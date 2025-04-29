using System.Security.Claims;
using LOMSAPI.Controllers;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LOMSAPITEST
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly List<ProductModel> _sampleProducts;
        public ProductsControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _controller = new ProductsController(_productRepositoryMock.Object);

            // Setup sample products
            _sampleProducts = new List<ProductModel>
        {
            new ProductModel { ProductID = 1, Name = "Product 1", Price = 10.99m, Stock = 100 },
            new ProductModel { ProductID = 2, Name = "Product 2", Price = 20.99m, Stock = 50 }
        };

            // Setup user claims for testing authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetProducts_ReturnsOkWithProducts()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetAllProducts())
                .ReturnsAsync(_sampleProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<ProductModel>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count());
        }

        [Fact]
        public async Task GetProduct_ExistingId_ReturnsOkWithProduct()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = _sampleProducts[0];
            _productRepositoryMock.Setup(repo => repo.GetProductById(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProduct = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal(expectedProduct.ProductID, returnProduct.ProductID);
        }

        [Fact]
        public async Task GetProduct_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((ProductModel)null);

            // Act
            var result = await _controller.GetProduct(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Sản phẩm không tồn tại.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllProductsByUser_ValidUser_ReturnsOkWithProducts()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetAllProductsByUser("test-user-id"))
                .ReturnsAsync(_sampleProducts);

            // Act
            var result = await _controller.GetAllProductsByUser();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<ProductModel>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count());
        }

        [Fact]
        public async Task GetAllProductsByUser_NoUserId_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.GetAllProductsByUser();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal("Missing user info", unauthorizedResult.Value);
        }

        [Fact]
        public async Task AddProduct_ValidInput_ReturnsOk()
        {
            // Arrange
            var productModel = new PostProductModel();
            IFormFile image = null;
            _productRepositoryMock.Setup(repo => repo.AddProduct(productModel, image, "test-user-id"))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.AddProduct(productModel, image);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddProduct_Failure_ReturnsBadRequest()
        {
            // Arrange
            var productModel = new PostProductModel();
            IFormFile image = null;
            _productRepositoryMock.Setup(repo => repo.AddProduct(productModel, image, "test-user-id"))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.AddProduct(productModel, image);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Create Producr Fail", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ValidInput_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var productModel = new PutProductModel { Name = "Updated Product", Price = 99.99m };
            var image = (IFormFile?)null;
            _productRepositoryMock.Setup(repo => repo.UpdateProduct(productId, productModel, image))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.UpdateProduct(productId, productModel, image);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _productRepositoryMock.Verify(repo => repo.UpdateProduct(productId, productModel, image), Times.Once());
        }

        [Fact]
        public async Task UpdateStockProduct_ValidInput_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var reduceStock = 10;
            _productRepositoryMock.Setup(repo => repo.UpdateStockProduct(productId, reduceStock))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.UpdateStockProduct(productId, reduceStock);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdatePriceProduct_ValidInput_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var newPrice = 99;
            _productRepositoryMock.Setup(repo => repo.UpdatePriceProduct(productId, newPrice))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.UpdatePriceProduct(productId, newPrice);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteProductById_ValidId_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            _productRepositoryMock.Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteProdutById(productId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteProductById_Failure_ReturnsBadRequest()
        {
            // Arrange
            var productId = 1;
            _productRepositoryMock.Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.DeleteProdutById(productId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Delete product Fail", badRequestResult.Value);
        }
    }
}

