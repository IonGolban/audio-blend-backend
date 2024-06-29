using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IGenreService
    {
        Task<Response<List<Genre>>> GetAllGenres();
        Task<Response<Genre>> GetGenreById(Guid id);
        Task<Response<Genre>> GetGenreByName(string name);
    }
}
