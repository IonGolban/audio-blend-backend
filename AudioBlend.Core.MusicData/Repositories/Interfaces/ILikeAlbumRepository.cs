using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface ILikeAlbumRepository : IAsyncRepository<LikeAlbum>
    {
        Task<Result<LikeAlbum>> GetLikeAlbum(string userId, Guid albumId);
        Task<Result<string>> DeleteLike(string userId, Guid playlistId);

        Task<Result<List<LikeAlbum>>> GetByUserId(string userId);
        


    }
}
