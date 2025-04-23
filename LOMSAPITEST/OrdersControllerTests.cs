using LOMSAPI.Controllers;
using LOMSAPI.Data.Entities;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Orders;
using LOMSAPI.Repositories.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace LOMSAPITEST
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly OrdersController _controller;
        private readonly string _userId = "test-user-id";

        public OrdersControllerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCache = new Mock<IDistributedCache>();
            _mockUserRepository = new Mock<IUserRepository>();

            // Setup default user for most tests
            _mockUserRepository
                .Setup(repo => repo.GetUserById(_userId))
                .ReturnsAsync(new User { Id = _userId, TokenFacbook = "facebook-token" });

            // Create controller with mocked dependencies
            _controller = new OrdersController(
                _mockOrderRepository.Object,
                _mockCache.Object,
                _mockUserRepository.Object);

            // Set up user claims for authenticated endpoints
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, _userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task GetAll_OrdersExist_ReturnsOkResult()
        {
            // Arrange
            var orders = new List<OrderModel>
            {
                new OrderModel
                {
                    OrderID = 1,
                    OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                    Status = "Pending",
                    Quantity = 2,
                    ProductID = 101,
                    CommentID = "comment1",
                    Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
                },
                new OrderModel
                {
                    OrderID = 2,
                    OrderDate = new DateTime(2025, 4, 20, 12, 0, 0),
                    Status = "Confirmed",
                    Quantity = 1,
                    ProductID = 102,
                    CommentID = "comment2",
                    Product = new ProductModel { ProductID = 102, Name = "Product 2", Price = 20.99m }
                }
            };

            _mockOrderRepository
                .Setup(repo => repo.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<OrderModel>>(serialized);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal(1, returnValue[0].OrderID);
            Assert.Equal(new DateTime(2025, 4, 20, 10, 0, 0), returnValue[0].OrderDate);
            Assert.Equal("Pending", returnValue[0].Status);
            Assert.Equal(2, returnValue[0].Quantity);
            Assert.Equal(101, returnValue[0].ProductID);
            Assert.Equal("comment1", returnValue[0].CommentID);
            Assert.Equal(101, returnValue[0].Product.ProductID);
            Assert.Equal("Product 1", returnValue[0].Product.Name);
            Assert.Equal(10.99m, returnValue[0].Product.Price);
            Assert.Equal(2, returnValue[1].OrderID);
            Assert.Equal(new DateTime(2025, 4, 20, 12, 0, 0), returnValue[1].OrderDate);
            Assert.Equal("Confirmed", returnValue[1].Status);
            Assert.Equal(1, returnValue[1].Quantity);
            Assert.Equal(102, returnValue[1].ProductID);
            Assert.Equal("comment2", returnValue[1].CommentID);
            Assert.Equal(102, returnValue[1].Product.ProductID);
            Assert.Equal("Product 2", returnValue[1].Product.Name);
            Assert.Equal(20.99m, returnValue[1].Product.Price);
        }

        [Fact]
        public async Task GetAll_NoOrders_ReturnsNotFoundResult()
        {
            // Arrange
            _mockOrderRepository
                .Setup(repo => repo.GetAllOrdersAsync())
                .ReturnsAsync(new List<OrderModel>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No orders found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllByUserId_OrdersExist_ReturnsOkResult()
        {
            // Arrange
            var orders = new List<OrderModel>
            {
                new OrderModel
                {
                    OrderID = 1,
                    OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                    Status = "Pending",
                    Quantity = 2,
                    ProductID = 101,
                    CommentID = "comment1",
                    Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
                }
            };

            _mockOrderRepository
                .Setup(repo => repo.GetAllOrdersByUserIdAsync(_userId))
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAllByUserId();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<OrderModel>>(serialized);

            Assert.Single(returnValue);
            Assert.Equal(1, returnValue[0].OrderID);
            Assert.Equal(new DateTime(2025, 4, 20, 10, 0, 0), returnValue[0].OrderDate);
        }

        [Fact]
        public async Task GetAllByUserId_NoOrders_ReturnsNotFoundResult()
        {
            // Arrange
            _mockOrderRepository
                .Setup(repo => repo.GetAllOrdersByUserIdAsync(_userId))
                .ReturnsAsync(new List<OrderModel>());

            // Act
            var result = await _controller.GetAllByUserId();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No orders found for the user.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllByUserId_UnauthenticatedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            var controller = new OrdersController(
                _mockOrderRepository.Object,
                _mockCache.Object,
                _mockUserRepository.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetAllByUserId();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No orders found for the user.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllByLiveStreamId_OrdersExist_ReturnsOkResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var orders = new List<OrderModel>
            {
                new OrderModel
                {
                    OrderID = 1,
                    OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                    Status = "Pending",
                    Quantity = 2,
                    ProductID = 101,
                    CommentID = "comment1",
                    Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
                }
            };

            _mockOrderRepository
                .Setup(repo => repo.GetAllOrdersByLiveStreamIdAsync(liveStreamId))
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAllByLiveStreamId(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<OrderModel>>(serialized);

            Assert.Single(returnValue);
            Assert.Equal(1, returnValue[0].OrderID);
        }

        [Fact]
        public async Task GetAllByLiveStreamId_NoOrders_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream1";

            _mockOrderRepository
                .Setup(repo => repo.GetAllOrdersByLiveStreamIdAsync(liveStreamId))
                .ReturnsAsync(new List<OrderModel>());

            // Act
            var result = await _controller.GetAllByLiveStreamId(liveStreamId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No orders found for the livestream.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetByCustomerId_OrdersExist_ReturnsOkResult()
        {
            // Arrange
            var customerId = "cust1";
            var orders = new List<OrderModel>
            {
                new OrderModel
                {
                    OrderID = 1,
                    OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                    Status = "Pending",
                    Quantity = 2,
                    ProductID = 101,
                    CommentID = "comment1",
                    Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
                }
            };

            _mockOrderRepository
                .Setup(repo => repo.GetOrdersByCustomerIdAsync(customerId))
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetByCustomerId(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<OrderModel>>(serialized);

            Assert.Single(returnValue);
            Assert.Equal(1, returnValue[0].OrderID);
        }

        [Fact]
        public async Task GetByCustomerId_NoOrders_ReturnsNotFoundResult()
        {
            // Arrange
            var customerId = "cust1";

            _mockOrderRepository
                .Setup(repo => repo.GetOrdersByCustomerIdAsync(customerId))
                .ReturnsAsync(new List<OrderModel>());

            // Act
            var result = await _controller.GetByCustomerId(customerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No orders found for customer ID: {customerId}", notFoundResult.Value);
        }

        [Fact]
        public async Task GetById_OrderExists_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var order = new OrderModel
            {
                OrderID = orderId,
                OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "Pending",
                Quantity = 2,
                ProductID = 101,
                CommentID = "comment1",
                Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
            };

            _mockOrderRepository
                .Setup(repo => repo.GetOrderByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.GetById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OrderModel>(okResult.Value);
            Assert.Equal(orderId, returnValue.OrderID);
            Assert.Equal(new DateTime(2025, 4, 20, 10, 0, 0), returnValue.OrderDate);
            Assert.Equal("Pending", returnValue.Status);
            Assert.Equal(2, returnValue.Quantity);
            Assert.Equal(101, returnValue.ProductID);
            Assert.Equal("comment1", returnValue.CommentID);
            Assert.Equal(101, returnValue.Product.ProductID);
            Assert.Equal("Product 1", returnValue.Product.Name);
            Assert.Equal(10.99m, returnValue.Product.Price);
        }

        [Fact]
        public async Task GetById_OrderNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;

            _mockOrderRepository
                .Setup(repo => repo.GetOrderByIdAsync(orderId))
                .ReturnsAsync((OrderModel)null);

            // Act
            var result = await _controller.GetById(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Success_ReturnsOkResult()
        {
            // Arrange
            var commentId = "comment1";

            _mockOrderRepository
                .Setup(repo => repo.AddOrderAsync(commentId, "facebook-token"))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Create(commentId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Create_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var commentId = "comment1";

            _mockOrderRepository
                .Setup(repo => repo.AddOrderAsync(commentId, "facebook-token"))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Create(commentId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't create this order", badRequestResult.Value);
        }         

        [Fact]
        public async Task CreateOrderFromComments_Success_ReturnsOkResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var resultCount = 5;

            _mockOrderRepository
                .Setup(repo => repo.CreateOrderFromComments(liveStreamId, "facebook-token"))
                .ReturnsAsync(resultCount);

            // Act
            var result = await _controller.CreateOrderFromComments(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(resultCount, okResult.Value);
        }

        [Fact]
        public async Task CreateOrderFromComments_Failure_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream1";

            _mockOrderRepository
                .Setup(repo => repo.CreateOrderFromComments(liveStreamId, "facebook-token"))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.CreateOrderFromComments(liveStreamId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Can't create this order", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateOrderFromComments_NullLiveStreamId_ReturnsBadRequestResult()
        {
            // Arrange
            string liveStreamId = null;

            // Act
            var result = await _controller.CreateOrderFromComments(liveStreamId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("liveStreamId is null", badRequestResult.Value);
        }
      
        [Fact]
        public async Task Update_Success_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var orderModel = new OrderModel
            {
                OrderID = orderId,
                OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "Confirmed",
                Quantity = 3,
                ProductID = 101,
                CommentID = "comment1",
                Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
            };

            _mockOrderRepository
                .Setup(repo => repo.UpdateOrderAsync(orderModel))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.Update(orderId, orderModel);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Update_Failure_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            var orderModel = new OrderModel
            {
                OrderID = orderId,
                OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "Confirmed",
                Quantity = 3,
                ProductID = 101,
                CommentID = "comment1",
                Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
            };

            _mockOrderRepository
                .Setup(repo => repo.UpdateOrderAsync(orderModel))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.Update(orderId, orderModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Can't update this order", notFoundResult.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequestResult()
        {
            // Arrange
            var orderId = 1;
            var orderModel = new OrderModel
            {
                OrderID = 2, // Mismatch with orderId
                OrderDate = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "Confirmed",
                Quantity = 3,
                ProductID = 101,
                CommentID = "comment1",
                Product = new ProductModel { ProductID = 101, Name = "Product 1", Price = 10.99m }
            };

            // Act
            var result = await _controller.Update(orderId, orderModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateStatus_Success_ReturnsOkResult()
        {
            // Arrange
            var orderId = 1;
            var status = OrderStatus.Shipped;

            _mockOrderRepository
                .Setup(repo => repo.UpdateStatusOrderAsync(orderId, status))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.UpdateStatus(orderId, status);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateStatus_Failure_ReturnsNotFoundResult()
        {
            // Arrange
            var orderId = 1;
            var status = OrderStatus.Shipped;

            _mockOrderRepository
                .Setup(repo => repo.UpdateStatusOrderAsync(orderId, status))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.UpdateStatus(orderId, status);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Can't update status of this order", notFoundResult.Value);
        }  
      
    }
}