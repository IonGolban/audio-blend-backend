using AudioBlend.Core.MusicData.Migrations;
using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Models.Playlists;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IPlaylistSongService
    {
        Task<Response<PlaylistSong>> AddPlaylistSong(AddSongToPlaylistDto addSongToPlaylist);
        Task<Response<PlaylistSong>> RemoveSongFromPlaylist(RemoveSongToPlaylistDto ps);
    }
}
