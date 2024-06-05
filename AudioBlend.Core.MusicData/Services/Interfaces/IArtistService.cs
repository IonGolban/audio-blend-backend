using AudioBlend.Core.MusicData.Models.DTOs.Artists;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IArtistService
    {
        Task<Response<List<ArtistQueryDto>>> GetAll();
        Task<Response<ArtistQueryDto>> GetById(Guid artist);
        Task<Response<ArtistQueryDto>> IsUserArtist(string userId);
    }
}
