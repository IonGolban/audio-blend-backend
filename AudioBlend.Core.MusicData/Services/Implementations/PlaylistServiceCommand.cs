using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.Shared.Results;
using Microsoft.AspNetCore.Identity;

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
    }
}
