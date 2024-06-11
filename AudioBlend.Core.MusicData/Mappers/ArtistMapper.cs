using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Artists;
using AudioBlend.Core.MusicData.Models.Genres;

namespace AudioBlend.Core.MusicData.Mappers
{
    public class ArtistMapper
    {
        public static ArtistQueryDto MapToArtistQueryDto(Artist artist, List<Genre> genres)
        {
            return new ArtistQueryDto
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImgUrl,
                Genres = genres,
                UserId = artist.UserId,
                Followers = artist.Followers,
            };
        }
    }
}
