using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IPlaylistService
    {

        Task<Response<List<Playlist>>> GetAllPLaylists();
        Task<Response<Playlist>> GetPlaylistById(Guid id);
        Task<Response<Playlist>> CreatePlaylist(Playlist playlist);
        Task<Response<List<Playlist>>> GetPlaylistsByUserId(string userId);
        Task<Response<List<Playlist>>> GetPlaylistsByCurrentUser();
        Task<Response<List<Playlist>>> GetLikedUserPlaylists(string userId);




    }
}
