using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.Shared.Responses;
using System.Threading.Tasks;

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
        Task<Response<List<LikeSong>>> GetLikeSongsByAlbum(string userId, Guid albumId);
        Task<Response<List<LikeSong>>> GetLikeSongsByPlaylist(string userId, Guid id);
        Task<Response<LikeAlbum>> GetAlbumLikeByUser(string userId, Guid albumId);
        Task<Response<LikePlaylist>> GetPlaylistLikeByUser(string userId, Guid playlistId);
        //Task<Response<string>> GetCountLikesPlaylist(Guid playlistId);
        //Task<Response<string>> GetCountLikesAlbum(Guid albumId);
        //Task<Response<string>> GetCountLikesSong(Guid songId);
    }
}
