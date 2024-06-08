namespace AudioBlend.Core.MusicData.Models.DTOs.Playlists
{
    public class RemoveSongToPlaylistDto
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
        public RemoveSongToPlaylistDto() { }
    }
}
