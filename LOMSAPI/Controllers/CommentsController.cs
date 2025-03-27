using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        [HttpGet("get-all-comment")]
        [AllowAnonymous]

        public async Task<IActionResult> GetAllComments(string liveStreamURL)
        {
            try
            {
                var comments = await _commentRepository.GetAllComments(liveStreamURL);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-comments-productcode")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByProductCode(string liveStreamURL, string ProductCode)
        {
            try
            {
                var comments = await _commentRepository.GetCommentsByProductCode(liveStreamURL, ProductCode);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}