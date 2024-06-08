using AudioBlend.Core.MusicData.Models.Playlists;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IPlaylistSongRepository : IAsyncRepository<PlaylistSong>
    {
        Task<Result<PlaylistSong>> GetPlaylistSong(Guid playlistId, Guid songId);
        Task<Result<PlaylistSong>> DeletePlaylistSong(Guid playlistId, Guid songId);

    }
}
