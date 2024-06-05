using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IArtistRepository : IAsyncRepository<Artist>
    {
        Task<Result<Artist>> getByUserId(string userId);
        Task<Result<List<SearchRepoResult<Artist>>>> SearchByArtistName(string name, int treshold, int count);
    }
}
