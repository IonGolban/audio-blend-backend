using AudioBlend.Core.MusicData.Mappers;
using AudioBlend.Core.MusicData.Models.DTOs.Artists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IGenreRepository _genreRepository;

        public ArtistService(IArtistRepository artistRepository, IGenreRepository genreRepository)
        {
            _artistRepository = artistRepository;
            _genreRepository = genreRepository;
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
    }
}
