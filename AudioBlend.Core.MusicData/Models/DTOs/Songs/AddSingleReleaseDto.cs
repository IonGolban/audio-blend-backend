using Microsoft.AspNetCore.Http;

namespace AudioBlend.Core.MusicData.Models.DTOs.Songs
{
    public class AddSingleReleaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Genres { get; set; }        
        public IFormFile CoverImage { get;set; }
        public IFormFile Song { get; set; }

    }
}
