﻿using System.Security.Claims;
using LOMSAPI.Repositories.LiveStreams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LiveStreamsController : ControllerBase
    {
        private readonly ILiveStreamRepostitory _liveStreamRepositories;
        public LiveStreamsController(ILiveStreamRepostitory liveStreamRepositories)
        {
            _liveStreamRepositories = liveStreamRepositories;
        }
        
        [HttpGet("facebook")] // Lấy từ Facebook API
        public async Task<IActionResult> GetAllLiveStreamsFromFacebook()
        {

             string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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

             string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        [HttpGet("IsLiveStreamStillLive/{liveStreamId}")]
        public async Task<IActionResult> IsLiveStreamStillLive(string liveStreamId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("UserID not found in token.");
            try
            {
                var isLive = await _liveStreamRepositories.IsLiveStreamStillLive(liveStreamId,userId);
                return Ok(isLive);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        //[HttpGet("MaxPrice/{liveStreamId}")]
        //public async Task<IActionResult> GetMaxPriceLiveStream(string liveStreamId)
        //{
        //    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Unauthorized("UserID not found in token.");
        //    try
        //    {
        //        var maxPrice = await _liveStreamRepositories.GetMaxPriceLiveStream(userId, liveStreamId);
        //        return Ok(maxPrice);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error: {ex.Message}");
        //    }

        //}
    }
}