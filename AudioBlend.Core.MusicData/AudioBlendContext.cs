using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Models.Playlists;
using Microsoft.EntityFrameworkCore;

namespace AudioBlend.Core.MusicData
{
    public class AudioBlendContext : DbContext
    {
        public AudioBlendContext(DbContextOptions<AudioBlendContext> dbContextOptions) : base(dbContextOptions){}

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<LikeAlbum> LikeAlbums { get; set; }
        public DbSet<LikePlaylist> LikePlaylists { get; set; }
        public DbSet<LikeSong> LikeSongs { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<FollowArtist> FollowArtists { get; set; }

        [DbFunction("LEVENSHTEIN", IsBuiltIn = true)]
        public static int LevenshteinDistance(string s, string t) => throw new NotImplementedException();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("fuzzystrmatch");
            generateArtistModel(modelBuilder);
            generateAlbumModel(modelBuilder);
            generatePlaylistModel(modelBuilder);
            generateSongModel(modelBuilder);
            generateLikeModels(modelBuilder);
            
            generatePlaylistSongModel(modelBuilder);
            
            modelBuilder.HasDefaultSchema("music_data");
        }

        private void generatePlaylistSongModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistSong>().HasKey(ps => new { ps.PlaylistId, ps.SongId });
            modelBuilder.Entity<PlaylistSong>()
                        .HasOne(ps => ps.Playlist)
                        .WithMany(p => p.PlaylistSongs);
            modelBuilder.Entity<PlaylistSong>()
                        .HasOne(ps => ps.Song);
            modelBuilder.Entity<PlaylistSong>().ToTable("playlist_songs");
        }

        private void generateArtistModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().HasKey(a => a.Id);
            //modelBuilder.Entity<Artist>().HasMany(a => a.Albums).WithOne(a => a.Artist).HasForeignKey(a => a.ArtistId);
            //modelBuilder.Entity<Artist>().HasMany(a => a.Songs).WithOne(s => s.Artist).HasForeignKey(s => s.ArtistId);
            modelBuilder.Entity<Artist>().ToTable("artists");
        }

        private void generateLikeModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LikeAlbum>().HasKey(l => new { l.UserId, l.AlbumId });
            modelBuilder.Entity<LikeAlbum>()
                        .HasOne(l => l.Album)
                        .WithMany(a => a.LikedByUsers)
                        .HasForeignKey(l => l.AlbumId);
            modelBuilder.Entity<LikeAlbum>().ToTable("like_albums");

            modelBuilder.Entity<LikePlaylist>().HasKey(l => new { l.UserId, l.PlaylistId });
            modelBuilder.Entity<LikePlaylist>()
                        .HasOne(l => l.Playlist)
                        .WithMany(p => p.LikedByUsers)
                        .HasForeignKey(l => l.PlaylistId);
            modelBuilder.Entity<LikePlaylist>().ToTable("like_playlists");

            modelBuilder.Entity<LikeSong>().HasKey(l => new { l.UserId, l.SongId });
            modelBuilder.Entity<LikeSong>()
                        .HasOne(l => l.Song)
                        .WithMany(s => s.LikedByUsers)
                        .HasForeignKey(l => l.SongId);
            modelBuilder.Entity<LikeSong>().ToTable("like_songs");

            modelBuilder.Entity<FollowArtist>().HasKey(f => new { f.UserId, f.ArtistId });
            modelBuilder.Entity<FollowArtist>()
                        .HasOne(f => f.Artist)
                        .WithMany(a => a.FollowedByUsers)
                        .HasForeignKey(f => f.ArtistId);
            modelBuilder.Entity<FollowArtist>().ToTable("follow_artists");
        }


        private void generateSongModel(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Song>().HasKey(s => s.Id);
            modelBuilder.Entity<Song>().HasMany(s => s.LikedByUsers).WithOne(l => l.Song).HasForeignKey(l => l.SongId);
            modelBuilder.Entity<Song>().HasOne(s => s.Artist);
            modelBuilder.Entity<Song>().ToTable("songs");

        }

        private void generatePlaylistModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>().HasKey(p => p.Id);
            modelBuilder.Entity<Playlist>().HasMany(p => p.LikedByUsers).WithOne(l => l.Playlist).HasForeignKey(l => l.PlaylistId);
            modelBuilder.Entity<Playlist>().HasMany(p => p.PlaylistSongs).WithOne(ps => ps.Playlist).HasForeignKey(ps => ps.PlaylistId);
            modelBuilder.Entity<Playlist>().ToTable("playlists");
        }


        private void generateAlbumModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>().HasKey(a => a.Id);
            modelBuilder.Entity<Album>().HasMany(a => a.Songs).WithOne(s => s.Album).HasForeignKey(s => s.AlbumId);
            modelBuilder.Entity<Album>().HasMany(a => a.LikedByUsers).WithOne(l => l.Album).HasForeignKey(l => l.AlbumId);
            modelBuilder.Entity<Album>().HasOne(a => a.Artist);
            modelBuilder.Entity<Album>().ToTable("albums");
        }
    }
}
