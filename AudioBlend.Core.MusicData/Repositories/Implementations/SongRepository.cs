using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class SongRepository(AudioBlendContext context) : BaseRepository<Song>(context), ISongRepository
    {
        private readonly AudioBlendContext _context = context;

        public override async Task<Result<Song>> GetByIdAsync(Guid id)
        {
            var result = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (result == null)
            {
                return Result<Song>.Failure("No song found");
            }
            return Result<Song>.Success(result);
        }
        public async Task<Result<List<Song>>> GetByAlbumId(Guid albumId)
        {
            var result = await _context.Songs.Where(s => s.AlbumId == albumId).ToListAsync();
            if (result == null)
            {
                return Result<List<Song>>.Failure("No songs found");
            }
            return Result<List<Song>>.Success(result);
        }

        public async Task<Result<List<Song>>> GetByArtistId(Guid artistId)
        {
            var result = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Where(s => s.ArtistId == artistId).ToListAsync();
            if (result == null)
            {
                return Result<List<Song>>.Failure("No songs found");
            }
            return Result<List<Song>>.Success(result);
        }

        public async Task<Result<List<Song>>> GetByGenre(string genre, int count)
        {
            var songs = await _context.Songs
                .Where(s => s.Genres.Contains(genre))
                .OrderBy(s => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            if (songs == null)
            {
                return Result<List<Song>>.Failure("No songs found");
            }
            return Result<List<Song>>.Success(songs);
        }

        public async Task<Result<List<Song>>> GetRandom(int count)
        {
            var songs = await _context.Songs
                .OrderBy(s => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            if (songs.Count == 0)
            {
                return Result<List<Song>>.Failure("No songs found");
            }

            return Result<List<Song>>.Success(songs);
        }

        public async Task<Result<List<SearchRepoResult<Song>>>> SearchBySongName(string name, int treshold, int count)
        {
            var result = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Select(s=> new
                {
                    Song = s,
                    Score = AudioBlendContext.LevenshteinDistance(name, s.Title)
                })
                .Where(s => s.Score <= treshold)
                .OrderBy(s => s.Score)
                .Take(count)
                .ToListAsync();

            if (result.Count == 0)
            {
                return Result<List<SearchRepoResult<Song>>>.Failure("No songs found");
            }

            return Result<List<SearchRepoResult<Song>>>.Success(result.Select(s => new SearchRepoResult<Song>
            {
                Result = s.Song,
                Score = s.Score
            }).ToList());
        }


    }
}
