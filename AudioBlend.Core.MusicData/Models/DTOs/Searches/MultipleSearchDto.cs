using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;

namespace AudioBlend.Core.MusicData.Models.DTOs.Searches
{
    public class MultipleSearchDto
    {
        public List<AlbumQueryDto> Albums { get; set; }
        public List<SongQueryDto> Songs { get; set; }
        public List<Artist> Artists { get; set; }

    }
}
