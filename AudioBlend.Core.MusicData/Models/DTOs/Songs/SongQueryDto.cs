using AudioBlend.Core.MusicData.Domain.Songs;

namespace AudioBlend.Core.MusicData.Models.DTOs.Songs
{
    public class SongQueryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }
        public Guid AlbumId { get; set; }
        public string AlbumName { get; set; }
        public List<string> Genres { get; set; }
        public string CoverUrl { get; set; }
        public string AudioUrl { get; set; }
        public int Duration { get; set; }
        public int Likes { get; set; }

        public SongQueryDto(Song song)
        {
            Id = song.Id;
            Title = song.Title;
            ArtistId = song.ArtistId;
            ArtistName = song.Artist.Name;
            AlbumId = song.AlbumId;
            AlbumName = song.Album.Title;
            Genres = song.Genres;
            CoverUrl = song.Album.CoverUrl;
            AudioUrl = song.AudioUrl;
            Duration = song.Duration;
        }

        public SongQueryDto()
        {
        }
    }
}
