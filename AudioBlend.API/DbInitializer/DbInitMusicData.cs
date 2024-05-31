using AudioBlend.API.DbInitializer.Models;
using AudioBlend.Core.MusicData;
using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Playlists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Models.Playlists;
using AudioBlend.Core.UserAccess;
using AudioBlend.Core.UserAccess.Models.Roles;
using AudioBlend.Core.UserAccess.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AudioBlend.API.DbInitializer
{
    public static class DbInitMusicData
    {
        private const string PATH_DATA = "C:\\\\Users\\\\uig26544\\\\Desktop\\\\Copyright-Free-Music\\\\info\\\\songs_info1.json";
        private const string PATH_IMAGES = "C:\\\\Users\\\\uig26544\\\\Desktop\\\\Copyright-Free-Music";

        private static List<Artist> usedArtists = new List<Artist>();
        private static List<String> songsUrl = new List<String>();
        public async static Task InitData(IServiceProvider serviceProvider)
        {
            songsUrl = SongBlobUrls.GetUrls();
            var users = await generateRandomUsers(serviceProvider);

            var music_data = MusicDataReader<RootJsonModel>.ReadJsonFile(PATH_DATA);
            var artists_db = ConvertToArtistDbMdodel(music_data, serviceProvider);
            var albums_db = ConvertToAlbumDbModels(music_data, serviceProvider, artists_db);
            albums_db = RemoveDuplicateAlbums(albums_db);
            var songs_db = ConvertToSongDbModels(music_data, serviceProvider, artists_db, albums_db);
            songs_db = RemoveDuplicateSongs(songs_db);
            albums_db = RemoveAlbumsWithoutSongs(albums_db, songs_db);

            var playlistsDb = GenerateRandomPlaylists(songs_db, users);
            var playlistSongsDb = GenerateRandomPlaylistSongs(playlistsDb, songs_db);

            var likes_album = GenerateRandomLikesAlbum(albums_db, users);
            var likes_songs = GenerateRandomLikesSong(songs_db, users);
            var likes_playlist = GenerateRandomLikesPlaylist(playlistsDb, users);

            await SaveModelsDb(serviceProvider, artists_db, albums_db, songs_db, playlistsDb, likes_album, likes_playlist, likes_songs, playlistSongsDb);
        }

        private static List<PlaylistSong> GenerateRandomPlaylistSongs(List<Playlist> playlistsDb, List<Song> songs_db)
        {
            List<PlaylistSong> playlistSongs = new List<PlaylistSong>();
            Random rnd = new Random();
            List<Playlist> tempPLaylist = new List<Playlist>();
            
            for (int i = 0; i < 2000; i++)
            {
                
                var playlist = playlistsDb[rnd.Next(0, playlistsDb.Count)];
                var song = songs_db[rnd.Next(0, songs_db.Count)];

                if (playlistSongs.Any(ps => ps.PlaylistId == playlist.Id && ps.SongId == song.Id))
                {
                    continue;
                }

                playlistSongs.Add(new PlaylistSong(playlist.Id, song.Id));
                tempPLaylist.Add(playlist);
            }

            return playlistSongs;
        }

        private static async Task SaveModelsDb(IServiceProvider serviceProvider, List<Artist> artists_db, List<Album> albums_db, List<Song> songs_db, List<Playlist> playlistsDb, List<LikeAlbum> likes_album, List<LikePlaylist> likes_playlist, List<LikeSong> likes_songs, List<PlaylistSong> playlistSongsDb)
        {
            var context = serviceProvider.GetRequiredService<AudioBlendContext>();

            await context.Artists.AddRangeAsync(artists_db);
            await context.Albums.AddRangeAsync(albums_db);
            await context.Songs.AddRangeAsync(songs_db);
            await context.Playlists.AddRangeAsync(playlistsDb);
            await context.LikeAlbums.AddRangeAsync(likes_album);
            await context.LikePlaylists.AddRangeAsync(likes_playlist);
            await context.LikeSongs.AddRangeAsync(likes_songs);
            await context.PlaylistSongs.AddRangeAsync(playlistSongsDb);
            await context.SaveChangesAsync();
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

        private static async Task<List<User>> generateRandomUsers(IServiceProvider serviceProvider)
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
                var result = await userManager.CreateAsync(user, "Password@"+i);
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

        private static List<Playlist> GenerateRandomPlaylists(List<Song> songs_db, List<User> users)
        {
            List<Playlist> playlists = new List<Playlist>();
            Random rnd = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < 3; i++)
                {
                    var playlist = new Playlist(Guid.NewGuid(), Faker.Lorem.Sentence(), true, user.Id);
                    playlists.Add(playlist);
                }
            }
            return playlists;
        }

        private static List<Song> ConvertToSongDbModels(RootJsonModel music_data, IServiceProvider serviceProvider, List<Artist> artist_db, List<Album> album_db)
        {
            var context = serviceProvider.GetRequiredService<AudioBlendContext>();
            Random rnd = new Random();
            List<Song> songs_db = new List<Song>();
            foreach (var song in music_data.Songs)
            {
                if (!songs_db.Any(s => s.Title == song.Track.Name))
                {
                    var artist_db_model = artist_db.FirstOrDefault(a => a.Name == song.Artists[0].Name);
                    var album_db_model = album_db.FirstOrDefault(a => a.Title == song.Artists[0].Albums[0].Name);
                    var song_url = songsUrl.FirstOrDefault(s => s.Contains(song.Track.Name));
                    if (song_url == null)
                    {
                        song_url = songsUrl[rnd.Next(0, songsUrl.Count)];
                    }
                    var song_db_model = new Song()
                    {
                        Id = Guid.NewGuid(),
                        Title = song.Track.Name,
                        Duration = song.Track.Duration,
                        Genres = artist_db_model.Genres,
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
                            var song_url = songsUrl.FirstOrDefault(s => s.Contains(song.Track.Name));
                            if (song_url == null)
                            {
                                song_url = songsUrl[rnd.Next(0, songsUrl.Count)];
                            }
                            var song_db_model = new Song()
                            {
                                Id = Guid.NewGuid(),
                                Title = track.Name,
                                Duration = track.Duration,
                                Genres = artist_db_model.Genres,
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

        public static List<Artist> ConvertToArtistDbMdodel(RootJsonModel rootModel, IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AudioBlendContext>();

            List<Artist> artistDbModels = new List<Artist>();

            foreach (var song in rootModel.Songs)
            {
                foreach (var artist in song.Artists)
                {
                    if (artistDbModels.Any(a => a.Name == artist.Name))
                    {
                        continue;
                    }
                    var artistDbModel = new Artist(Guid.NewGuid(), artist.Name, artist.Img, artist.Genres, artist.Followers);
                    artistDbModels.Add(artistDbModel);
                }
            }

            // Add artists from track artists if they are not already in the db list

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
                                    artistDbModels.Add(new Artist(Guid.NewGuid(), track_artist, "", new List<string>()));
                                }
                            }
                        }
                    }
                }
            }
            return artistDbModels;
        }

        public static List<Album> ConvertToAlbumDbModels(RootJsonModel rootModel, IServiceProvider serviceProvider, List<Artist> artists)
        {
            var context = serviceProvider.GetRequiredService<AudioBlendContext>();

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
                        DateTime releaseDate = new DateTime();

                        if (string.IsNullOrEmpty(album.ReleaseDate))
                        {
                            releaseDate = DateTime.MinValue;
                        }
                        else
                        {
                            releaseDate = DateTime.Parse(album.ReleaseDate);
                        }
                        var artistDb = artists.FirstOrDefault(a => a.Name == artist.Name);
                        var albumDbModel = new Album(Guid.NewGuid(), album.Name, string.Empty, album.Type, album.img_url, artistDb.Id, releaseDate);
                        albumDbModels.Add(albumDbModel);
                    }
                }
            }

            return albumDbModels;
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
