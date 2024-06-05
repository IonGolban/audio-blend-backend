using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Artists;

namespace AudioBlend.Core.MusicData.Mappers
{
    public class ArtistMapper
    {
        public static ArtistQueryDto MapToArtistQueryDto(Artist artist)
        {
            return new ArtistQueryDto
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImgUrl,
                Genres = artist.Genres,
                UserId = artist.UserId,
                Followers = artist.Followers,
            };
        }
    }
}
