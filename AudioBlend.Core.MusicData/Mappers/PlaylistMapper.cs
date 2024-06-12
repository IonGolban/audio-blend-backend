using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Models.DTOs.Playlists;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;

namespace AudioBlend.Core.MusicData.Mappers
{
    public class PlaylistMapper
    {
        public static PlaylistQueryDto MapToQueryDto(Playlist playlist, List<SongQueryDto> songs)
        {
            return new PlaylistQueryDto()
            {
                Id = playlist.Id,
                Title = playlist.Title,
                IsPublic = playlist.IsPublic,
                UserId = playlist.UserId,
                CoverUrl = playlist.CoverUrl,
                Description = playlist.Description,
                Likes = playlist.Likes,
                Songs = songs
            };
        }
    }
}
