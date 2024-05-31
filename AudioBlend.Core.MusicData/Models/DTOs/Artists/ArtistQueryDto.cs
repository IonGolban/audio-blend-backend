namespace AudioBlend.Core.MusicData.Models.DTOs.Artists
{
    public class ArtistQueryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Genres { get; private set; }
        public int Followers { get; set; }
        public string UserId { get; private set; } = string.Empty;
    }
}
