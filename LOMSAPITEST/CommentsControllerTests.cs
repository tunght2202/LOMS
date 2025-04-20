using LOMSAPI.Controllers;
using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Security.Claims;
using LOMSAPI.Models;
using LOMSAPI.Repositories.Users;

namespace LOMSAPI.Tests.Controllers
{

    public class CommentControllerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly CommentController _controller;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly string _userId = "test-user-id";

        public CommentControllerTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockCache = new Mock<IDistributedCache>();

            // Setup default user for most tests
            _mockUserRepository
                .Setup(repo => repo.GetUserById(_userId))
                .ReturnsAsync(new User { Id = _userId, TokenFacbook = "facebook-token" });

            _controller = new CommentController(
                _mockCommentRepository.Object,
                _mockCache.Object,
                _mockUserRepository.Object);

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
            var liveStreamId = "stream1";
            var user = new User { Id = "user1", TokenFacbook = "fake_token" };

            var commentList = new List<CommentModel>
    {
        new CommentModel { CommentID = "c1", Content = "Hello!" }
    };

            _mockUserRepository
                .Setup(repo => repo.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockCommentRepository
                .Setup(repo => repo.GetAllComments(liveStreamId, user.TokenFacbook))
                .ReturnsAsync(commentList);

            // Act
            var result = await _controller.GetAllComments(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedComments = Assert.IsAssignableFrom<IEnumerable<CommentModel>>(okResult.Value);
            Assert.NotNull(returnedComments);                 // ✅ Ensure it's not null
            Assert.NotEmpty(returnedComments);                // ✅ Ensure it has at least one comment
        }

        [Fact]
        public async Task GetAllComments_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream1";

            // Mock GetUserById to return null (simulate missing user)
            _mockUserRepository
                .Setup(repo => repo.GetUserById(It.IsAny<string>()))
                .ReturnsAsync((User)null);

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
            var liveStreamId = "stream1";

            // Mock user
            var user = new User { Id = "user1", TokenFacbook = "token123" };
            _mockUserRepository.Setup(repo => repo.GetUserById(It.IsAny<string>())).ReturnsAsync(user);

            // Force the comment repository to throw
            _mockCommentRepository
                .Setup(repo => repo.GetAllComments(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            var result = await _controller.GetAllComments(liveStreamId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Repository failure", badRequestResult.Value);
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

            // Mock user repository to return the user
            _mockUserRepository
                .Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync(user);

            // Mock comment repository
            _mockCommentRepository
                .Setup(repo => repo.GetAllComments(liveStreamId, facebookToken))
                .ReturnsAsync(comments);

            // Act
            var result = await _controller.GetAllComments(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedComments = Assert.IsAssignableFrom<List<CommentModel>>(okResult.Value);
            Assert.Single(returnedComments);
            Assert.Equal("Nice stream!", returnedComments[0].Content);
        }

    }
}
