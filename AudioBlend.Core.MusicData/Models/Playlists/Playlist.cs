﻿
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Models.Playlists;

namespace AudioBlend.Core.MusicData.Domain.Playlists
{
    public class Playlist
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public bool IsPublic { get; private set; }
        public string UserId{ get; private set; }
        public ICollection<LikePlaylist> LikedByUsers { get; private set; } = [];
        public ICollection<PlaylistSong> playlistSongs { get; private set; } = [];

        public Playlist(Guid id, string title, bool isPublic, string userId)
        {
            Id = id;
            Title = title;
            IsPublic = isPublic;
            UserId = userId;
            
        }   
    }
}