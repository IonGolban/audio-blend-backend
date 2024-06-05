using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class FollowArtistRepository(AudioBlendContext context) : BaseRepository<FollowArtist>(context), IFollowArtistRepository
    {
        private readonly AudioBlendContext _context = context;

        public async Task<Result<FollowArtist>> GetFollowArtist(string userId, Guid artistId)
        {
            var reponse = await _context.FollowArtists
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ArtistId == artistId);

            if (reponse == null)
            {
                return Result<FollowArtist>.Failure("follow artist does not exist");
            }

            return Result<FollowArtist>.Success(reponse);
        }

        public async Task<Result<FollowArtist>> FollowArtist(FollowArtist followArtist)
        {
            try
            {
                var followArtistIfExists = await _context.FollowArtists
                        .FirstOrDefaultAsync(x => x.UserId == followArtist.UserId && x.ArtistId == followArtist.ArtistId);

                if (followArtistIfExists != null)
                {
                    return Result<FollowArtist>.Failure("follow artist already exists");
                }

                var response = await _context.FollowArtists.AddAsync(followArtist);

                if (response != null) await _context.SaveChangesAsync();

                return Result<FollowArtist>.Success(followArtist);
            }
            catch (Exception e)
            {
                return Result<FollowArtist>.Failure(e.Message);
            }
        }
        public async Task<Result<FollowArtist>> UnfollowArtist(string userId, Guid artistId)
        {
            var followArtist = await _context.FollowArtists
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ArtistId == artistId);

            if (followArtist == null)
            {
                return Result<FollowArtist>.Failure("follow artist does not exist");
            }

            _context.FollowArtists.Remove(followArtist);
            await _context.SaveChangesAsync();

            return Result<FollowArtist>.Success(followArtist);
        }

        public async Task<Result<List<FollowArtist>>> GetByArtistId(Guid artistId)
        {
            var response = await _context.FollowArtists
                .Where(x => x.ArtistId == artistId)
                .ToListAsync();

            if (response.Count == 0)
            {
                return Result<List<FollowArtist>>.Failure("follow artist does not exist");
            }
            return Result<List<FollowArtist>>.Success(response);
        }

        public async Task<Result<List<FollowArtist>>> GetByUserId(string userId)
        {
            var response = await _context.FollowArtists
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (response.Count == 0)
            {
                return Result<List<FollowArtist>>.Failure("follow artist does not exist");
            }
            return Result<List<FollowArtist>>.Success(response);
        }
    }
}
