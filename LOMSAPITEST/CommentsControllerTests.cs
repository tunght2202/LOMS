using LOMSAPI.Controllers;
using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Security.Claims;
using Newtonsoft.Json;
using LOMSAPI.Models;

namespace LOMSAPI.Tests.Controllers
{ 

    public class CommentControllerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<LOMSDbContext> _mockContext;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockContext = new Mock<LOMSDbContext>(new DbContextOptions<LOMSDbContext>());
            _mockCache = new Mock<IDistributedCache>();

            _controller = new CommentController(_mockCommentRepository.Object, _mockCache.Object, _mockContext.Object);

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
        public async Task GetAllComments_UserExistsWithToken_ReturnsOkResult()
        {
            // Arrange
            var userId = "test-user-id";
            var liveStreamId = "stream123";
            var facebookToken = "fbToken123";

            var user = new User { Id = userId, TokenFacbook = facebookToken };
            var comments = new List<CommentModel>
    {
        new CommentModel
        {
            CommentID = "c1",
            Content = "Nice stream!",
            CommentTime = DateTime.Now,
            CustomerAvatar = "avatar.png",
            CustomerName = "Alice",
            CustomerId = "cust1"
        }
    };

            var mockDbSet = CreateMockDbSet(new List<User> { user });
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            _mockCommentRepository
                .Setup(repo => repo.GetAllComments(liveStreamId, facebookToken))
                .ReturnsAsync(comments);

            // Act
            var result = await _controller.GetAllComments(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<Comment>>(serialized);

            Assert.Single(returnValue);
            Assert.Equal("Nice stream!", returnValue[0].Content);
        }

        [Fact]
        public async Task GetAllComments_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream123";

            var mockDbSet = CreateMockDbSet(new List<User>());
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _controller.GetAllComments(liveStreamId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not found user!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllComments_RepositoryThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = "test-user-id";
            var liveStreamId = "stream123";
            var facebookToken = "fbToken123";
            var errorMessage = "Error retrieving comments";

            var user = new User { Id = userId, TokenFacbook = facebookToken };

            var mockDbSet = CreateMockDbSet(new List<User> { user });
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            _mockCommentRepository
                .Setup(repo => repo.GetAllComments(liveStreamId, facebookToken))
                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetAllComments(liveStreamId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllComments_UserExistsWithNullToken_ReturnsOkResult()
        {
            // Arrange
            var userId = "test-user-id";
            var liveStreamId = "stream123";
            string facebookToken = null;

            var user = new User { Id = userId, TokenFacbook = facebookToken };
            var comments = new List<CommentModel>
    {
        new CommentModel
        {
            CommentID = "c1",
            Content = "Nice stream!",
            CommentTime = DateTime.Now,
            CustomerAvatar = "avatar.png",
            CustomerName = "Alice",
            CustomerId = "cust1"
        }
    };

            var mockDbSet = CreateMockDbSet(new List<User> { user });
            _mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            _mockCommentRepository
                .Setup(repo => repo.GetAllComments(liveStreamId, facebookToken))
                .ReturnsAsync(comments);

            // Act
            var result = await _controller.GetAllComments(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<Comment>>(serialized);

            Assert.Single(returnValue);
            Assert.Equal("Nice stream!", returnValue[0].Content);
            // Lưu ý: Nên sửa controller để kiểm tra TokenFacbook không null
        }

      

        private static Mock<DbSet<User>> CreateMockDbSet(List<User> data)
        {
            var mockDbSet = new Mock<DbSet<User>>();
            var queryable = data.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            mockDbSet
                .Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] keyValues) => data.FirstOrDefault(u => u.Id == (string)keyValues[0]));

            mockDbSet
                .Setup(m => m.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), default))
                .ReturnsAsync((System.Linq.Expressions.Expression<Func<User, bool>> predicate, CancellationToken _) =>
                    data.FirstOrDefault(predicate.Compile()));


            return mockDbSet;
        }
    }
}
