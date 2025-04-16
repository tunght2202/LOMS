using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        
        public CommentController(ICommentRepository commentRepository,IDistributedCache cache)
        {
            _commentRepository = commentRepository;
            
        }
        [HttpGet("get-all-comment")]
        public async Task<IActionResult> GetAllComments(string liveStreamId)
        {
            try
            {
                var comments = await _commentRepository.GetAllComments(liveStreamId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-comments-productcode")]
        public async Task<IActionResult> GetCommentsByProductCode(string ProductCode)
        {
            try
            {
                var comments = await _commentRepository.GetCommentsByProductCode(ProductCode);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}