using Microsoft.AspNetCore.Http;

namespace AudioBlend.Core.MusicData.Models.DTOs.Playlists
{
    public class CreatePlaylistDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsPublic { get; set; } = true;
        public CreatePlaylistDto() { }
    }
}
