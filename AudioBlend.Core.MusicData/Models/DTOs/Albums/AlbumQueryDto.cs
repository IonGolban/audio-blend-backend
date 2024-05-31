using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;

namespace AudioBlend.Core.MusicData.Models.DTOs.Albums
{
    public class AlbumQueryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Type { get; set; }
        public string? CoverUrl { get; set; } = string.Empty;
        public List<SongQueryDto> Songs { get;  set; } = [];
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }

    }
}
