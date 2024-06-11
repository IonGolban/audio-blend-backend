using AudioBlend.Core.MusicData.Models.Genres;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<Genre>> GetRecommendationGenres(string id, int count);
    }
}
