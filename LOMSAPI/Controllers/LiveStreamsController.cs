using System.Security.Claims;
using LOMSAPI.Repositories.LiveStreams;
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
        /// <summary>
        /// Api to get all livestreams from database
        /// </summary>
        /// <returns></returns>
        [HttpGet("allDb")]
        public async Task<IActionResult> GetAllLiveStreamsFromDb()
        {
           // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           string userId = "2347eaee-4ab1-4fec-aee3-19a6325cb494";
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("UserID not found in token.");

            try
            {
                var liveStreams = await _liveStreamRepositories.GetAllLiveStreamsFromDb(userId);
                return Ok(liveStreams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Api to get all livestreams from Facebook API
        /// </summary>
        /// <returns></returns>
        [HttpGet("facebook")] // Lấy từ Facebook API
        public async Task<IActionResult> GetAllLiveStreamsFromFacebook()
        {
            // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string userId = "269841f9-391e-4b7c-83f4-2f14459ad728";
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("UserID not found in token.");

            try
            {
                var liveStreams = await _liveStreamRepositories.GetAllLiveStreamsFromFacebook(userId);
                return Ok(liveStreams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Api to get livestream by ID
        /// </summary>
        /// <param name="liveStreamId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Api to delete a livestream
        /// </summary>
        /// <param name="liveStreamId"></param>
        /// <returns></returns>
        [HttpDelete("{liveStreamId}")]
        public async Task<IActionResult> DeleteLiveStream(string liveStreamId)
        {
            // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string userId = "269841f9-391e-4b7c-83f4-2f14459ad728";
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