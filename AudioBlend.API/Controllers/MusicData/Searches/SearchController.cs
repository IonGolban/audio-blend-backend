using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.UserAccess.Services.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Searches
{

    [ApiController]
    [Route("api/v1/music-data/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;


        public SearchController(ISearchService searchService,ICurrentUserService currentUserService, IUserService userService)
        {
            _searchService = searchService;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAll([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }

            var res = await _searchService.SearchAll(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("albums")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAlbums([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }
            var res = await _searchService.SearchAlbums(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("songs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchSongs([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }
            var res = await _searchService.SearchSongs(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("artists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchArtists([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }
            var res = await _searchService.SearchArtists(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchUsers([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }
            var res = await _userService.SerachUsersByQuery(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

    }
}
