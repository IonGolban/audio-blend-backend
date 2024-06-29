using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.MusicData.Models.DTOs.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Users;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IArtistService
    {
        Task<Response<List<ArtistQueryDto>>> GetAll();
        Task<Response<ArtistQueryDto>> GetById(Guid artist);
        Task<Response<ArtistQueryDto>> IsUserArtist(string userId);
        Task<Response<Artist>> CreateArtistByUserId(string userId);
        Task<Response<List<Artist>>> GetByGenres(GenresQueryDto genres, int count);
        Task<Response<Artist>> UpdateImage(UpdateImgDto updateImgDto);
        Task<Response<Artist>> UpdateName(string name);
    }
}
