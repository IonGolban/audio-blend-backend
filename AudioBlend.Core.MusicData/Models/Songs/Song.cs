using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Models.Likes;

namespace AudioBlend.Core.MusicData.Domain.Songs
{
    public class Song
    {
        public Guid Id { get;  set; }
        public string Title { get;  set; }
        public int Duration { get;  set; }
        public List<Guid> GenresIds{ get;  set; } = [];
        public Guid ArtistId { get;  set; }
        public Artist Artist { get;  set; }
        public Guid AlbumId { get;  set; }
        public Album Album { get;  set; }
        public string? AudioUrl { get;  set; } = string.Empty;
        public ICollection<LikeSong>? LikedByUsers { get;  set; }
    }
}
