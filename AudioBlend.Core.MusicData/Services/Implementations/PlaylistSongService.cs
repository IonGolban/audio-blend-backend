using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Models.Playlists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class PlaylistSongService : IPlaylistSongService
    {
        private readonly IPlaylistSongRepository _playlistSongRepository;
        private readonly ISongRepository _songRepository;
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistSongService(IPlaylistSongRepository playlistSongRepository, ISongRepository songRepository, IPlaylistRepository playlistRepository)
        {
            _playlistSongRepository = playlistSongRepository;
            _songRepository = songRepository;
            _playlistRepository = playlistRepository;
        }
        public async Task<Response<PlaylistSong>> AddPlaylistSong(AddSongToPlaylistDto addSongToPlaylistDto)
        {
            var playlistId = addSongToPlaylistDto.PlaylistId;
            var songId = addSongToPlaylistDto.SongId;
            if(playlistId == Guid.Empty || songId == Guid.Empty)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = "Invalid playlist or song id"
                };
            }
            var playlist = await _playlistRepository.GetByIdAsync(playlistId);
            if(!playlist.IsSuccess)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = "Playlist not found"
                };
            }
            var song = await _songRepository.GetByIdAsync(songId);
            if(!song.IsSuccess)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = "Song not found"
                };
            }

            var ifExists = await _playlistSongRepository.GetPlaylistSong(playlistId, songId);
            if(ifExists.IsSuccess)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = "Song already exists in playlist"
                };
            }

            var playlistSong = new PlaylistSong(playlistId, songId, song.Value, playlist.Value);

            var result = await _playlistSongRepository.AddAsync(playlistSong);
            if(!result.IsSuccess)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<PlaylistSong>()
            {
                Data = playlistSong,
                Success = true
            };
        }

        public async Task<Response<PlaylistSong>> RemoveSongFromPlaylist(RemoveSongToPlaylistDto ps)
        {
            var playlistId = ps.PlaylistId;
            var songId = ps.SongId;

            if(playlistId == Guid.Empty || songId == Guid.Empty)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = "Invalid playlist or song id"
                };
            }

            var playlistSong = await _playlistSongRepository.GetPlaylistSong(playlistId, songId);
            if(!playlistSong.IsSuccess)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = "Playlist song not found"
                };
            }

            var result = await _playlistSongRepository.DeletePlaylistSong(playlistId,songId);

            if(!result.IsSuccess)
            {
                return new Response<PlaylistSong>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            return new Response<PlaylistSong>()
            {
                Success = true,
                Data = playlistSong.Value
            };

            

        }
    }
}
