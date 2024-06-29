using Microsoft.AspNetCore.Http;

namespace AudioBlend.Core.MusicData.Models.DTOs.Albums
{
    public class AlbumSongDto
    {
        public string Title { get; set; }
        public IFormFile Song { get; set; }
        public List<Guid> Genres { get; set; }

    }
}
