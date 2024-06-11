using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Implementations;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class AlbumRepository(AudioBlendContext context, ISongRepository songRepository, IGenreRepository genreRepository) : BaseRepository<Album>(context), IAlbumRepository
    {
        private readonly AudioBlendContext _context = context;
        private readonly ISongRepository _songRepository = songRepository;
        private IGenreRepository _genreRepository = genreRepository;

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

        public async Task<Result<List<Album>>> GetRecommendedByGenreFromArtists(Guid genre, int count)
        {
            var result = await _context.Albums
                .Include(a => a.Songs)
                .Include(a => a.LikedByUsers)
                .Include(a => a.Artist)
                .Where(a => a.Artist.GenresIds.Contains(genre))
                .OrderBy(a => Guid.NewGuid())
                .Take(count)
                .ToListAsync();

            return Result<List<Album>>.Success(result);
            
        }
        public async Task<Result<List<Album>>> GetRecommendedByGenreFromSongs(Guid genre, int count)
        {
            if(genre == null)
            {
                return Result<List<Album>>.Failure("Genre is empty");
            }

            var genreEntity = await _genreRepository.GetByIdAsync(genre);
            if (!genreEntity.IsSuccess)
            {
                return Result<List<Album>>.Failure("Genre not found");
            }

            var songs = await _songRepository.GetByGenre(genreEntity.Value.Id, 10);
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

        public async Task<Result<List<SearchRepoResult<Album>>>> SearchByAlbumName(string albumName, int treshold, int count)
        {
            var result = await _context.Albums
                .Include(a => a.Artist)
                .Select(a => new
                {
                    Album = a,
                    Score = AudioBlendContext.LevenshteinDistance(a.Title, albumName)
                })
                .Where(a => a.Score <= treshold)
                .OrderBy(a => a.Score)
                .Take(count)
                .ToListAsync();

            if(result.Count == 0)
            {
                return Result<List<SearchRepoResult<Album>>>.Failure("No albums found");
            }

            return Result<List<SearchRepoResult<Album>>>.Success(result
                .Select(a => new SearchRepoResult<Album> { 
                    Result = a.Album, Score = a.Score })
                .ToList());
        }

        public async Task<Result<List<Album>>> GetByArtistId(Guid id)
        {
            var result = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Include(a => a.LikedByUsers)
                .Where(a => a.ArtistId == id)
                .ToListAsync();

            if (result == null)
            {
                return Result<List<Album>>.Failure("No albums found");
            }

            return Result<List<Album>>.Success(result);
        }
    }
}
