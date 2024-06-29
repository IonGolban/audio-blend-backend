using AudioBlend.Core.MusicData.Models.Genres;

namespace AudioBlend.Core.MusicData.Models.DTOs
{
    public class GenresQueryDto
    {
        
        public List<Guid> GenresIds { get; set; }

        public GenresQueryDto() { }
    }
}
