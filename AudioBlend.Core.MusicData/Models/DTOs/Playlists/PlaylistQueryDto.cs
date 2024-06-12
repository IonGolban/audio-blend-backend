using AudioBlend.Core.MusicData.Models.DTOs.Songs;
namespace AudioBlend.Core.MusicData.Models.DTOs.Playlists
{
    public class PlaylistQueryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsPublic { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? CoverUrl { get; set; }
        public string Description { get; set; }
        public int Likes { get; set; }
        public List<SongQueryDto> Songs { get;  set; } = [];

    }
}
