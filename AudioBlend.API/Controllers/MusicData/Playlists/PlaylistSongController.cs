using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Playlists
{

    [ApiController]
    [Route("api/v1/music-data/playlist-songs")]
    public class PlaylistSongController : ControllerBase
    {
        private readonly IPlaylistSongService _playlistSongService;
        private readonly ICurrentUserService _currentUserService;

        public PlaylistSongController(IPlaylistSongService playlistSongService, ICurrentUserService currentUserService)
        {
            _playlistSongService = playlistSongService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSongToPlaylist([FromBody] AddSongToPlaylistDto addSongToPlaylistDto)
        {
            var response = await _playlistSongService.AddPlaylistSong(addSongToPlaylistDto);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveSongFromPlaylist([FromBody] RemoveSongToPlaylistDto ps)
        {
            var response = await _playlistSongService.RemoveSongFromPlaylist(ps);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }
    }
}
