using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LOMSAPI.Controllers;
using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.LiveStreams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace LOMSAPITEST
{
    public class LiveStreamsControllerTests
    {
        private readonly Mock<ILiveStreamRepostitory> _mockLiveStreamRepository;
        private readonly LiveStreamsController _controller;
        private readonly string _userId = "test-user-id";

        public LiveStreamsControllerTests()
        {
            _mockLiveStreamRepository = new Mock<ILiveStreamRepostitory>();

            _controller = new LiveStreamsController(_mockLiveStreamRepository.Object);

            // Mock user claims for authenticated endpoints
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, _userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task GetAllLiveStreamsFromFacebook_LiveStreamsExist_ReturnsOkResult()
        {
            // Arrange
            var liveStreams = new List<LiveStream>
            {
               new LiveStream
                {
                    LivestreamID = "stream1",
                    UserID = _userId,
                    ListProductID = 1,
                    StreamURL = "https://facebook.com/stream1",
                    StreamTitle = "Live Stream 1",
                    StartTime = new DateTime(2025, 4, 20, 10, 0, 0),
                    Status = "LIVE",
                    StatusDelete = false
                },
                new LiveStream
                {
                    LivestreamID = "stream2",
                    UserID = _userId,
                    ListProductID = 2,
                    StreamURL = "https://facebook.com/stream2",
                    StreamTitle = "Live Stream 2",
                    StartTime = new DateTime(2025, 4, 20, 12, 0, 0),
                    Status = "VOD",
                    StatusDelete = false
                }
            };

            _mockLiveStreamRepository
                .Setup(repo => repo.GetAllLiveStreamsFromFacebook(_userId))
                .ReturnsAsync(liveStreams);

            // Act
            var result = await _controller.GetAllLiveStreamsFromFacebook();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var serialized = JsonConvert.SerializeObject(okResult.Value);
            var returnValue = JsonConvert.DeserializeObject<List<LiveStream>>(serialized);

            Assert.Equal(2, returnValue.Count);
            Assert.Equal("stream1", returnValue[0].LivestreamID);
            Assert.Equal(_userId, returnValue[0].UserID);
            Assert.Equal(1, returnValue[0].ListProductID);
            Assert.Equal("https://facebook.com/stream1", returnValue[0].StreamURL);
            Assert.Equal("Live Stream 1", returnValue[0].StreamTitle);
            Assert.Equal(new DateTime(2025, 4, 20, 10, 0, 0), returnValue[0].StartTime);
            Assert.Equal("LIVE", returnValue[0].Status);
            Assert.False(returnValue[0].StatusDelete);
            Assert.Equal("stream2", returnValue[1].LivestreamID);
            Assert.Equal(_userId, returnValue[1].UserID);
            Assert.Equal(2, returnValue[1].ListProductID);
            Assert.Equal("https://facebook.com/stream2", returnValue[1].StreamURL);
            Assert.Equal("Live Stream 2", returnValue[1].StreamTitle);
            Assert.Equal(new DateTime(2025, 4, 20, 12, 0, 0), returnValue[1].StartTime);
            Assert.Equal("VOD", returnValue[1].Status);
            Assert.False(returnValue[1].StatusDelete);
        }

        [Fact]
        public async Task GetAllLiveStreamsFromFacebook_UnauthenticatedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            var controller = new LiveStreamsController(_mockLiveStreamRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
                }
            };

            // Act
            var result = await controller.GetAllLiveStreamsFromFacebook();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("UserID not found in token.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task GetAllLiveStreamsFromFacebook_ThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            _mockLiveStreamRepository
                .Setup(repo => repo.GetAllLiveStreamsFromFacebook(_userId))
                .ThrowsAsync(new Exception("Facebook API error"));

            // Act
            var result = await _controller.GetAllLiveStreamsFromFacebook();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error: Facebook API error", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetLiveStreamById_LiveStreamExists_ReturnsOkResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var liveStream = new LiveStream
            {
                LivestreamID = liveStreamId,
                UserID = _userId,
                ListProductID = 1,
                StreamURL = "https://facebook.com/stream1",
                StreamTitle = "Live Stream 1",
                StartTime = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "LIVE",
                StatusDelete = false
            };

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ReturnsAsync(liveStream);

            // Act
            var result = await _controller.GetLiveStreamById(liveStreamId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LiveStream>(okResult.Value);
            Assert.Equal(liveStreamId, returnValue.LivestreamID);
            Assert.Equal(_userId, returnValue.UserID);
            Assert.Equal(1, returnValue.ListProductID);
            Assert.Equal("https://facebook.com/stream1", returnValue.StreamURL);
            Assert.Equal("Live Stream 1", returnValue.StreamTitle);
            Assert.Equal(new DateTime(2025, 4, 20, 10, 0, 0), returnValue.StartTime);
            Assert.Equal("LIVE", returnValue.Status);
            Assert.False(returnValue.StatusDelete);
        }

        [Fact]
        public async Task GetLiveStreamById_LiveStreamNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream1";

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ReturnsAsync((LiveStream)null);

            // Act
            var result = await _controller.GetLiveStreamById(liveStreamId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetLiveStreamById_UnauthenticatedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var controller = new LiveStreamsController(_mockLiveStreamRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
                }
            };

            // Act
            var result = await controller.GetLiveStreamById(liveStreamId);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("UserID not found in token.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task GetLiveStreamById_ThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var liveStreamId = "stream1";

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetLiveStreamById(liveStreamId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error: Database error", statusCodeResult.Value);
        }

        [Fact]
        public async Task DeleteLiveStream_LiveStreamExistsAndUserHasPermission_ReturnsOkResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var liveStream = new LiveStream
            {
                LivestreamID = liveStreamId,
                UserID = _userId,
                ListProductID = 1,
                StreamURL = "https://facebook.com/stream1",
                StreamTitle = "Live Stream 1",
                StartTime = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "LIVE",
                StatusDelete = false
            };

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ReturnsAsync(liveStream);

            _mockLiveStreamRepository
                .Setup(repo => repo.DeleteLiveStream(liveStreamId))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteLiveStream(liveStreamId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteLiveStream_LiveStreamNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream1";

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ReturnsAsync((LiveStream)null);

            // Act
            var result = await _controller.DeleteLiveStream(liveStreamId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Livestream not found or you do not have permission to delete it.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteLiveStream_UserLacksPermission_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var liveStream = new LiveStream
            {
                LivestreamID = liveStreamId,
                UserID = "different-user",
                ListProductID = 1,
                StreamURL = "https://facebook.com/stream1",
                StreamTitle = "Live Stream 1",
                StartTime = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "LIVE",
                StatusDelete = false
            };

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ReturnsAsync(liveStream);

            // Act
            var result = await _controller.DeleteLiveStream(liveStreamId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Livestream not found or you do not have permission to delete it.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteLiveStream_DeletionFails_ReturnsNotFoundResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var liveStream = new LiveStream
            {
                LivestreamID = liveStreamId,
                UserID = _userId,
                ListProductID = 1,
                StreamURL = "https://facebook.com/stream1",
                StreamTitle = "Live Stream 1",
                StartTime = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "LIVE",
                StatusDelete = false
            };

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ReturnsAsync(liveStream);

            _mockLiveStreamRepository
                .Setup(repo => repo.DeleteLiveStream(liveStreamId))
                .ReturnsAsync(0);

            // Act
            var result = await _controller.DeleteLiveStream(liveStreamId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Livestream not found or already deleted.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteLiveStream_UnauthenticatedUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            var liveStreamId = "stream1";
            var controller = new LiveStreamsController(_mockLiveStreamRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
                }
            };

            // Act
            var result = await controller.DeleteLiveStream(liveStreamId);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("UserID not found in token.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task DeleteLiveStream_ThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var liveStreamId = "stream1";
            var liveStream = new LiveStream
            {
                LivestreamID = liveStreamId,
                UserID = _userId,
                ListProductID = 1,
                StreamURL = "https://facebook.com/stream1",
                StreamTitle = "Live Stream 1",
                StartTime = new DateTime(2025, 4, 20, 10, 0, 0),
                Status = "LIVE",
                StatusDelete = false
            };

            _mockLiveStreamRepository
                .Setup(repo => repo.GetLiveStreamById(liveStreamId, _userId))
                .ReturnsAsync(liveStream);

            _mockLiveStreamRepository
                .Setup(repo => repo.DeleteLiveStream(liveStreamId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteLiveStream(liveStreamId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error deleting livestream: Database error", statusCodeResult.Value);
        }
    }
}
