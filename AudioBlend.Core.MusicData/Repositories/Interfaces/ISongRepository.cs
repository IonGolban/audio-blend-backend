using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface ISongRepository : IAsyncRepository<Song>
    {
        Task<Result<List<Song>>> GetRandom(int count);
        Task<Result<List<Song>>> GetByGenre(string genre, int count);
        Task<Result<List<Song>>> GetByAlbumId(Guid albumId);
        Task<Result<List<Song>>> GetByArtistId(Guid artistId);
        Task<Result<List<SearchRepoResult<Song>>>> SearchBySongName(string name, int treshold, int count);
    }
}
