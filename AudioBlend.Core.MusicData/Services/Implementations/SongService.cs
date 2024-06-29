using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Mappers;
using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class SongService(IRecommendationService recommendationService, IPlaylistRepository playlistRepository,
                             ILikeAlbumRepository likeAlbumRepository, ILikePlaylistRepository likePlaylistRepository,
                             ILikeSongRepository likeSongRepository, ISongRepository songRepository,
                             IArtistRepository artistRepository, IAlbumRepository albumRepository,
                             ICurrentUserService currentUserService, UserManager<IdentityUser> userManager,
                             IGenreRepository genreRepository) : ISongService
    {
        private readonly ILikeSongRepository _likeSongRepositry = likeSongRepository;
        private readonly ILikeAlbumRepository _likeAlbumRepository = likeAlbumRepository;
        private readonly ILikePlaylistRepository _likePlaylistRepository = likePlaylistRepository;
        private readonly ISongRepository _songRepository = songRepository;
        private readonly IArtistRepository _artistRepository = artistRepository;
        private readonly IAlbumRepository _albumRepository = albumRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IPlaylistRepository _playlistRepository = playlistRepository;
        private readonly IRecommendationService recommendationService = recommendationService;
        private readonly IGenreRepository _genreRepository = genreRepository;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        public async Task<Response<List<SongQueryDto>>> GetLikedSongs(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            
            var likes = await _likeSongRepositry.GetLikedByUser(userId);

            if (!likes.IsSuccess)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = true,
                    Message = likes.ErrorMsg
                };
            }

            var songs = new List<SongQueryDto>();
            foreach (var like in likes.Value)
            {
                var song = await _songRepository.GetByIdAsync(like.SongId);
                var genres = await _genreRepository.GetBySongId(song.Value.Id);


               
                if (song.IsSuccess)
                {
                    songs.Add(SongMapper.MapToSongQueryDto(song.Value, song.Value.Artist,genres.Value));
                }
            }

            return new Response<List<SongQueryDto>>()
            {
                Success = true,
                Data = songs
            };
        }
        public async Task<Response<List<SongQueryDto>>> GetTopSongs(int count)
        {
            var resultLiked = await _likeSongRepositry.GetTopSongs(count);

            List<Song> songs = new List<Song>();

            foreach (var likeSong in resultLiked.Value)
            {
                var song = await _songRepository.GetByIdAsync(likeSong.SongId);
                if (song.IsSuccess)
                {
                    if (!songs.Any((s) => s.Id == song.Value.Id))
                    {

                        songs.Add(song.Value);
                    }
                }
            }

            List<SongQueryDto> songsQuery = new List<SongQueryDto>();

            foreach (var song in songs)
            {
                var aritst = await _artistRepository.GetByIdAsync(song.ArtistId);
                var album = await _albumRepository.GetByIdAsync(song.AlbumId);
                var genres = await _genreRepository.GetBySongId(song.Id);
                Console.WriteLine("Genres: " + genres.Value);
                if (!aritst.IsSuccess || !album.IsSuccess || !genres.IsSuccess)
                {
                    continue;
                }

                songsQuery.Add(
                    SongMapper.MapToSongQueryDto(song, aritst.Value, genres.Value));
            }
            return new Response<List<SongQueryDto>>()
            {
                Data = songsQuery,
                Success = true
            };
        }


        public async Task<Response<List<SongQueryDto>>> GetByArtistId(Guid id)
        {
            var result = await _songRepository.GetByArtistId(id);
            if (!result.IsSuccess)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            var songs = result.Value;
            var listSongsDto = new List<SongQueryDto>();
            foreach (var song in songs)
            {
                var genres = await _genreRepository.GetBySongId(song.Id);
                if (!genres.IsSuccess)
                {
                    Console.WriteLine("Genres not found");
                }
                listSongsDto.Add(SongMapper.MapToSongQueryDto(song, song.Artist, genres.Value));
            }
            return new Response<List<SongQueryDto>>()
            {
                Data = listSongsDto,
                Success = true
            };
        }

        public async Task<Response<List<SongQueryDto>>> GetRandomSongs(int count)
        {
            var result = await _songRepository.GetRandom(count);
            if (!result.IsSuccess)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            var listSongsDto = new List<SongQueryDto>();

            foreach (var song in result.Value)
            {
                var artist = await _artistRepository.GetByIdAsync(song.ArtistId);
                var album = await _albumRepository.GetByIdAsync(song.AlbumId);
                var genres = await _genreRepository.GetBySongId(song.Id);
                if (!artist.IsSuccess || !album.IsSuccess)
                {
                    continue;
                }

                listSongsDto.Add(SongMapper.MapToSongQueryDto(song, song.Artist,genres.Value));
            }

            return new Response<List<SongQueryDto>>()
            {
                Data = listSongsDto,
                Success = true
            };


        }

        public async Task<Response<List<SongQueryDto>>> GetRecommendations(int count)
        {
            var userId = _currentUserService.GetUserId;

            var topGenres = await recommendationService.GetRecommendationGenres(userId, 3);

            var recommendedSongs = new List<Song>();
            foreach (var genre in topGenres)
            {
                var result = await _songRepository.GetByGenre(genre.Id, 10);
                if (result.IsSuccess)
                {
                    recommendedSongs.AddRange(result.Value);
                }
            }



            var recommendations = new List<SongQueryDto>();
            foreach (var song in recommendedSongs)
            {
                var artist = await _artistRepository.GetByIdAsync(song.ArtistId);
                var album = await _albumRepository.GetByIdAsync(song.AlbumId);
                if (artist.IsSuccess && album.IsSuccess)
                {
                    var genres = await _genreRepository.GetByMultipleIds(song.GenresIds);
                    if (!genres.IsSuccess)
                    {
                        Console.WriteLine("Genres not found");

                    }
                    recommendations.Add(new SongQueryDto()
                    {
                        Id = song.Id,
                        Title = song.Title,
                        ArtistId = song.ArtistId,
                        ArtistName = artist.Value.Name,
                        AlbumId = song.AlbumId,
                        AlbumName = album.Value.Title,
                        Duration = song.Duration,
                        Genres = genres.Value,
                        CoverUrl = album.Value.CoverUrl,
                        AudioUrl = song.AudioUrl
                    });
                }
            }
            recommendations = recommendations.Distinct().ToList();
            recommendations = recommendations.OrderBy(x => Guid.NewGuid()).Take(count).ToList();

            return new Response<List<SongQueryDto>>()
            {
                Data = recommendations,
                Success = true
            };
        }

        public async Task<Response<List<SongQueryDto>>> GetByGenre(Guid genre, int count)
        {

            var genreEntity = await _genreRepository.GetByIdAsync(genre);

            if (!genreEntity.IsSuccess)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = genreEntity.ErrorMsg
                };
            }

            var result = await _songRepository.GetByGenre(genreEntity.Value.Id, count);
            if (!result.IsSuccess)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            var listSongsDto = new List<SongQueryDto>();

            foreach (var song in result.Value)
            {
                var artist = await _artistRepository.GetByIdAsync(song.ArtistId);
                var album = await _albumRepository.GetByIdAsync(song.AlbumId);
                var genres = await _genreRepository.GetBySongId(song.Id);
                if (!artist.IsSuccess || !album.IsSuccess)
                {
                    continue;
                }

                listSongsDto.Add(SongMapper.MapToSongQueryDto(song, song.Artist, genres.Value));
            }

            return new Response<List<SongQueryDto>>()
            {
                Success = true,
                Data = listSongsDto
            };
        }

        public async Task<Response<List<SongQueryDto>>> GetByGenres(GenresQueryDto genresQuery, int count)
        {
            var songs = new List<Song>();

            if(genresQuery.GenresIds.Count <= 0 || genresQuery.GenresIds.Count == 0)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = "No genres found"
                };
            }

            foreach (var genre in genresQuery.GenresIds)
            {
                var genreEntity = await _genreRepository.GetByIdAsync(genre);

                if (!genreEntity.IsSuccess)
                {
                    continue;
                }

                var result = await _songRepository.GetByGenre(genreEntity.Value.Id, count);

                if (result.IsSuccess)
                {
                    songs.AddRange(result.Value);
                }
            }

            if (songs.Count == 0)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = "No songs found"
                };
            }

            var listSongsDto = new List<SongQueryDto>();

            foreach (var song in songs)
            {
                var artist = await _artistRepository.GetByIdAsync(song.ArtistId);
                var album = await _albumRepository.GetByIdAsync(song.AlbumId);
                var genresSong = await _genreRepository.GetBySongId(song.Id);
                if (!artist.IsSuccess || !album.IsSuccess)
                {
                    continue;
                }

                listSongsDto.Add(SongMapper.MapToSongQueryDto(song, song.Artist, genresSong.Value));
            }

            return new Response<List<SongQueryDto>>()
            {
                Success = true,
                Data = listSongsDto
            };
        }


    }
}
