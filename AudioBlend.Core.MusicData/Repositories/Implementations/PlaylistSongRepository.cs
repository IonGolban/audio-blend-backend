using AudioBlend.Core.MusicData.Models.Playlists;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class PlaylistSongRepository(AudioBlendContext context) : BaseRepository<PlaylistSong>(context), IPlaylistSongRepository
    {
        private readonly AudioBlendContext _context = context;
                
        public async Task<Result<PlaylistSong>> GetPlaylistSong(Guid playlistId, Guid songId)
        {
            var playlistSong = await _context.PlaylistSongs.FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);
            if(playlistSong == null)
            {
                return Result<PlaylistSong>.Failure("Playlist song not found");
            }
            return Result<PlaylistSong>.Success(playlistSong);
        }

        public async Task<Result<PlaylistSong>> DeletePlaylistSong(Guid playlistId, Guid songId)
        {
            var playlistSong = await _context.PlaylistSongs.FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);
            if(playlistSong == null)
            {
                return Result<PlaylistSong>.Failure("Playlist song not found");
            }
            _context.PlaylistSongs.Remove(playlistSong);
            await _context.SaveChangesAsync();
            return Result<PlaylistSong>.Success(playlistSong);

        }
    }
}
