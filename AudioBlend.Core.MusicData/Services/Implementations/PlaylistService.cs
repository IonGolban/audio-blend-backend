using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Mappers;
using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class PlaylistService(IAlbumRepository albumRepository,IArtistRepository artistRepository,IGenreRepository genreRepository,IPlaylistRepository playlistRepository, ICurrentUserService currentUserService, UserManager<IdentityUser> userManager) : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository = playlistRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenreRepository _genreRepository = genreRepository;
        private readonly IArtistRepository _artistRepository = artistRepository;
        private readonly IAlbumRepository _albumRepository = albumRepository;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public Task<Response<Playlist>> CreatePlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<PlaylistQueryDto>>> GetAllPLaylists()
        {
            var result = await _playlistRepository.GetAll();
            if (!result.IsSuccess)
            {
                return new Response<List<PlaylistQueryDto>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            var playlists = result.Value.ToList();
            var playlistQueryDtos = new List<PlaylistQueryDto>();
            foreach (var playlist in playlists)
            {
                var songs = await ToListSongQueryDto(playlist.PlaylistSongs.Select(sg => sg.Song).ToList());
                playlistQueryDtos.Add(PlaylistMapper.MapToQueryDto(playlist, songs));
            }

            return new Response<List<PlaylistQueryDto>>()
            {
                Data = playlistQueryDtos,
                Success = true
            };
        }


        public async Task<Response<PlaylistQueryDto>> GetPlaylistById(Guid id)
        {
            var result = await _playlistRepository.GetByIdAsync(id);
            if(!result.IsSuccess)
            {
                return new Response<PlaylistQueryDto>()
                {
                    Message = result.ErrorMsg,
                    Success = false
                };
            }
            var songsQueryDto = await ToListSongQueryDto(result.Value.PlaylistSongs.Select(sg => sg.Song).ToList());
            
            return new Response<PlaylistQueryDto>()
            {
                Data = PlaylistMapper.MapToQueryDto(result.Value,songsQueryDto),
                Success = true
            };
        }

        public async Task<Response<List<PlaylistQueryDto>>> GetPlaylistsByCurrentUser()
        {
            var currentUserId = _currentUserService.GetUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new Response<List<PlaylistQueryDto>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            var result = await GetPlaylistsByUserId(currentUserId);
            
            if (!result.Success)
            {
                return new Response<List<PlaylistQueryDto>>()
                {
                    Success = false,
                    Message = result.Message
                };
            
            }
            
            return new Response<List<PlaylistQueryDto>>()
            {
                Data = result.Data,
                Success = true
            };

        }

        public async Task<Response<List<PlaylistQueryDto>>> GetPlaylistsByUserId(string userId)
        {
            if(!await CheckUserId(userId))
            {
                return new Response<List<PlaylistQueryDto>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var result = await _playlistRepository.GetPlaylistsByUserId(userId);
            if (!result.IsSuccess)
            {
                return new Response<List<PlaylistQueryDto>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            var playlistQueryDtos = new List<PlaylistQueryDto>();
            foreach (var playlist in result.Value)
            {
                var songs = await ToListSongQueryDto(playlist.PlaylistSongs.Select(sg => sg.Song).ToList());
                playlistQueryDtos.Add(PlaylistMapper.MapToQueryDto(playlist, songs));
            }

            return new Response<List<PlaylistQueryDto>>()
            {
                Data = playlistQueryDtos,
                Success = true
            };
        }

        public async Task<Response<List<PlaylistQueryDto>>> GetLikedUserPlaylists(string userId)
        {
            if(!await CheckUserId(userId))
            {
                return new Response<List<PlaylistQueryDto>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var likedPlaylists = await _playlistRepository.GetLikedByUserId(userId);
            if (!likedPlaylists.IsSuccess)
            {
                return new Response<List<PlaylistQueryDto>>()
                {
                    Success = false,
                    Message = likedPlaylists.ErrorMsg
                };
            }

            var playlistQueryDtos = new List<PlaylistQueryDto>();

            foreach (var playlist in likedPlaylists.Value)
            {
                var songs = await ToListSongQueryDto(playlist.PlaylistSongs.Select(sg => sg.Song).ToList());
                playlistQueryDtos.Add(PlaylistMapper.MapToQueryDto(playlist, songs));
            }

            return new Response<List<PlaylistQueryDto>>()
            {
                Data = playlistQueryDtos,
                Success = true
            };
        }
        public async Task<bool> CheckUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            return true;
        }
        private async Task<List<SongQueryDto>> ToListSongQueryDto(List<Song> songs)
        {
            var songDtos = new List<SongQueryDto>();
            foreach (var song in songs)
            {
                var genres = await _genreRepository.GetBySongId(song.Id);
                var artist = await _artistRepository.GetByIdAsync(song.ArtistId);
                var album = await _albumRepository.GetByIdAsync(song.AlbumId);
                song.Album = album.Value;

                var songDto = SongMapper.MapToSongQueryDto(song,artist.Value, genres.Value);
                songDtos.Add(songDto);
            }
            return songDtos;
        }
        private async Task<SongQueryDto> ToSongQueryDto(Song song)
        {
            var genres = await _genreRepository.GetBySongId(song.Id);
            var artist = await _artistRepository.GetByIdAsync(song.ArtistId);
            var album = await _albumRepository.GetByIdAsync(song.AlbumId);
            
            
            song.Album = album.Value;
            var songDto = SongMapper.MapToSongQueryDto(song, artist.Value, genres.Value);

            return songDto;
        }

    }
}
