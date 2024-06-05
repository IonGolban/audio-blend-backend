using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Repositories.Implementations;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class FollowService : IFollowService
    {
        private readonly IFollowArtistRepository _followArtistRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<IdentityUser> _userManager;

        public FollowService(IFollowArtistRepository followArtistRepository, IArtistRepository artistRepository, ICurrentUserService currentUserService, UserManager<IdentityUser> userManager)
        {
            _followArtistRepository = followArtistRepository;
            _artistRepository = artistRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<Response<FollowArtist>> FollowArtist(string userId, Guid artistId)
        {
            if (string.IsNullOrEmpty(userId) || artistId == null)
            {
                return new Response<FollowArtist>()
                {
                    Success = false,
                    Message = "Invalid input"
                };
            }
            var artist = await _artistRepository.GetByIdAsync(artistId);
            if (!artist.IsSuccess)
            {
                return new Response<FollowArtist>()
                {
                    Success = false,
                    Message = "Artist not found"
                };
            }
            var followArtist = new FollowArtist(userId, artistId);

            var checkFollow = await _followArtistRepository.GetFollowArtist(userId, artistId);
            if (checkFollow.IsSuccess)
            {
                return new Response<FollowArtist>()
                {
                    Success = false,
                    Message = "Already following artist"
                };
            }

            var result = await _followArtistRepository.FollowArtist(followArtist);
            
            if (!result.IsSuccess)
            {
                return new Response<FollowArtist>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            
            }
            return new Response<FollowArtist>()
            {
                Success = true,
                Data = result.Value
            };
        }

        public async Task<Response<FollowArtist>> GetFollowArtist(string usriId, Guid artistId)
        {
            var artist = await _artistRepository.GetByIdAsync(artistId);
            if (!artist.IsSuccess)
            {
                return new Response<FollowArtist>()
                {
                    Success = false,
                    Message = "Artist not found"
                };
            }

            var result = await _followArtistRepository.GetFollowArtist(usriId, artistId);
            if (!result.IsSuccess)
            {
                return new Response<FollowArtist>()
                {
                    Success = true,
                    Message = result.ErrorMsg
                };
            }
            return new Response<FollowArtist>()
            {
                Success = true,
                Data = result.Value
            };


        }

        public async Task<Response<List<FollowArtist>>> GetFollowersArtistByArtist(Guid artistId)
        {
            var artist = await _artistRepository.GetByIdAsync(artistId);
            if (!artist.IsSuccess)
            {
                return new Response<List<FollowArtist>>()
                {
                    Success = false,
                    Message = "Artist not found"
                };
            }

            var result = await _followArtistRepository.GetByArtistId(artistId);
            if (!result.IsSuccess)
            {
                return new Response<List<FollowArtist>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            return new Response<List<FollowArtist>>()
            {
                Success = true,
                Data = result.Value
            };

        }

        public async Task<Response<FollowArtist>> UnfollowArtist(string userId, Guid artistId)
        {
            var artist = _artistRepository.GetByIdAsync(artistId);
            if (!artist.Result.IsSuccess)
            {
                return new Response<FollowArtist>()
                {
                    Success = false,
                    Message = "Artist not found"
                };
            }
            var result = _followArtistRepository.UnfollowArtist(userId, artistId);
            if (!result.Result.IsSuccess)
            {
                return new Response<FollowArtist>()
                {
                    Success = false,
                    Message = result.Result.ErrorMsg
                };
            }
            return new Response<FollowArtist>()
            {
                Success = true,
                Data = result.Result.Value
            };

        }

        public async Task<Response<List<FollowArtist>>> GetFollowArtistByUser(string userId)
        {
            if(string.IsNullOrEmpty(userId))
            {
                return new Response<List<FollowArtist>>()
                {
                    Success = false,
                    Message = "Invalid input"
                };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response<List<FollowArtist>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            var result = await _followArtistRepository.GetByUserId(userId);
            if (!result.IsSuccess)
            {
                return new Response<List<FollowArtist>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            return new Response<List<FollowArtist>>()
            {
                Success = true,
                Data = result.Value
            };

        }
    }
}
