using LOMSAPI.Controllers;
using LOMSAPI.Repositories.Revenues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace LOMSAPITEST;
public class RevenueControllerTests
{
    private readonly Mock<IRevenueRepository> _revenueRepositoryMock;
    private readonly RevenuesController _controller;

    public RevenueControllerTests()
    {
        _revenueRepositoryMock = new Mock<IRevenueRepository>();

        _controller = new RevenuesController(_revenueRepositoryMock.Object);

        // Giả lập HttpContext với Claims
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }
    private void SetupAuthenticatedUser(string userId)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }
   // [Fact]
   /* public async Task GetRevenueByDateRange_ValidDateRange_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 1, 31);
        var revenue = 5000.75m;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetRevenueByDateRange(userId, startDate, endDate))
            .ReturnsAsync(revenue);

        // Act
        var result = await _controller.GetRevenueByDateRange(startDate, endDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<dynamic>(okResult.Value);
        Assert.Equal(startDate, response.StartDate);
        Assert.Equal(endDate, response.EndDate);
        Assert.Equal(revenue, response.TotalRevenue);
        _revenueRepositoryMock.Verify(repo => repo.GetRevenueByDateRange(userId, startDate, endDate), Times.Once());
    }*/

    [Fact]
    public async Task GetRevenueByLivestreamId_ReturnsOkResult_WithRevenue()
    {
        _revenueRepositoryMock.Setup(repo => repo.GetRevenueByLivestreamId("test-user-id", "live1")).ReturnsAsync(250m);

        var result = await _controller.GetRevenueByLivestreamId("live1");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(250m, (decimal)obj["LiveStreamRevenue"]);
    }

    [Fact]
    public async Task GetRevenueByDateRange_ReturnsBadRequest_IfStartAfterEnd()
    {
        var result = await _controller.GetRevenueByDateRange(DateTime.Now, DateTime.Now.AddDays(-1));
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetRevenueByDateRange_ReturnsOkResult_WithRevenue()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 1, 31);
        _revenueRepositoryMock.Setup(repo => repo.GetRevenueByDateRange("test-user-id", start, end)).ReturnsAsync(500m);

        var result = await _controller.GetRevenueByDateRange(start, end);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(500m, (decimal)obj["TotalRevenue"]);
        Assert.Equal(start, (DateTime)obj["StartDate"]);
        Assert.Equal(end, (DateTime)obj["EndDate"]);
    }

    [Fact]
    public async Task GetRevenueByDateRange_ValidDateRange_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 1, 31);
        var revenue = 5000.75m;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetRevenueByDateRange(userId, startDate, endDate))
            .ReturnsAsync(revenue);

        // Act
        var result = await _controller.GetRevenueByDateRange(startDate, endDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(5000.75m, (decimal)obj["TotalRevenue"]);
        Assert.Equal(startDate, (DateTime)obj["StartDate"]);
        Assert.Equal(endDate, (DateTime)obj["EndDate"]);
    }

    [Fact]
    public async Task GetRevenueByDateRange_InvalidDateRange_ReturnsBadRequest()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2025, 1, 31);
        var endDate = new DateTime(2025, 1, 1);
        SetupAuthenticatedUser(userId);

        // Act
        var result = await _controller.GetRevenueByDateRange(startDate, endDate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Start date must be before end date.", badRequestResult.Value);
        _revenueRepositoryMock.Verify(repo => repo.GetRevenueByDateRange(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
    }

    [Fact]
    public async Task GetTotalOrdersByLivestreamId_AuthenticatedUser_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var livestreamId = "live456";
        var totalOrders = 50;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetTotalOrderByLivestreamId(userId, livestreamId))
            .ReturnsAsync(totalOrders);

        // Act
        var result = await _controller.GetTotalOrdersByLivestreamId(livestreamId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(50, (decimal)obj["TotalOrders"]);
    }

    [Fact]
    public async Task GetTotalOrdersCancelledByLivestreamId_AuthenticatedUser_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var livestreamId = "live456";
        var totalCancelled = 10;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetTotalOrederCancelledByLivestreamId(userId, livestreamId))
            .ReturnsAsync(totalCancelled);

        // Act
        var result = await _controller.GetTotalOrdersCancelledByLivestreamId(livestreamId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(10, (decimal)obj["TotalOrdersCancelled"]);
    }

 

    [Fact]
    public async Task GetTotalOrdersDeliveredByLivestreamId_AuthenticatedUser_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var livestreamId = "live456";
        var totalDelivered = 45;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetTotalOrederDeliveredByLivestreamId(userId, livestreamId))
            .ReturnsAsync(totalDelivered);

        // Act
        var result = await _controller.GetTotalOrdersDeliveredByLivestreamId(livestreamId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(45, (decimal)obj["TotalOrdersDelivered"]);
    }

    [Fact]
    public async Task GetTotalOrdersByDateRange_ValidDateRange_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 1, 31);
        var totalOrders = 200;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetTotalOrdersByDateRange(userId, startDate, endDate))
            .ReturnsAsync(totalOrders);

        // Act
        var result = await _controller.GetTotalOrdersByDateRange(startDate, endDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(200, (decimal)obj["TotalOrders"]);
        Assert.Equal(startDate, (DateTime)obj["StartDate"]);
        Assert.Equal(endDate, (DateTime)obj["EndDate"]);
    }

    [Fact]
    public async Task GetTotalOrdersCancelledByDateRange_ValidDateRange_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 1, 31);
        var totalCancelled = 20;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetTotalOrederCancelledByDateRange(userId, startDate, endDate))
            .ReturnsAsync(totalCancelled);

        // Act
        var result = await _controller.GetTotalOrdersCancelledByDateRange(startDate, endDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(20, (decimal)obj["TotalOrdersCancelled"]);
        Assert.Equal(startDate, (DateTime)obj["StartDate"]);
        Assert.Equal(endDate, (DateTime)obj["EndDate"]);
    }

    [Fact]
    public async Task GetTotalOrdersReturnedByDateRange_ValidDateRange_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 1, 31);
        var totalReturned = 15;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetTotalOrederReturnedByDateRange(userId, startDate, endDate))
            .ReturnsAsync(totalReturned);

        // Act
        var result = await _controller.GetTotalOrdersReturnedByDateRange(startDate, endDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(15, (decimal)obj["TotalOrdersReturned"]);
        Assert.Equal(startDate, (DateTime)obj["StartDate"]);
        Assert.Equal(endDate, (DateTime)obj["EndDate"]);
    }

    [Fact]
    public async Task GetTotalOrdersDeliveredByDateRange_ValidDateRange_ReturnsOk()
    {
        // Arrange
        var userId = "user123";
        var startDate = new DateTime(2025, 1, 1);
        var endDate = new DateTime(2025, 1, 31);
        var totalDelivered = 180;
        SetupAuthenticatedUser(userId);
        _revenueRepositoryMock.Setup(repo => repo.GetTotalOrederDelivered(userId, startDate, endDate))
            .ReturnsAsync(totalDelivered);

        // Act
        var result = await _controller.GetTotalOrdersDeliveredByDateRange(startDate, endDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(180, (decimal)obj["TotalOrdersDelivered"]);
        Assert.Equal(startDate, (DateTime)obj["StartDate"]);
        Assert.Equal(endDate, (DateTime)obj["EndDate"]);
    }

}
