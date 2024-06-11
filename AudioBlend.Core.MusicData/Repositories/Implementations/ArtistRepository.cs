using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class ArtistRepository(AudioBlendContext context) : BaseRepository<Artist>(context), IArtistRepository
    {
        private readonly AudioBlendContext _context = context;

        public async Task<Result<Artist>> getByUserId(string userId)
        {
            var result = await _context.Artists
                .FirstOrDefaultAsync(a => a.UserId == userId);
            if (result == null)
            {
                return Result<Artist>.Failure("No artist found");
            }

            return Result<Artist>.Success(result);
        }

        public async Task<Result<List<SearchRepoResult<Artist>>>> SearchByArtistName(string name, int treshold, int count)
        {
            var result = await _context.Artists
                .Select(a => new
                {
                    Artist = a,
                    Score = AudioBlendContext.LevenshteinDistance(name, a.Name)
                })
                .Where(a => a.Score <= treshold)
                .OrderBy(a => a.Score)
                .Take(count)
                .ToListAsync();

            if (result == null || result.Count == 0)
            {
                return Result<List<SearchRepoResult<Artist>>>.Failure("No artists found");
            }

            return Result<List<SearchRepoResult<Artist>>>.Success(
                result.Select(ar =>
                new SearchRepoResult<Artist>
                {
                    Result = ar.Artist,
                    Score = ar.Score
                }).ToList());

        }
    }
}
