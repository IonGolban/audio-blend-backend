using Microsoft.AspNetCore.Http;

namespace AudioBlend.Core.MusicData.Models.DTOs.Albums
{
    public class AddAlbumDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ArtistId { get; set; }
        public List<string> Genres { get; set; }
        public IFormFile CoverImage { get; set; }
        public List<IFormFile> Songs { get; set; }
        
    }
}
