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
       
        [HttpGet("page/live-streams")]
        public async Task<IActionResult> GetLiveStreams()
        {
            var liveStreams = await _liveStreamRepositories.GetAllLiveStreams();
            if (liveStreams == null || !liveStreams.Any()) return NotFound("Không tìm thấy livestream nào.");
            return Ok(liveStreams);
        }
    }
}
