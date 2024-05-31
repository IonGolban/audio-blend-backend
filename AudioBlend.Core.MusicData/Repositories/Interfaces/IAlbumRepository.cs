using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.MusicData.Repositories.Interfaces
{
    public interface IAlbumRepository : IAsyncRepository<Album>
    {
        Task<Result<List<Album>>> GetAll();
        Task<Result<List<Album>>> GetLikedAlbumsByUserId(string userId);
        Task<Result<List<Album>>> GetRandomAlbums(int count);

        Task<Result<List<Album>>> GetRecommendedByGenreFromArtists(string genre, int count);
        Task<Result<List<Album>>> GetRecommendedByGenreFromSongs(string genre, int count);
        



    }
}
