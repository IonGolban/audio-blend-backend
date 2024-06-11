using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Models.Playlists;
using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.UserAccess.Models.Users;
using Microsoft.AspNetCore.Identity;
using AudioBlend.API.DbInitializer.Models;
using AudioBlend.Core.MusicData;
using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Domain.Artists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioBlend.Core.UserAccess.Models.Roles;
using AudioBlend.Core.UserAccess;
using AudioBlend.Core.MusicData.Models.Genres;

namespace AudioBlend.API.DbInitializer
{
    public static class DbInitMusicData
    {
        private const string PATH_DATA = "C:\\Users\\uig26544\\Desktop\\Copyright-Free-Music\\info\\songs_info1.json";
        private const string PATH_IMAGES = "C:\\Users\\uig26544\\Desktop\\Copyright-Free-Music";
        private const string UNKNOWN_USER_IMG = "https://golbanionstorage.blob.core.windows.net/default/unknown_user.jpg";

        private static List<Artist> usedArtists = new List<Artist>();
        private static List<string> songsUrl = new List<string>();

        public async static Task InitData(IServiceProvider serviceProvider)
        {
            songsUrl = SongBlobUrls.GetUrls();
            var users = await GenerateRandomUsers(serviceProvider);

            var music_data = MusicDataReader<RootJsonModel>.ReadJsonFile(PATH_DATA);
            var genres_db = ConvertToGenreDbModels(music_data, serviceProvider);
            var artists_db = ConvertToArtistDbModels(music_data, serviceProvider, genres_db);
            var albums_db = ConvertToAlbumDbModels(music_data, serviceProvider, artists_db);
            albums_db = RemoveDuplicateAlbums(albums_db);
            var songs_db = ConvertToSongDbModels(music_data, serviceProvider, artists_db, albums_db, genres_db);
            songs_db = RemoveDuplicateSongs(songs_db);
            albums_db = RemoveAlbumsWithoutSongs(albums_db, songs_db);

            var playlistsDb = GenerateRandomPlaylists(songs_db, users);
            var playlistSongsDb = GenerateRandomPlaylistSongs(playlistsDb, songs_db);

            var likes_album = GenerateRandomLikesAlbum(albums_db, users);
            var likes_songs = GenerateRandomLikesSong(songs_db, users);
            var likes_playlist = GenerateRandomLikesPlaylist(playlistsDb, users);
            var follows_artists = GenerateRandomFollowsArtist(artists_db, users);

            await SaveModelsDb(serviceProvider, genres_db, artists_db, albums_db, songs_db, playlistsDb, likes_album, likes_playlist, likes_songs, playlistSongsDb, follows_artists);
        }

        private static async Task SaveModelsDb(IServiceProvider serviceProvider, List<Genre> genres_db, List<Artist> artists_db, List<Album> albums_db, List<Song> songs_db, List<Playlist> playlistsDb, List<LikeAlbum> likes_album, List<LikePlaylist> likes_playlist, List<LikeSong> likes_songs, List<PlaylistSong> playlistSongsDb, List<FollowArtist> follows_artists)
        {
            var context = serviceProvider.GetRequiredService<AudioBlendContext>();

            await context.Genres.AddRangeAsync(genres_db);
            await context.Artists.AddRangeAsync(artists_db);
            await context.Albums.AddRangeAsync(albums_db);
            await context.Songs.AddRangeAsync(songs_db);
            await context.Playlists.AddRangeAsync(playlistsDb);
            await context.LikeAlbums.AddRangeAsync(likes_album);
            await context.LikePlaylists.AddRangeAsync(likes_playlist);
            await context.LikeSongs.AddRangeAsync(likes_songs);
            await context.PlaylistSongs.AddRangeAsync(playlistSongsDb);
            await context.FollowArtists.AddRangeAsync(follows_artists);
            await context.SaveChangesAsync();
        }

        private static List<Genre> ConvertToGenreDbModels(RootJsonModel music_data, IServiceProvider serviceProvider)
        {
            List<Genre> genres_db = new List<Genre>();

            foreach (var song in music_data.Songs)
            {
                foreach (var artist in song.Artists)
                {
                    foreach (var genre in artist.Genres)
                    {
                        if (!genres_db.Any(g => g.Name == genre))
                        {
                            genres_db.Add(new Genre { Id = Guid.NewGuid(), Name = genre });
                        }
                    }
                }
            }

            return genres_db;
        }

        private static List<Artist> ConvertToArtistDbModels(RootJsonModel rootModel, IServiceProvider serviceProvider, List<Genre> genres_db)
        {
            List<Artist> artistDbModels = new List<Artist>();
            var rnd = new Random();
            foreach (var song in rootModel.Songs)
            {
                foreach (var artist in song.Artists)
                {
                    if (artistDbModels.Any(a => a.Name == artist.Name))
                    {
                        continue;
                    }
                    var artistId = Guid.NewGuid();
                    var artistGenres = genres_db.Where(g => artist.Genres.Contains(g.Name)).Select(g => g.Id).ToList();
                    var artistDbModel = new Artist(artistId, artist.Name, artist.Img, artistGenres , artist.Followers);
                    artistDbModels.Add(artistDbModel);
                }
            }

            foreach (var song in rootModel.Songs)
            {
                foreach (var artist in song.Artists)
                {
                    foreach (var album in artist.Albums)
                    {
                        foreach (var track in album.Tracks)
                        {
                            foreach (var track_artist in track.Artists)
                            {
                                if (!artistDbModels.Any(a => a.Name == track_artist))
                                {
                                    var randomGenres = genres_db.OrderBy(g => rnd.Next()).Take(rnd.Next(1, 3)).Select(g => g.Id).ToList();
                                    artistDbModels.Add(new Artist(Guid.NewGuid(), track_artist, UNKNOWN_USER_IMG, randomGenres));
                                }
                            }
                        }
                    }
                }
            }
            return artistDbModels;
        }

        private static List<Album> ConvertToAlbumDbModels(RootJsonModel rootModel, IServiceProvider serviceProvider, List<Artist> artists)
        {
            List<Album> albumDbModels = new List<Album>();

            foreach (var song in rootModel.Songs)
            {
                foreach (var artist in song.Artists)
                {
                    foreach (var album in artist.Albums)
                    {
                        if (albumDbModels.Any(a => a.Title == album.Name))
                        {
                            continue;
                        }
                        DateTime releaseDate = string.IsNullOrEmpty(album.ReleaseDate) ? DateTime.MinValue : DateTime.Parse(album.ReleaseDate);
                        var artistDb = artists.FirstOrDefault(a => a.Name == artist.Name);
                        var albumDbModel = new Album(Guid.NewGuid(), album.Name, string.Empty, album.Type, album.img_url, artistDb.Id, releaseDate);
                        albumDbModels.Add(albumDbModel);
                    }
                }
            }

            return albumDbModels;
        }

        private static List<Song> ConvertToSongDbModels(RootJsonModel music_data, IServiceProvider serviceProvider, List<Artist> artist_db, List<Album> album_db, List<Genre> genres_db)
        {
            Random rnd = new Random();
            List<Song> songs_db = new List<Song>();
            foreach (var song in music_data.Songs)
            {
                if (!songs_db.Any(s => s.Title == song.Track.Name))
                {
                    var artist_db_model = artist_db.FirstOrDefault(a => a.Name == song.Artists[0].Name);
                    var album_db_model = album_db.FirstOrDefault(a => a.Title == song.Artists[0].Albums[0].Name);
                    var song_url = songsUrl.FirstOrDefault(s => s.Contains(song.Track.Name)) ?? songsUrl[rnd.Next(0, songsUrl.Count)];
                    var songId = Guid.NewGuid(); 
                    var songGenresIds = genres_db.Where(g => song.Artists[0].Genres.Contains(g.Name)).Select(g => g.Id).ToList();
                    var song_db_model = new Song
                    {
                        Id = songId,
                        Title = song.Track.Name,
                        Duration = song.Track.Duration,
                        GenresIds = songGenresIds,
                        ArtistId = artist_db_model.Id,
                        AlbumId = album_db_model.Id,
                        AudioUrl = song_url,
                    };
                    songs_db.Add(song_db_model);
                }

                foreach (var artist in song.Artists)
                {
                    foreach (var album in artist.Albums)
                    {
                        foreach (var track in album.Tracks)
                        {
                            if (songs_db.Any(s => s.Title == track.Name))
                            {
                                continue;
                            }
                            var artist_db_model = artist_db.FirstOrDefault(a => a.Name == artist.Name);
                            var album_db_model = album_db.FirstOrDefault(a => a.Title == album.Name);
                            var song_url = songsUrl.FirstOrDefault(s => s.Contains(track.Name)) ?? songsUrl[rnd.Next(0, songsUrl.Count)];
                            var songId = Guid.NewGuid();
                            var songGenresIds = genres_db.Where(g => artist.Genres.Contains(g.Name)).Select(g => g.Id).ToList();
                            var song_db_model = new Song
                            {
                                Id = songId,
                                Title = track.Name,
                                Duration = track.Duration,
                                GenresIds = songGenresIds,
                                ArtistId = artist_db_model.Id,
                                AlbumId = album_db_model.Id,
                                AudioUrl = song_url
                            };
                            songs_db.Add(song_db_model);
                        }
                    }
                }
            }
            return songs_db;
        }

        private static List<Playlist> GenerateRandomPlaylists(List<Song> songs_db, List<User> users)
        {
            List<Playlist> playlists = new List<Playlist>();
            Random rnd = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < 3; i++)
                {
                    var playlist = new Playlist(
                        Guid.NewGuid(),
                        Faker.Lorem.Sentence(),
                        rnd.Next(0, 2) == 1,
                        user.Id,
                        $"https://picsum.photos/seed/{rnd.Next(0, 10000)}/200/300",
                        Faker.Lorem.Paragraph()
                    );
                    playlists.Add(playlist);
                }
            }
            return playlists;
        }

        private static List<PlaylistSong> GenerateRandomPlaylistSongs(List<Playlist> playlistsDb, List<Song> songs_db)
        {
            List<PlaylistSong> playlistSongs = new List<PlaylistSong>();
            Random rnd = new Random();

            foreach (var playlist in playlistsDb)
            {
                var numberOfSongs = rnd.Next(5, 20); // Each playlist will have between 5 and 20 songs
                for (int i = 0; i < numberOfSongs; i++)
                {
                    var song = songs_db[rnd.Next(0, songs_db.Count)];
                    if (playlistSongs.Any(ps => ps.PlaylistId == playlist.Id && ps.SongId == song.Id))
                    {
                        continue;
                    }

                    playlistSongs.Add(new PlaylistSong(playlist.Id, song.Id));
                }
            }

            return playlistSongs;
        }

        private static List<LikeAlbum> GenerateRandomLikesAlbum(List<Album> albums_db, List<User> users)
        {
            var likes_album = new List<LikeAlbum>();

            Random rnd = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < 10; i++)
                {
                    var album = albums_db[rnd.Next(0, albums_db.Count)];
                    if (likes_album.Any(l => l.UserId == user.Id && l.AlbumId == album.Id))
                    {
                        continue;
                    }
                    var like = new LikeAlbum(user.Id, album.Id);
                    likes_album.Add(like);
                }
            }
            return likes_album;
        }

        private static List<LikeSong> GenerateRandomLikesSong(List<Song> songs, List<User> users)
        {
            var likes_songs = new List<LikeSong>();

            Random rnd = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < 100; i++)
                {
                    var song = songs[rnd.Next(0, songs.Count)];
                    if (likes_songs.Any(l => l.UserId == user.Id && l.SongId == song.Id))
                    {
                        continue;
                    }
                    var like = new LikeSong(user.Id, song.Id);
                    likes_songs.Add(like);
                }
            }
            return likes_songs;
        }

        private static List<LikePlaylist> GenerateRandomLikesPlaylist(List<Playlist> playlistsDb, List<User> users)
        {
            var likes_playlist = new List<LikePlaylist>();

            Random rnd = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < 10; i++)
                {
                    var playlist = playlistsDb[rnd.Next(0, playlistsDb.Count)];
                    if (likes_playlist.Any(l => l.UserId == user.Id && l.PlaylistId == playlist.Id))
                    {
                        continue;
                    }

                    var like = new LikePlaylist(user.Id, playlist.Id);
                    likes_playlist.Add(like);
                }
            }
            return likes_playlist;
        }

        private static List<FollowArtist> GenerateRandomFollowsArtist(List<Artist> artists_db, List<User> users)
        {
            var follows_artists = new List<FollowArtist>();

            Random rnd = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < 10; i++)
                {
                    var artist = artists_db[rnd.Next(0, artists_db.Count)];
                    if (follows_artists.Any(f => f.UserId == user.Id && f.ArtistId == artist.Id))
                    {
                        continue;
                    }

                    var follow = new FollowArtist(user.Id, artist.Id);
                    follows_artists.Add(follow);
                }
            }
            return follows_artists;
        }

        private static async Task<List<User>> GenerateRandomUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<UserAccessContext>();
            var role = UserRoles.User;
            var users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                var user = new User()
                {
                    UserName = "user" + i,
                    Email = "user" + i + "@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var result = await userManager.CreateAsync(user, "Password@" + i);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Error creating user: " + user.UserName);
                }
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                await userManager.AddToRoleAsync(user, role);
                users.Add(user);
            }
            return users;
        }

        private static List<Song> RemoveDuplicateSongs(List<Song> songs)
        {
            HashSet<string> songTitles = new HashSet<string>();
            List<Song> uniqueSongs = new List<Song>();

            foreach (var song in songs)
            {
                if (!songTitles.Contains(song.Title))
                {
                    uniqueSongs.Add(song);
                    songTitles.Add(song.Title);
                }
            }

            return uniqueSongs;
        }

        private static List<Album> RemoveDuplicateAlbums(List<Album> albums)
        {
            HashSet<string> albumTitles = new HashSet<string>();
            List<Album> uniqueAlbums = new List<Album>();

            foreach (var album in albums)
            {
                if (!albumTitles.Contains(album.Title))
                {
                    uniqueAlbums.Add(album);
                    albumTitles.Add(album.Title);
                }
            }

            return uniqueAlbums;
        }

        private static List<Album> RemoveAlbumsWithoutSongs(List<Album> albums, List<Song> songs)
        {


            List<Album> albumsWithSongs = new List<Album>();

            foreach (var album in albums)
            {
                if (songs.Any(s => s.AlbumId == album.Id))
                {
                    albumsWithSongs.Add(album);
                }
            }

            return albumsWithSongs;
        }
    }
}
