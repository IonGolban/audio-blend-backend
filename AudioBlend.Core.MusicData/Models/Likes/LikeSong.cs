using AudioBlend.Core.MusicData.Domain.Songs;

namespace AudioBlend.Core.MusicData.Models.Likes
{
    public class LikeSong : Like
    {
        public Guid SongId { get; private set; }
        public Song Song { get; private set; }
        public LikeSong() { }

        public LikeSong(string userId, Guid songId) : base(userId)
        {
            SongId = songId;
        }
    }
}
