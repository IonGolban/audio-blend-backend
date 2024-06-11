using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.Genres;

namespace AudioBlend.Core.MusicData.Models.DTOs.Songs
{
    public class SongQueryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }
        public Guid AlbumId { get; set; }
        public string AlbumName { get; set; }
        public List<Genre> Genres { get; set; }
        public string CoverUrl { get; set; }
        public string AudioUrl { get; set; }
        public int Duration { get; set; }
        public int Likes { get; set; }

        public SongQueryDto()
        {
        }
    }
}
