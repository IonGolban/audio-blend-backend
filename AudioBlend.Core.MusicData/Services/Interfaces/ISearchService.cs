using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface ISearchService
    {
        Task<Response<MultipleSearchDto>> SearchAll(string query, int count);
        Task<Response<List<AlbumQueryDto>>> SearchAlbums(string query, int count);
        Task<Response<List<SongQueryDto>>> SearchSongs(string query, int count);
        Task<Response<List<Artist>>> SearchArtists(string query, int count);

    }
}
