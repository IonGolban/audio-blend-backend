using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Implementations;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class AlbumRepository(AudioBlendContext context, ISongRepository songRepository) : BaseRepository<Album>(context), IAlbumRepository
    {
        private readonly AudioBlendContext _context = context;
        private readonly ISongRepository _songRepository = songRepository;

        public async override Task<Result<Album>> GetByIdAsync(Guid id)
        {

            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Include(a => a.LikedByUsers)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
            {
                return Result<Album>.Failure("Album not found");
            }

            return Result<Album>.Success(album);
        }

        public async Task<Result<List<Album>>> GetLikedAlbumsByUserId(string userId)
        {
            var likedAlbums = await _context.LikeAlbums
                .Where(la => la.UserId == userId)
                .Include(la => la.Album)

                .ThenInclude(a => a.Artist)
                .Include(la => la.Album.Songs)
                .Include(la => la.Album.LikedByUsers)
                .ToListAsync();

            if (likedAlbums == null)
            {
                return Result<List<Album>>.Failure("No liked albums found");
            }

            return Result<List<Album>>.Success(likedAlbums.Select(la => la.Album).ToList());


        }

        public async Task<Result<List<Album>>> GetRandomAlbums(int count)
        {
            var randomAlbums = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Include(a => a.LikedByUsers)
                .OrderBy(a => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            return Result<List<Album>>.Success(randomAlbums);
        }

        public async Task<Result<List<Album>>> GetRecommendedByGenreFromArtists(string genre, int count)
        {
            var result = await _context.Albums
                .Include (a => a.Songs)
                .Include (a => a.LikedByUsers)
                .Include(a => a.Artist)
                .Where(a => a.Artist.Genres.Any( g => g.Equals(genre)))
                .OrderBy(a => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            return Result<List<Album>>.Success(result);
            
        }
        public async Task<Result<List<Album>>> GetRecommendedByGenreFromSongs(string genre, int count)
        {

            var songs = await _songRepository.GetByGenre(genre, 10);
            if (!songs.IsSuccess)
            {
                songs = await _songRepository.GetRandom(10);
                if (!songs.IsSuccess)
                { 
                
                    return Result<List<Album>>.Failure("No songs found");
                }
            }
            
            var result = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Include(a => a.LikedByUsers)
                .Where(a => a.Songs.Any(s => songs.Value.Select(s => s.Id).Contains(s.Id)))
                .OrderBy(a => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            if (result == null)
            {
                return Result<List<Album>>.Failure("No albums found");
            }

            return Result<List<Album>>.Success(result);

        }
        public async Task<Result<List<Album>>> GetAll()
        {
            var result = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Include(a => a.LikedByUsers)
                .ToListAsync();

            if (result == null)
            {
                return Result<List<Album>>.Failure("No albums found");
            }

            return Result<List<Album>>.Success(result);

        }
    }
}
