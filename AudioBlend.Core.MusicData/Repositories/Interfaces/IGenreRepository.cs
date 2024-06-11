using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IGenreRepository : IAsyncRepository<Genre>
    {
        Task<Result<Genre>> GetByName(string name);
        Task<Result<List<Genre>>> GetBySongId(Guid songId);
        Task<Result<List<Genre>>> GetByArtistId(Guid artistId);
        Task<Result<List<Genre>>> GetByMultipleIds(List<Guid> ids);    
        Task<Result<List<Genre>>> GetByAlbumId(Guid albumId);
    }
}
