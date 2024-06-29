using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.Likes;

namespace AudioBlend.Core.MusicData.Domain.Albums
{
    public class Album
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string Type { get; private set; }
        public string? CoverUrl { get; private set; } = string.Empty;
        public List<Song> Songs { get; private set; } = [];
        public Guid ArtistId { get; private set; }
        public Artist Artist { get; private set; }
        public List<LikeAlbum>? LikedByUsers { get; private set; } = [];
        public Album(Guid id, string title, string description, string type, string coverUrl, Guid artistId, DateTime releaseDate)
        {
            Id = id;
            Title = title;
            Description = description;
            Type = type;
            CoverUrl = coverUrl;
            ArtistId = artistId;
            ReleaseDate = releaseDate;
        }

        public Album(Guid id, string title, string description, string type, string coverUrl, DateTime releaseDate)
        {
            Id = id;
            Title = title;
            Description = description;
            Type = type;
            CoverUrl = coverUrl;
            ReleaseDate = releaseDate;
        }


        public void AddSong(Song song)
        {
            Songs.Add(song);
        }

        public void RemoveSong(Song song)
        {
            Songs.Remove(song);
        }

        public void AddSongRange(List<Song> songs)
        {
            Songs.AddRange(songs);
        }
        
    }
}
