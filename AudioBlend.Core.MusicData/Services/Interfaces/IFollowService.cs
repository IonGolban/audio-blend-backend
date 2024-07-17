using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IFollowService
    {
        Task<Response<FollowArtist>> GetFollowArtist(string usriId, Guid artistId);
        Task<Response<FollowArtist>> FollowArtist(string userId, Guid artistId);
        Task<Response<FollowArtist>> UnfollowArtist(string userId, Guid artistId);
        Task<Response<List<FollowArtist>>> GetFollowArtistByUser(string userId);
        Task<Response<List<FollowArtist>>> GetFollowersArtistByArtist(Guid artistId);
        
    }
}
