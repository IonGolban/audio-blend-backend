using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IPlaylistRepository : IAsyncRepository<Playlist>
    {
        Task<Result<List<Playlist>>> GetPlaylistsByUserId(string userId);
        Task<Result<List<Playlist>>> GetLikedByUserId(string userId);
    }
}
