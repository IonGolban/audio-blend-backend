using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.Shared.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class PlaylistServiceCommand : IPlaylistServiceCommand
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        public PlaylistServiceCommand(IAzureBlobStorageService azureBlobStorageService ,IPlaylistRepository playlistRepository, ICurrentUserService currentUserService, UserManager<IdentityUser> userManager)
        {
            _playlistRepository = playlistRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _azureBlobStorageService = azureBlobStorageService;
        }
        public async Task<Response<Playlist>> AddPlaylist(CreatePlaylistDto playlist)
        {
            if(string.IsNullOrEmpty(playlist.Name) || playlist.IsPublic == null)
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = "Name or public access is required"
                };
            }
            var user = await _userManager.FindByIdAsync(_currentUserService.GetUserId);
            if(user == null)
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            Result<string> ImgUrl = null;
            if(playlist.Image != null)
            {
                ImgUrl = await _azureBlobStorageService.UploadFileToBlobAsync(playlist.Image);
            }

            var newPlaylist = new Playlist(Guid.NewGuid(), playlist.Name, playlist.IsPublic, user.Id, ImgUrl.Value, playlist.Description);
            var result = await _playlistRepository.AddAsync(newPlaylist);
            if (!result.IsSuccess)
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<Playlist>()
            {
                Data = result.Value,
                Success = true
            };


            
        }

        public async Task<Response<Playlist>> DeletePlaylist(Guid id)
        {
            var checkPlaylist = await _playlistRepository.GetByIdAsync(id);
            if (checkPlaylist == null)
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = "Playlist not found"
                };
            }

            var result = await _playlistRepository.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<Playlist>()
            {
                Data = result.Value,
                Success = true
            };

        }

        public async Task<Response<Playlist>> UpdatePlaylist(UpdatePlaylistDto playlist)
        {
            if (playlist.Id == Guid.Empty || string.IsNullOrEmpty(playlist.Name) || string.IsNullOrEmpty(playlist.Description))
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = "Invalid inputs"
                };
            }
            var requestedPlaylist = await _playlistRepository.GetByIdAsync(playlist.Id);
            if (requestedPlaylist == null)
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = "Playlist not found"
                };
            }

            var coverUrl = requestedPlaylist.Value.CoverUrl;
            Console.WriteLine(coverUrl);
            Console.WriteLine(playlist.Image == null);
            if (playlist.Image != null)
            {
                var ImgUrl = await _azureBlobStorageService.UploadFileToBlobAsync(playlist.Image);
                if (!ImgUrl.IsSuccess)
                {
                    return new Response<Playlist>()
                    {
                        Success = false,
                        Message = ImgUrl.ErrorMsg
                    };
                }
                coverUrl = ImgUrl.Value;
                Console.WriteLine(coverUrl);
            }

            requestedPlaylist.Value.Update(playlist.Name, playlist.Description, playlist.IsPublic, coverUrl);
            

            var result = await _playlistRepository.UpdateAsync(requestedPlaylist.Value);
            if (!result.IsSuccess)
            {
                return new Response<Playlist>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<Playlist>()
            {
                Data = result.Value,
                Success = true
            };      
        }
    }
}

