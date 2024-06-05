using AudioBlend.Core.MusicData.Domain.Artists;

namespace AudioBlend.Core.MusicData.Models.Likes
{
    public class FollowArtist : Like
    {
        public Guid ArtistId { get; private set; }
        public Artist Artist { get; private set; }
        public FollowArtist(string userId, Guid artistId) : base(userId)
        {
            ArtistId = artistId;
        }
    }
}
