using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IPlaylistService
    {

        Task<Response<List<PlaylistQueryDto>>> GetAllPLaylists();
        Task<Response<PlaylistQueryDto>> GetPlaylistById(Guid id);
        Task<Response<Playlist>> CreatePlaylist(Playlist playlist);
        Task<Response<List<PlaylistQueryDto>>> GetPlaylistsByUserId(string userId);
        Task<Response<List<PlaylistQueryDto>>> GetPlaylistsByCurrentUser();
        Task<Response<List<PlaylistQueryDto>>> GetLikedUserPlaylists(string userId);




    }
}
