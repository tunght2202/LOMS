using LOMSAPI.Data.Entities;
using LOMSAPI.Repositories.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly LOMSDbContext _context;

        public CommentController(ICommentRepository commentRepository,IDistributedCache cache, LOMSDbContext context)
        {
            _commentRepository = commentRepository;
            _context = context;

        }
        [HttpGet("get-all-comment")]
        public async Task<IActionResult> GetAllComments(string liveStreamId)
        {
            try
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
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