using LOMSAPI.Repositories.Live;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LiveStreamController : ControllerBase
    {
        private readonly ILiveRepository _liveRepository;

        public LiveStreamController(ILiveRepository liveRepository)
        {
            _liveRepository = liveRepository;
        }

        [HttpGet("get-all-livestreams")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLivestreams()
        {
            try
            {
                var livestreams = await _liveRepository.GetLiveStreamsAsync();
                return Ok(livestreams);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách livestream", error = ex.Message });
            }
        }

        
        [HttpGet("fetch-from-facebook")]
        [AllowAnonymous]
        public async Task<IActionResult> FetchFromFacebook()
        {
            try
            {
                var livestreams = await _liveRepository.FetchLiveStreamsFromFacebookAsync();
                return Ok(livestreams);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy livestream từ Facebook", error = ex.Message });
            }
        }
    }
}
