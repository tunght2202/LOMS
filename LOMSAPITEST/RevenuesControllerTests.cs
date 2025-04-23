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
    private readonly Mock<IRevenueRepository> _mockRepo;
    private readonly RevenuesController _controller;

    public RevenueControllerTests()
    {
        _mockRepo = new Mock<IRevenueRepository>();

        _controller = new RevenuesController(_mockRepo.Object);

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

    [Fact]
    public async Task GetTotalOrders_ReturnsOkResult_WithTotalOrders()
    {
        _mockRepo.Setup(repo => repo.GetTotalOrders("test-user-id")).ReturnsAsync(10);

        var result = await _controller.GetTotalOrders();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(10, (int)obj["TotalOrders"]);
    }

    [Fact]
    public async Task GetRevenueByLivestreamId_ReturnsOkResult_WithRevenue()
    {
        _mockRepo.Setup(repo => repo.GetRevenueByLivestreamId("test-user-id", "live1")).ReturnsAsync(250m);

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
        _mockRepo.Setup(repo => repo.GetRevenueByDateRange("test-user-id", start, end)).ReturnsAsync(500m);

        var result = await _controller.GetRevenueByDateRange(start, end);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(500m, (decimal)obj["TotalRevenue"]);
        Assert.Equal(start, (DateTime)obj["StartDate"]);
        Assert.Equal(end, (DateTime)obj["EndDate"]);
    }

    [Fact]
    public async Task GetTotalOrdersCancelled_ReturnsOkResult()
    {
        _mockRepo.Setup(repo => repo.GetTotalOrederCancelled("test-user-id")).ReturnsAsync(2);

        var result = await _controller.GetTotalOrdersCancelled();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(2, (int)obj["TotalOrdersCancelled"]);
    }

    [Fact]
    public async Task GetTotalOrdersReturned_ReturnsOkResult()
    {
        _mockRepo.Setup(repo => repo.GetTotalOrederReturned("test-user-id")).ReturnsAsync(1);

        var result = await _controller.GetTotalOrdersReturned();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(1, (int)obj["TotalOrdersReturned"]);
    }

    [Fact]
    public async Task GetTotalOrdersDelivered_ReturnsOkResult()
    {
        _mockRepo.Setup(repo => repo.GetTotalOrederDelivered("test-user-id")).ReturnsAsync(7);

        var result = await _controller.GetTotalOrdersDelivered();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var obj = JObject.FromObject(okResult.Value);
        Assert.Equal(7, (int)obj["TotalOrdersDelivered"]);
    }
}
