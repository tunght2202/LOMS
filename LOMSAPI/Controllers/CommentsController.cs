using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.Comments;
using LOMSAPI.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CommentController(ICommentRepository commentRepository,IDistributedCache cache, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        [HttpGet("get-all-comment")]
        public async Task<IActionResult> GetAllComments(string liveStreamId)
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                User user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound("Not found user!");
                }
                var token = user.TokenFacbook;
                var comments = await _commentRepository.GetAllComments(liveStreamId,token);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }      
    }
}