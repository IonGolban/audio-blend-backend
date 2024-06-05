using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Songs
{ 
    [ApiController]
    [Route("api/v1/music-data/songs")]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly ICurrentUserService _currentUserService;

        public SongController(ISongService songService, ICurrentUserService currentUserService)
        {
            _songService = songService;
            _currentUserService = currentUserService;
        }

        [HttpGet("top/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopSongs([FromRoute] int count)
        {
            var res = await _songService.GetTopSongs(count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandomSongs([FromQuery] int count)
        {
            var res = await _songService.GetRandomSongs(count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpGet("auth/random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandomAuthUserSongs([FromQuery]int count)
        {
            var res = await _songService.GetRecommendations(count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("genre/{genre}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByGenre([FromRoute] string genre, [FromQuery] int count)
        {
            var res = await _songService.GetByGenre(genre, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("genres/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByGenres([FromBody] List<string> genres, [FromRoute] int count)
        {
            var res = await _songService.GetByGenres(genres, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("artist/{artistId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByArtist([FromRoute] Guid artistId)
        {
            var res = await _songService.GetByArtistId(artistId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [Authorize]
        [HttpGet("liked")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikedSongs()
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _songService.GetLikedSongs(userId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("liked/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikedSongsByUser([FromRoute] string userId)
        {
            var res = await _songService.GetLikedSongs(userId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

    }
}
