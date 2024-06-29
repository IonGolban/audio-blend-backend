using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface IAlbumCommandService
    {
        public Task<Response<Album>> AddAlbum(AddAlbumDto albumDto);
        public Task<Response<Album>> AddSingleRelease(AddSingleReleaseDto singleReleaseDto);
        public Task<Response<Album>> DeleteAlbum(Guid albumId);

    }
}
