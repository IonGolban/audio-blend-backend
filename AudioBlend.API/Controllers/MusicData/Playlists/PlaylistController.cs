using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AudioBlend.API.Controllers.MusicData.Playlists
{
    [ApiController]
    [Route("api/v1/music-data/playlists")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPlaylistServiceCommand _playlistServiceCommand;

        public PlaylistController(IPlaylistService playlistService, ICurrentUserService currentUserService, IPlaylistServiceCommand playlistServiceCommand)
        {
            _playlistService = playlistService;
            _currentUserService = currentUserService;
            _playlistServiceCommand = playlistServiceCommand;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            var response = await _playlistService.GetAllPLaylists();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylistById([FromRoute] Guid id)
        {
            var response = await _playlistService.GetPlaylistById(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist([FromRoute] Guid id)
        {
            var response = await _playlistServiceCommand.DeletePlaylist(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPlaylistsByUserId([FromRoute] string userId)
        {
            var response = await _playlistService.GetPlaylistsByUserId(userId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetPlaylistsByCurrentUser()
        {
            var response = await _playlistService.GetPlaylistsByCurrentUser();
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpGet("liked")]
        public async Task<IActionResult> GetLikedByCurrentUser()
        {
            var userId = _currentUserService.GetUserId;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found");
            }
            var response = await _playlistService.GetLikedUserPlaylists(userId);
            if (!response.Success)
            {   
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet("liked/{id}")]
        public async Task<IActionResult> GetLikedByUserId([FromRoute] string id)
        {
            var userId = _currentUserService.GetUserId;
            var response = await _playlistService.GetLikedUserPlaylists(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost("add")]
        
        public async Task<IActionResult> AddPlaylist(CreatePlaylistDto playlist)
        {
            var response = await _playlistServiceCommand.AddPlaylist(playlist);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePlaylist(UpdatePlaylistDto playlist)
        {
            var response = await _playlistServiceCommand.UpdatePlaylist(playlist);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }


    }
}
