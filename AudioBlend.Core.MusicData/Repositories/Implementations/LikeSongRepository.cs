using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class LikeSongRepository(AudioBlendContext context) : BaseRepository<LikeSong>(context), ILikeSongRepository
    {
        private readonly AudioBlendContext _context = context;

        public async Task<Result<LikeSong>> GetLikeSong(string userId, Guid songId)
        {
            try
            {
                var result = await _context.LikeSongs.FirstOrDefaultAsync(x => x.UserId == userId && x.SongId == songId);
                if (result == null)
                {
                    return Result<LikeSong>.Failure("No liked song found");
                }
                return Result<LikeSong>.Success(result);
            }
            catch (Exception e)
            {
                return Result<LikeSong>.Failure(e.Message);
            }

        }

        public async Task<Result<string>> DeleteLike(string userId, Guid songId)
        {
            try
            {
                var like = await _context.LikeSongs.FirstOrDefaultAsync(x => x.UserId == userId && x.SongId == songId);
                if (like == null)
                {
                    return Result<string>.Failure("No liked playlist found");
                }
                _context.LikeSongs.Remove(like);
                await _context.SaveChangesAsync();
                return Result<string>.Success("Playlist unliked");

            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<LikeSong>>> GetTopSongs(int count)
        {
            try
            {
                var topSongs = await _context.LikeSongs
                    .GroupBy(ls => ls.SongId)
                    .Select(g => new
                    {
                        SongId = g.Key,
                        LikesCount = g.Count()
                    })
                    .OrderByDescending(x => x.LikesCount)
                    .Take(count)
                    .ToListAsync();

                var topLikeSongs = await _context.LikeSongs
                    .Where(ls => topSongs.Select(ts => ts.SongId).Contains(ls.SongId))
                    .ToListAsync();

                if (topLikeSongs == null)
                {
                    return Result<List<LikeSong>>.Failure("No liked songs found");
                }

                return Result<List<LikeSong>>.Success(topLikeSongs);
            }
            catch (Exception ex)
            {
                return Result<List<LikeSong>>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<LikeSong>>> GetLikedByUser(string userId)
        {
            var result = await _context.LikeSongs.Where(ls => ls.UserId == userId).ToListAsync();
            if (result == null)
            {
                return Result<List<LikeSong>>.Failure("No liked songs found");
            }

            return Result<List<LikeSong>>.Success(result);
        }

    }
}

