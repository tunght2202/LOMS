using System.Security.Claims;
using LOMSAPI.Repositories.LiveStreams;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveStreamsController : ControllerBase
    {
        private readonly ILiveStreamRepostitory _liveStreamRepositories;
        public LiveStreamsController(ILiveStreamRepostitory liveStreamRepositories)
        {
            _liveStreamRepositories = liveStreamRepositories;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLiveStreams()
        {
           // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           string userId = "30eb327f-1ccc-4d1e-90ca-b7694be94761";
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("UserID not found in token.");

            try
            {
                var liveStreams = await _liveStreamRepositories.GetAllLiveStreams(userId);
                return Ok(liveStreams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{liveStreamId}")]
        public async Task<IActionResult> GetLiveStreamById(string liveStreamId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("UserID not found in token.");

            try
            {
                var liveStream = await _liveStreamRepositories.GetLiveStreamById(liveStreamId, userId);
                if (liveStream == null)
                    return NotFound();
                return Ok(liveStream);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpDelete("{liveStreamId}")]
        public async Task<IActionResult> DeleteLiveStream(string liveStreamId)
        {
            // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string userId = "30eb327f-1ccc-4d1e-90ca-b7694be94761";
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("UserID not found in token.");

            try
            {
                // Check if the livestream exists and belongs to the user
                var liveStream = await _liveStreamRepositories.GetLiveStreamById(liveStreamId, userId);
                if (liveStream == null || liveStream.UserID != userId)
                    return NotFound("Livestream not found or you do not have permission to delete it.");

                // Perform the soft delete
                int result = await _liveStreamRepositories.DeleteLiveStream(liveStreamId);
                if (result == 0)
                    return NotFound("Livestream not found or already deleted.");

                return Ok(); // 204 No Content - successful deletion
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting livestream: {ex.Message}");
            }
        }
    }
}