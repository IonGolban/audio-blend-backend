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
    }
}
