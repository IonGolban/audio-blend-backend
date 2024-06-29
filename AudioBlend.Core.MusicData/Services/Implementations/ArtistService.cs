using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Mappers;
using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.MusicData.Models.DTOs.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Users;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.Shared.Results;
using Microsoft.AspNetCore.Identity;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        public ArtistService(IAzureBlobStorageService azureBlobStorageService ,ICurrentUserService currentUserService, UserManager<IdentityUser> userManager ,IArtistRepository artistRepository, IGenreRepository genreRepository)
        {
            _artistRepository = artistRepository;
            _genreRepository = genreRepository;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _azureBlobStorageService = azureBlobStorageService;
        }

        public async Task<Response<List<ArtistQueryDto>>> GetAll()
        {
            var result = await _artistRepository.GetAll();
            if (!result.IsSuccess)
            {
                return new Response<List<ArtistQueryDto>>()
                {
                    Success = false,
                    Message = "Error while getting all artists"
                };
            }

            var artistsDto = new List<ArtistQueryDto>();
            foreach (var artist in result.Value)
            {
                var genres = await _genreRepository.GetByArtistId(artist.Id);
                artistsDto.Add(ArtistMapper.MapToArtistQueryDto(artist,genres.Value));
            }

            return new Response<List<ArtistQueryDto>>()
            {
                Data = artistsDto,
                Success = true
            };

        }

        public async Task<Response<ArtistQueryDto>> GetById(Guid artist)
        {
            var result = await _artistRepository.GetByIdAsync(artist);
            if (!result.IsSuccess)
            {
                return new Response<ArtistQueryDto>()
                {
                    Success = false,
                    Message = "Error while getting artist"
                };
            }
            var genres = await _genreRepository.GetByArtistId(artist);
            return new Response<ArtistQueryDto>()
            {
                Data = ArtistMapper.MapToArtistQueryDto(result.Value, genres.Value),
                Success = true
            };
        }

        public async Task<Response<ArtistQueryDto>> IsUserArtist(string userId)
        {
            var result = await _artistRepository.getByUserId(userId);
            if (!result.IsSuccess)
            {
                return new Response<ArtistQueryDto>()
                {
                    Success = true,
                    Message = "Error while getting artist"
                };
            }
            var genres = await _genreRepository.GetByArtistId(result.Value.Id);
            return new Response<ArtistQueryDto>()
            {
                Data = ArtistMapper.MapToArtistQueryDto(result.Value, genres.Value),
                Success = true
            };
        }

        public async Task<Response<Artist>> CreateArtistByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var artist = new Artist(Guid.NewGuid(), user.UserName, userId);
            artist.setImage(string.Empty);


            var result = await _artistRepository.AddAsync(artist);
            if (!result.IsSuccess)
            {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "Error while creating artist"
                };
            }
            return new Response<Artist>()
            {
                Data = result.Value,
                Success = true
            };

        }

        public async Task<Response<List<Artist>>> GetByGenres(GenresQueryDto genres, int count)
        {
            if(genres.GenresIds.Count == 0)
            {
                return new Response<List<Artist>>()
                {
                    Success = false,
                    Message = "No genres selected"
                };
            }
            var result = await _artistRepository.GetByGenres(genres.GenresIds, count);

            if (!result.IsSuccess)
            {
                return new Response<List<Artist>>()
                {
                    Success = false,
                    Message = "Error while getting artist"
                };
            }

            return new Response<List<Artist>>()
            {
                Data = result.Value,
                Success = true
            };

        }

        public async Task<Response<Artist>> UpdateImage(UpdateImgDto updateImgDto)
        {
            if(updateImgDto.Image == null)
            {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "No image provided"
                };
            }

            var userId = _currentUserService.GetUserId;
            
            var artist = await _artistRepository.getByUserId(userId);
            if (!artist.IsSuccess)
            {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "Error while getting artist by current user"
                };
            }

            var imageUrl = await _azureBlobStorageService.UploadFileToBlobAsync(updateImgDto.Image);

            if (!imageUrl.IsSuccess)
            {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "Error while uploading image"
                };
            }

            artist.Value.setImage(imageUrl.Value);

            var result = await _artistRepository.UpdateAsync(artist.Value);

            if (!result.IsSuccess)
            {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "Error while updating artist"
                };
            }

            return new Response<Artist>()
            {
                Data = result.Value,
                Success = true
            };
            
        }

        public async Task<Response<Artist>> UpdateName(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "No name provided"
                };
            }
            var userId = _currentUserService.GetUserId;
            
            var artist = await _artistRepository.getByUserId(userId);
            if (!artist.IsSuccess)
            {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "Error while getting artist by current user"
                };
            }

            artist.Value.setName(name);

            var result = await _artistRepository.UpdateAsync(artist.Value);
            if (!result.IsSuccess) {
                return new Response<Artist>()
                {
                    Success = false,
                    Message = "Error while updating artist"
                };
            }

            return new Response<Artist>()
            {
                Data = result.Value,
                Success = true
            };
        }
    }
}
