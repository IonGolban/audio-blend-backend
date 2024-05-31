using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface ILikeSongRepository : IAsyncRepository<LikeSong>
    {
        Task<Result<LikeSong>> GetLikeSong(string userId, Guid songId);
        Task<Result<string>> DeleteLike(string userId, Guid songId);

        Task<Result<List<LikeSong>>> GetTopSongs(int count);
        Task<Result<List<LikeSong>>> GetLikedByUser(string userId);



    }
}
