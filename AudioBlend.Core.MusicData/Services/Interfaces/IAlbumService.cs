using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<Response<List<AlbumQueryDto>>> GetAllAlbums();
        Task<Response<AlbumQueryDto>> GetAlbumById(Guid albumId);
        Task<Response<List<AlbumQueryDto>>> GetLikedUserAlbums(string userId);
        Task<Response<List<AlbumQueryDto>>> GetRandomAlbums(int count);
        Task<Response<List<AlbumQueryDto>>> GetRecommendedAlbums(int count);

        Task<Response<List<AlbumQueryDto>>> GetByGenre(string genre, int count);
        Task<Response<List<AlbumQueryDto>>> GetByGenres(List<string> genres, int count);

    }
}
