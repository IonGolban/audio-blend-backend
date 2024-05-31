using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface ILikePlaylistRepository : IAsyncRepository<LikePlaylist>
    {
        Task<Result<LikePlaylist>> GetLikePlaylist(string userId, Guid playlistId);
        Task<Result<string>> DeleteLike(string userId, Guid playlistId);

        Task<Result<List<LikePlaylist>>> GetByUserId(string userId);

    }
}
