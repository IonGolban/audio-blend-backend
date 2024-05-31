using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Domain.Songs;

namespace AudioBlend.Core.MusicData.Models.Playlists
{
    public class PlaylistSong
    {
        public Guid PlaylistId { get; private set; }
        public Playlist Playlist { get; private set; }
        public Guid SongId { get; private set; }
        public Song Song { get; private set; }
        public PlaylistSong(Guid playlistId, Guid songId)
        {
            PlaylistId = playlistId;
            SongId = songId;

        }

        public PlaylistSong(Guid playlistId, Guid songId, Song song, Playlist playlist)
        {
            PlaylistId = playlistId;
            SongId = songId;
            Playlist = playlist;
            Song = song;
        }
    }
}
