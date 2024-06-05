using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IFollowArtistRepository : IAsyncRepository<FollowArtist>
    {
        Task<Result<FollowArtist>> GetFollowArtist(string userId, Guid artistId);
        Task<Result<FollowArtist>> FollowArtist(FollowArtist followArtist);
        Task<Result<FollowArtist>> UnfollowArtist(string userId, Guid artistId);
        Task<Result<List<FollowArtist>>> GetByArtistId(Guid artistId);
        Task<Result<List<FollowArtist>>> GetByUserId(string userId);

    }
}
