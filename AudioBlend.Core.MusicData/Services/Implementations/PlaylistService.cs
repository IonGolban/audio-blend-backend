using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class PlaylistService(IPlaylistRepository playlistRepository, ICurrentUserService currentUserService, UserManager<IdentityUser> userManager) : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository = playlistRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public Task<Response<Playlist>> CreatePlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<Playlist>>> GetAllPLaylists()
        {
            var result = await _playlistRepository.GetAll();
            if (!result.IsSuccess)
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            var playlists = result.Value.ToList();



            return new Response<List<Playlist>>()
            {
                Data = playlists,
                Success = true
            };
        }


        public async Task<Response<Playlist>> GetPlaylistById(Guid id)
        {
            var result = await _playlistRepository.GetByIdAsync(id);

            if(result.IsSuccess)
            {
                return new Response<Playlist>()
                {
                    Data = result.Value,
                    Success = true
                };
            } 

            var playlist = result.Value;
            return new Response<Playlist>()
            {
                Data = playlist,
                Success = true
            };
        }

        public async Task<Response<List<Playlist>>> GetPlaylistsByCurrentUser()
        {
            var currentUserId = _currentUserService.GetUserId;
            if (currentUserId == null)
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            var result = await GetPlaylistsByUserId(currentUserId);
            if(!result.Success)
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = result.Message
                };
            }
            return new Response<List<Playlist>>()
            {
                Data = result.Data,
                Success = true
            };

        }

        public async Task<Response<List<Playlist>>> GetPlaylistsByUserId(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            if(!await CheckUserId(userId))
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var result = await _playlistRepository.GetPlaylistsByUserId(userId);
            if (!result.IsSuccess)
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<List<Playlist>>()
            {
                Data = result.Value.ToList(),
                Success = true
            };
        }

        public async Task<Response<List<Playlist>>> GetLikedUserPlaylists(string userId)
        {
            if(!await CheckUserId(userId))
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var likedPlaylists = await _playlistRepository.GetLikedByUserId(userId);
            if (!likedPlaylists.IsSuccess)
            {
                return new Response<List<Playlist>>()
                {
                    Success = false,
                    Message = likedPlaylists.ErrorMsg
                };
            }
            return new Response<List<Playlist>>()
            {
                Data = likedPlaylists.Value,
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

    }
}
