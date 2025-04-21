using System.Security.Claims;
using LOMSAPI.Controllers;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace LOMSAPITEST
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();

            _controller = new CustomersController(_mockCustomerRepository.Object);

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
        public async Task GetCustomerById_CustomerExists_ReturnsOkResult()
        {
            // Arrange
            var customerId = "customer123";
            var customer = new GetCustomerModel
            {
                CustomerID = customerId,
                FacebookName = "Customer Name",
                ImageURL = "https://example.com/image.jpg",
                FullName = "John Doe",
                PhoneNumber = "1234567890",
                Email = "customer@example.com",
                Address = "123 Main St",
                SuccessfulDeliveries = 5,
                FailedDeliveries = 1
            };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerById(customerId))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerById(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetCustomerModel>(okResult.Value);
            Assert.Equal(customerId, returnValue.CustomerID);
            Assert.Equal("Customer Name", returnValue.FacebookName);
            Assert.Equal("https://example.com/image.jpg", returnValue.ImageURL);
            Assert.Equal("John Doe", returnValue.FullName);
            Assert.Equal("1234567890", returnValue.PhoneNumber);
            Assert.Equal("customer@example.com", returnValue.Email);
            Assert.Equal("123 Main St", returnValue.Address);
            Assert.Equal(5, returnValue.SuccessfulDeliveries);
            Assert.Equal(1, returnValue.FailedDeliveries);
        }

        [Fact]
        public async Task GetCustomerById_CustomerNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var customerId = "customer123";

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerById(customerId))
                .ReturnsAsync((GetCustomerModel)null);

            // Act
            var result = await _controller.GetCustomerById(customerId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{customerId} not exit", badRequestResult.Value);
        }

        [Fact]
        public async Task GetByOrderID_CustomerExists_ReturnsOkResult()
        {
            // Arrange
            var orderId = 123;
            var customer = new GetCustomerModel
            {
                CustomerID = "customer123",
                FacebookName = "Customer Name",
                SuccessfulDeliveries = 5,
                FailedDeliveries = 1
            };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByOrderIdAsync(orderId))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetByOrderID(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetCustomerModel>(okResult.Value);
            Assert.Equal("customer123", returnValue.CustomerID);
            Assert.Equal("Customer Name", returnValue.FacebookName);
            Assert.Equal(5, returnValue.SuccessfulDeliveries);
            Assert.Equal(1, returnValue.FailedDeliveries);
        }

        [Fact]
        public async Task GetByOrderID_CustomerNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var orderId = 123;

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomerByOrderIdAsync(orderId))
                .ReturnsAsync((GetCustomerModel)null);

            // Act
            var result = await _controller.GetByOrderID(orderId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{orderId} not exit", badRequestResult.Value);
        }

        [Fact]
        public async Task GetByUserID_CustomersExist_ReturnsOkResult()
        {
            // Arrange
            var userId = "user123";
            var customers = new List<GetCustomerModel>
    {
        new GetCustomerModel { CustomerID = "customer1", FacebookName = "Customer 1", SuccessfulDeliveries = 3, FailedDeliveries = 0 },
        new GetCustomerModel { CustomerID = "customer2", FacebookName = "Customer 2", SuccessfulDeliveries = 2, FailedDeliveries = 1 }
    };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomersByUserIdAsync(userId))
                .ReturnsAsync(customers);

            // Mock ClaimsPrincipal with userId
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.GetByUserID();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<GetCustomerModel>>(serialized);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal("customer1", returnValue[0].CustomerID);
            Assert.Equal("Customer 1", returnValue[0].FacebookName);
            Assert.Equal(3, returnValue[0].SuccessfulDeliveries);
            Assert.Equal(0, returnValue[0].FailedDeliveries);
            Assert.Equal("customer2", returnValue[1].CustomerID);
            Assert.Equal("Customer 2", returnValue[1].FacebookName);
            Assert.Equal(2, returnValue[1].SuccessfulDeliveries);
            Assert.Equal(1, returnValue[1].FailedDeliveries);
        }

        [Fact]
        public async Task GetByUserID_CustomersNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = "user123";

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomersByUserIdAsync(userId))
                .ReturnsAsync((List<GetCustomerModel>?)null); // explicitly return null

            // Mock ClaimsPrincipal with userId
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.GetByUserID();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{userId} not exit", badRequestResult.Value);
        }

        [Fact]
        public async Task GetByLiveStreamID_CustomersExist_ReturnsOkResult()
        {
            // Arrange
            var liveStreamId = "stream123";
            var customers = new List<GetCustomerModel>
            {
                new GetCustomerModel { CustomerID = "customer1", FacebookName = "Customer 1", SuccessfulDeliveries = 3, FailedDeliveries = 0 },
                new GetCustomerModel { CustomerID = "customer2", FacebookName = "Customer 2", SuccessfulDeliveries = 2, FailedDeliveries = 1 }
            };

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomersByLiveStreamIdAsync(liveStreamId))
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetByLiveStreamID(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<GetCustomerModel>>(serialized);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal("customer1", returnValue[0].CustomerID);
            Assert.Equal("Customer 1", returnValue[0].FacebookName);
            Assert.Equal(3, returnValue[0].SuccessfulDeliveries);
            Assert.Equal(0, returnValue[0].FailedDeliveries);
            Assert.Equal("customer2", returnValue[1].CustomerID);
            Assert.Equal("Customer 2", returnValue[1].FacebookName);
            Assert.Equal(2, returnValue[1].SuccessfulDeliveries);
            Assert.Equal(1, returnValue[1].FailedDeliveries);
        }

        [Fact]
        public async Task GetByLiveStreamID_CustomersNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var liveStreamId = "stream123";

            _mockCustomerRepository
                .Setup(repo => repo.GetCustomersByLiveStreamIdAsync(liveStreamId))
                .ReturnsAsync((List<GetCustomerModel>)null);

            // Act
            var result = await _controller.GetByLiveStreamID(liveStreamId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"{liveStreamId} not exit", badRequestResult.Value);
        }

        [Fact]
        public async Task AddCustomer_Success_ReturnsOkResult()
        {
            // Arrange
            var customerId = "customer123";
            var customerFacebookName = "Customer Name";

            _mockCustomerRepository
                .Setup(repo => repo.AddCustomer(customerId, customerFacebookName))
                .ReturnsAsync(1); // Giả lập thêm thành công

            // Act
            var result = await _controller.AddCustomer(customerId, customerFacebookName);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddCustomer_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var customerId = "customer123";
            var customerFacebookName = "Customer Name";

            _mockCustomerRepository
                .Setup(repo => repo.AddCustomer(customerId, customerFacebookName))
                .ReturnsAsync(0); // Giả lập thêm thất bại

            // Act
            var result = await _controller.AddCustomer(customerId, customerFacebookName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Can't add {customerFacebookName} customer!", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCustomerByID_Success_ReturnsOkResult()
        {
            // Arrange
            var customerId = "customer123";
            var customerUpdate = new UpdateCustomerModel { FacebookName = "Updated Name" };

            _mockCustomerRepository
                .Setup(repo => repo.UpdateCustomer(customerId, customerUpdate))
                .ReturnsAsync(1); // Giả lập cập nhật thành công

            // Act
            var result = await _controller.UpdateCustomerByID(customerId, customerUpdate);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateCustomerByID_Failure_ReturnsBadRequestResult()
        {
            // Arrange
            var customerId = "customer123";
            var customerUpdate = new UpdateCustomerModel { FacebookName = "Updated Name" };

            _mockCustomerRepository
                .Setup(repo => repo.UpdateCustomer(customerId, customerUpdate))
                .ReturnsAsync(0); // Giả lập cập nhật thất bại

            // Act
            var result = await _controller.UpdateCustomerByID(customerId, customerUpdate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't update customer!", badRequestResult.Value);
        }


        [Fact]
        public async Task UpdateCustomerByID_InvalidModelState_ReturnsBadRequestResult()
        {
            // Arrange
            var customerId = "customer123";
            var customerUpdate = new UpdateCustomerModel { FacebookName = null };

            _controller.ModelState.AddModelError("FacebookName", "FacebookName is required");

            // Act
            var result = await _controller.UpdateCustomerByID(customerId, customerUpdate);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
