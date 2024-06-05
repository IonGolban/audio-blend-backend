using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Artists
{
    [ApiController]
    [Route("api/v1/music-data/artists")]
    public class ArtistController : ControllerBase
    {
        public ArtistController(IArtistService artistService, ICurrentUserService currentUserService)
        {
            _artistService = artistService;
            _currentUserService = currentUserService;
        }
        private readonly IArtistService _artistService;
        private readonly ICurrentUserService _currentUserService;

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetArtist([FromRoute] Guid id)
        {
            var res = await _artistService.GetById(id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllArtists()
        {
            var res = await _artistService.GetAll();
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("user/isArtist/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsUserArtist([FromRoute] string userId)
        {
            var res = await _artistService.IsUserArtist(userId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [Authorize]
        [HttpGet("user/isArtist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsCurrentUserArtist()
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _artistService.IsUserArtist(userId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

    }
}
