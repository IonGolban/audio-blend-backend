using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class SongRepository(AudioBlendContext context) : BaseRepository<Song>(context), ISongRepository
    {
        private readonly AudioBlendContext _context = context;

        public async Task<Result<List<Song>>> GetByAlbumId(Guid albumId)
        {
            var result = await _context.Songs.Where(s => s.AlbumId == albumId).ToListAsync();
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


    }
}
