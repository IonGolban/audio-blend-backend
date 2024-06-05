using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface ISongService
    {
        Task<Response<List<SongQueryDto>>> GetTopSongs(int count);
        Task<Response<List<SongQueryDto>>> GetRandomSongs(int count);
        Task<Response<List<SongQueryDto>>> GetRecommendations(int count);
        Task<Response<List<SongQueryDto>>> GetByGenre(string genre, int count);
        Task<Response<List<SongQueryDto>>> GetByGenres(List<string> genres, int count);
        Task<Response<List<SongQueryDto>>> GetByArtistId(Guid artistId);
        Task<Response<List<SongQueryDto>>> GetLikedSongs(string userId);
    }
}
