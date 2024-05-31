using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class LikePlaylistRepository(AudioBlendContext context) : BaseRepository<LikePlaylist>(context), ILikePlaylistRepository
    {
        private readonly AudioBlendContext _context = context;

        public async Task<Result<LikePlaylist>> GetLikePlaylist(string userId, Guid playlistId)
        {
            var result = await _context.LikePlaylists.FirstOrDefaultAsync(x => x.UserId == userId && x.PlaylistId == playlistId);
            if (result == null)
            {
                return Result<LikePlaylist>.Failure("No liked playlist found");
            }
            return Result<LikePlaylist>.Success(result);
        }

        public async Task<Result<string>> DeleteLike(string userId, Guid playlistId)
        {
            var like = await _context.LikePlaylists.FirstOrDefaultAsync(x => x.UserId == userId && x.PlaylistId == playlistId);
            if (like == null)
            {
                return Result<string>.Failure("No liked playlist found");
            }
            _context.LikePlaylists.Remove(like);
            await _context.SaveChangesAsync();
            return Result<string>.Success("Playlist unliked");
        }

        public async Task<Result<List<LikePlaylist>>> GetByUserId(string userId)
        {
            var result = await _context.LikePlaylists.Where(x => x.UserId == userId).ToListAsync();
            if (result == null)
            {
                return Result<List<LikePlaylist>>.Failure("No liked playlists found");
            }
            return Result<List<LikePlaylist>>.Success(result);
        }
    }
}
