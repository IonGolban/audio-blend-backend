using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Albums
{
    [ApiController]
    [Route("api/v1/music-data/albums")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly ICurrentUserService _currentUserService;
        
        public AlbumController(IAlbumService albumService, ICurrentUserService currentUserService)
        {
            _albumService = albumService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAlbums()
        {
            var res = await _albumService.GetAllAlbums();
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAlbum([FromRoute] Guid id)
        {
            var res = await _albumService.GetAlbumById(id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("liked")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikedAlbums()
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _albumService.GetLikedUserAlbums(userId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("liked/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikedAlbumsByUser( string id)
        {
            var res = await _albumService.GetLikedUserAlbums(id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);

        }

        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandom([FromQuery]int count)
        {
            var res = await _albumService.GetRandomAlbums(count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [Authorize]
        [HttpGet("auth/random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandomAuth([FromQuery]int count)
        {
            var res = await _albumService.GetRecommendedAlbums(count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("genre/{genre}/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByGenre(string genre, int count)
        {
            var res = await _albumService.GetByGenre(genre, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("genres/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByGenres([FromQuery] List<string> genres, int count)
        {
            var res = await _albumService.GetByGenres(genres, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("artist/{artistId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByArtistId([FromRoute] Guid artistId)
        {
            var res = await _albumService.GetByArtistId(artistId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
    }
}
