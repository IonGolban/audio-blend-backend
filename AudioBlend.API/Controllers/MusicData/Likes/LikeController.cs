using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Likes
{
    [ApiController]
    [Route("api/v1/music-data/likes")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly ICurrentUserService _currentUserService;

        public LikeController(ILikeService likeService, ICurrentUserService currentUserService)
        {
            _likeService = likeService;
            _currentUserService = currentUserService;
        }

        [Authorize]
        [HttpPost("add/song/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LikeSong([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.LikeSong(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpPost("add/playlist/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LikePlaylist([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.LikePlaylist(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpPost("add/album/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LikeAlbum([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.LikeAlbum(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpDelete("remove/song/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLikeSong([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.UnlikeSong(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpDelete("remove/playlist/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLikePlaylist([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.UnlikePlaylist(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [Authorize]
        [HttpDelete("remove/album/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLikeAlbum([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.UnlikeAlbum(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [Authorize]
        [HttpGet("songs/album/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikedSongsByAlbum([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.GetLikeSongsByAlbum(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpGet("songs/playlist/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikedSongsByPlaylist([FromRoute] Guid id)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.GetLikeSongsByPlaylist(userId, id);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [Authorize]
        [HttpGet("album/check/{albumId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckAlbumLikeByUser([FromRoute] Guid albumId)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.GetAlbumLikeByUser(userId, albumId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [Authorize]
        [HttpGet("playlist/check/{playlistId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckPlaylistLikeByUser([FromRoute] Guid playlistId)
        {
            var userId = _currentUserService.GetUserId;
            if (userId == null)
            {
                return BadRequest("User not found");
            }
            var res = await _likeService.GetPlaylistLikeByUser(userId, playlistId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

    }
}
