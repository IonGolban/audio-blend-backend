using Microsoft.AspNetCore.Http;

namespace AudioBlend.Core.MusicData.Models.DTOs.Playlists
{
    public class UpdatePlaylistDto  
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; } = true;
        public IFormFile? Image { get; set; }
        public UpdatePlaylistDto() { }
    }
}
