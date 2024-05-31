namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<string>> GetRecommendationGenres(string id, int count);
    }
}
