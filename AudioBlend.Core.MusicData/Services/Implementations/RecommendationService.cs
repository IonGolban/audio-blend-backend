using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ILikeSongRepository _likeSongRepositry;
        private readonly ILikeAlbumRepository _likeAlbumRepository;
        private readonly ILikePlaylistRepository _likePlaylistRepository;
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly ICurrentUserService _currentUserService ;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IGenreRepository _genreRepository;
        private const int SONG_POINTS = 30;
        private const int ALBUM_SONG_POINTS = 5;
        private const int PLAYLIST_POINTS = 10;
        private const int OWN_PLAYLIST_POINTS = 20;

        public RecommendationService(IGenreRepository genreRepository ,ILikeSongRepository likeSongRepositry, ILikeAlbumRepository likeAlbumRepository, ILikePlaylistRepository likePlaylistRepository, ISongRepository songRepository, IArtistRepository artistRepository, IAlbumRepository albumRepository, ICurrentUserService currentUserService, IPlaylistRepository playlistRepository)
        {
            _likeSongRepositry = likeSongRepositry;
            _likeAlbumRepository = likeAlbumRepository;
            _likePlaylistRepository = likePlaylistRepository;
            _songRepository = songRepository;
            _artistRepository = artistRepository;
            _albumRepository = albumRepository;
            _currentUserService = currentUserService;
            _playlistRepository = playlistRepository;
            _genreRepository = genreRepository;
        }

        public async Task<List<Genre>> GetRecommendationGenres(string userId, int count)
        {
            var likes = await _likeSongRepositry.GetLikedByUser(userId);
            var likedSongs = new List<Song>();

            // Collect liked songs
            foreach (var like in likes.Value)
            {
                var song = await _songRepository.GetByIdAsync(like.SongId);
                if (song.IsSuccess)
                {
                    likedSongs.Add(song.Value);
                }
            }

            if (likedSongs.Count == 0)
            {
                likedSongs = (await _songRepository.GetRandom(10)).Value;
            }

            var genresCount = new Dictionary<Guid, int>();

            // Count genres from liked songs
            foreach (var song in likedSongs)
            {
                foreach (var genre in song.GenresIds)
                {
                    if (genresCount.ContainsKey(genre))
                    {
                        genresCount[genre] += SONG_POINTS;
                    }
                    else
                    {
                        genresCount[genre] = SONG_POINTS;
                    }
                }
            }

            // Collect liked albums and count genres
            var albums = await _likeAlbumRepository.GetByUserId(userId);
            var likedAlbums = new List<Album>();
            foreach (var like in albums.Value)
            {
                var album = await _albumRepository.GetByIdAsync(like.AlbumId);
                if (album.IsSuccess)
                {
                    likedAlbums.Add(album.Value);
                    var albumSongs = await _songRepository.GetByAlbumId(album.Value.Id);
                    foreach (var albumSong in albumSongs.Value)
                    {
                        foreach (var genre in albumSong.GenresIds)
                        {
                            if (genresCount.ContainsKey(genre))
                            {
                                genresCount[genre] += ALBUM_SONG_POINTS;
                            }
                            else
                            {
                                genresCount[genre] = ALBUM_SONG_POINTS;
                            }
                        }
                    }
                }
            }

            // Collect liked playlists and count genres
            var playlistsLike = await _likePlaylistRepository.GetByUserId(userId);
            foreach (var like in playlistsLike.Value)
            {
                var playlist = await _playlistRepository.GetByIdAsync(like.PlaylistId);
                if (playlist.IsSuccess)
                {
                    foreach (var song in playlist.Value.PlaylistSongs.Select(ps => ps.Song))
                    {
                        foreach (var genre in song.GenresIds)
                        {
                            if (genresCount.ContainsKey(genre))
                            {
                                genresCount[genre] += PLAYLIST_POINTS;
                            }
                            else
                            {
                                genresCount[genre] = PLAYLIST_POINTS;
                            }
                        }
                    }
                }
            }

            var ownedPlaylists = await _playlistRepository.GetPlaylistsByUserId(userId);
            foreach (var playlist in ownedPlaylists.Value)
            {
                foreach (var song in playlist.PlaylistSongs.Select(ps => ps.Song))
                {
                    foreach (var genre in song.GenresIds)
                    {
                        if (genresCount.ContainsKey(genre))
                        {
                            genresCount[genre] += OWN_PLAYLIST_POINTS;
                        }
                        else
                        {
                            genresCount[genre] = OWN_PLAYLIST_POINTS;
                        }
                    }
                }
            }



            var topGenres = genresCount.OrderByDescending(g => g.Value).Select(g => g.Key).Take(count).ToList();

            var genres = new List<Genre>();
            foreach (var genreId in topGenres)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);
                if (genre.IsSuccess)
                {
                    genres.Add(genre.Value);
                }
            }
            return genres;
               
        }
    }
}
