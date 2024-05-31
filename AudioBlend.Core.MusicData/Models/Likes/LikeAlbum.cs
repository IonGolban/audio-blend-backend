using AudioBlend.Core.MusicData.Domain.Albums;

namespace AudioBlend.Core.MusicData.Models.Likes
{
    public class LikeAlbum : Like
    {
        public Guid AlbumId { get; private set; }
        public Album Album { get; private set; }
        public LikeAlbum() { }

        public LikeAlbum(string userId, Guid albumId) : base(userId)
        {
            AlbumId = albumId;
        }
    }
}
