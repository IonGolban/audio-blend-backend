using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface ILikeService
    {
        Task<Response<LikeAlbum>> LikeAlbum(string userId, Guid albumId);
        Task<Response<LikeSong>> LikeSong(string userId, Guid songId);
        Task<Response<LikePlaylist>> LikePlaylist(string userId, Guid playlistId);
        Task<Response<LikeSong>> UnlikeSong(string userId, Guid songId);
        Task<Response<LikeAlbum>> UnlikeAlbum(string userId, Guid albumId);
        Task<Response<LikePlaylist>> UnlikePlaylist(string userId, Guid playlistId);
        //Task<Response<string>> GetCountLikesPlaylist(Guid playlistId);
        //Task<Response<string>> GetCountLikesAlbum(Guid albumId);
        //Task<Response<string>> GetCountLikesSong(Guid songId);
    }
}
