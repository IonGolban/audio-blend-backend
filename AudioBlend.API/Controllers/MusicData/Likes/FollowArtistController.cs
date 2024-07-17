using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Likes
{

    [ApiController]
    [Route("api/v1/music-data/likes/artists")]
    public class FollowArtistController : ControllerBase
    {
        private readonly IFollowService _followArtistService;
        private readonly ICurrentUserService _currentUserService;

        public FollowArtistController(IFollowService followArtistService, ICurrentUserService currentUserService)
        {
            _followArtistService = followArtistService;
            _currentUserService = currentUserService;
        }

        [Authorize]
        [HttpPost("add/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> FollowArtist([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _followArtistService.FollowArtist(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpPost("remove/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UnfollowArtist([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _followArtistService.UnfollowArtist(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("followed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFollowedArtists()
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _followArtistService.GetFollowArtistByUser(userId);

            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res.Data);
        }

        [HttpGet("followed/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFollowedArtistsByUser([FromRoute] string userId)
        {
            var res = await _followArtistService.GetFollowArtistByUser(userId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("followers/{artistId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFollowers([FromRoute] Guid artistId)
        {
            var res = await _followArtistService.GetFollowersArtistByArtist(artistId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpGet("{artistId}/check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFollowedEntity([FromRoute] Guid artistId)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _followArtistService.GetFollowArtist(userId, artistId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
      

    }
}
