using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class LikeAlbumRepository(AudioBlendContext context) : BaseRepository<LikeAlbum>(context), ILikeAlbumRepository
    {
        private readonly AudioBlendContext _context = context;

        public async Task<Result<string>> DeleteLike(string userId, Guid albumId)
        {
            var likeAlbum = await _context.LikeAlbums.FirstOrDefaultAsync(x => x.UserId == userId && x.AlbumId == albumId);

            if (likeAlbum == null)
            {
                return Result<string>.Failure("Like not found");
            }

            _context.LikeAlbums.Remove(likeAlbum);

            await _context.SaveChangesAsync();

            return Result<string>.Success("Like deleted successfully");
        }

        public async Task<Result<List<LikeAlbum>>> GetByUserId(string userId)
        {
            var result = await _context.LikeAlbums.Where(x => x.UserId == userId).ToListAsync();
           
            if (result == null)
            {
                return Result<List<LikeAlbum>>.Failure("No liked albums found");
            }
            return Result<List<LikeAlbum>>.Success(result);
        }

        public async Task<Result<LikeAlbum>> GetLikeAlbum(string userId, Guid albumId)
        {
            var result = await _context.LikeAlbums.FirstOrDefaultAsync(x => x.UserId == userId && x.AlbumId == albumId);
            
            if (result == null)
            {
                return Result<LikeAlbum>.Failure("No liked album found");
            }
            return Result<LikeAlbum>.Success(result);
        }
    }
}
