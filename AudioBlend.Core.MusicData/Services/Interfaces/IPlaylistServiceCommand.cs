using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IPlaylistServiceCommand
    {
        Task<Response<Playlist>> AddPlaylist(CreatePlaylistDto playlist);
        Task<Response<Playlist>> UpdatePlaylist(UpdatePlaylistDto playlist);

    }
}
