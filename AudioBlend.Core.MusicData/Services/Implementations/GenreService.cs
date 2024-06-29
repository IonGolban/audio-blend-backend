using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Repositories.Implementations;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class GenreService(IGenreRepository genreRepository) : IGenreService
    {
        private readonly IGenreRepository _genreRepository = genreRepository;

        public async Task<Response<List<Genre>>> GetAllGenres()
        {
            var result = await _genreRepository.GetAll();

            if (!result.IsSuccess)
            {
                return new Response<List<Genre>>()
                {
                    Success = false,
                    Message = "Error while getting all genres"
                };
            }

            return new Response<List<Genre>>()
            {
                Data = result.Value.ToList(),
                Success = true
            };

        }

        public async Task<Response<Genre>> GetGenreById(Guid id)
        {
            var result = await _genreRepository.GetByIdAsync(id);
            if (!result.IsSuccess) {
                return new Response<Genre>()
                {
                    Success = false,
                    Message = "Genre does not exists"
                };
            }
            return new Response<Genre>()
            {
                Data = result.Value,
                Success = true
            };
        }

        public async Task<Response<Genre>> GetGenreByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
