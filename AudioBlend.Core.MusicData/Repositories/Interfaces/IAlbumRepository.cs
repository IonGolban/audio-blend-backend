using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IAlbumRepository : IAsyncRepository<Album>
    {
        Task<Result<List<Album>>> GetAll();
        Task<Result<List<Album>>> GetLikedAlbumsByUserId(string userId);
        Task<Result<List<Album>>> GetRandomAlbums(int count);
        Task<Result<List<Album>>> GetByArtistId(Guid id);
        Task<Result<List<Album>>> GetRecommendedByGenreFromArtists(Guid genre, int count);
        Task<Result<List<Album>>> GetRecommendedByGenreFromSongs(Guid genre, int count);

        Task<Result<List<SearchRepoResult<Album>>>> SearchByAlbumName(string albumName, int treshold, int count);





    }
}
