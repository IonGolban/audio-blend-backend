using AudioBlend.Core.MusicData.Domain.Playlists;

namespace AudioBlend.Core.MusicData.Models.Likes
{
    public class LikePlaylist : Like
    {
        public Guid PlaylistId { get; private set; }
        public Playlist Playlist { get; private set; }
        public LikePlaylist() { }

        public LikePlaylist(string userId, Guid playlistId) : base(userId)
        {
            PlaylistId = playlistId;
        }
    }
}
