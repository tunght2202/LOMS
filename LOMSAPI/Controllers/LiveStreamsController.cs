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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLiveStreams()
        {
           // string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           string userId = "269841f9-391e-4b7c-83f4-2f14459ad728";
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
    }
}