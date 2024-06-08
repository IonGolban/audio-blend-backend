namespace AudioBlend.Core.MusicData.Models.DTOs.Playlists
{
    public class AddSongToPlaylistDto
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
        public AddSongToPlaylistDto() { }

    }
}
