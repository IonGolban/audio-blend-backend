using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class GenreRepository(AudioBlendContext context) : BaseRepository<Genre>(context), IGenreRepository
    {
        private readonly AudioBlendContext _context = context;
        public async Task<Result<List<Genre>>> GetByAlbumId(Guid albumId)
        {

            var songs = await _context.Songs.Where(x => x.AlbumId == albumId).ToListAsync();
            if (songs == null)
            {
                return Result<List<Genre>>.Failure("Songs not found");
            }

            var result = new List<Genre>();
            foreach (var song in songs)
            {
                foreach (var genre in song.GenresIds)
                {
                    var genreResult = await _context.Genres.FirstOrDefaultAsync(x => x.Id == genre);
                    if (genreResult == null)
                    {
                        return Result<List<Genre>>.Failure("Genre not found");
                    }
                    result.Add(genreResult);
                }
            }

            return Result<List<Genre>>.Success(result);

        }

        public async Task<Result<List<Genre>>> GetByArtistId(Guid artistId)
        {
           
            var artist = await _context.Artists.FindAsync(artistId);

            var result = new List<Genre>();
            foreach (var genre in artist.GenresIds)
            {
                var genreResult = await _context.Genres.FirstOrDefaultAsync(x => x.Id == genre);
                if (genreResult == null)
                {
                    return Result<List<Genre>>.Failure("Genre not found");
                }
                result.Add(genreResult);
            }

            return Result<List<Genre>>.Success(result);
        }
        public async Task<Result<List<Genre>>> GetByMultipleIds(List<Guid> ids)
        {
            var genres = await _context.Genres.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (genres == null)
            {
                return Result<List<Genre>>.Failure("Genres not found");
            }

            return Result<List<Genre>>.Success(genres);
        }

        public async Task<Result<Genre>> GetByName(string name)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Name == name);
            if (genre == null)
            {
                return Result<Genre>.Failure("Genre not found");
            }
            return Result<Genre>.Success(genre);
        }

        public async Task<Result<List<Genre>>> GetBySongId(Guid songId)
        {
            var song = await _context.Songs.FindAsync(songId);
            if (song == null)
            {
                return Result<List<Genre>>.Failure("Song not found");
            }
            var result = new List<Genre>();
            foreach (var genre in song.GenresIds)
            {
                var genreResult = await _context.Genres.FindAsync(genre);
                if (genreResult == null)
                {
                    return Result<List<Genre>>.Failure("Genre not found");
                }
                result.Add(genreResult);
            }

            return Result<List<Genre>>.Success(result);
        }
    }
}
