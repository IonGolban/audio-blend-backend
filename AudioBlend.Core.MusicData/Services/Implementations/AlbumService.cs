using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Mappers;
using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRecommendationService _recommendationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenreRepository _genreRepository;

        public AlbumService(IGenreRepository genreRepository,ICurrentUserService currentUserService, IAlbumRepository albumRepository, UserManager<IdentityUser> userManager, IRecommendationService recommendationService)
        {
            _albumRepository = albumRepository;
            _userManager = userManager;
            _recommendationService = recommendationService;
            _currentUserService = currentUserService;
            _genreRepository = genreRepository;
        }

        public async Task<Response<List<AlbumQueryDto>>> GetAllAlbums()
        {
            var albums = await _albumRepository.GetAll();
            if (!albums.IsSuccess)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = albums.ErrorMsg
                };
            }

            var albumQuery = new List<AlbumQueryDto>();
            foreach (var album in albums.Value)
            {
                var songsDto = new List<SongQueryDto>();
                foreach (var song in album.Songs)
                {
                    var songGenres = await _genreRepository.GetBySongId(song.Id);
                    songsDto.Add(SongMapper.MapToSongQueryDto(song, album.Artist, songGenres.Value));
                }

                albumQuery.Add(AlbumMapper.MapToAlbumQueryDto(album,songsDto));
            }
            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };
        }

        public async Task<Response<AlbumQueryDto>> GetAlbumById(Guid albumId)
        {
            var album = await _albumRepository.GetByIdAsync(albumId);
            if (!album.IsSuccess)
            {
                return new Response<AlbumQueryDto>()
                {
                    Success = false,
                    Message = album.ErrorMsg
                };
            }

            var songsDto = new List<SongQueryDto>();
            foreach (var song in album.Value.Songs)
            {
                var songGenres = await _genreRepository.GetBySongId(song.Id);
                songsDto.Add(new SongQueryDto()
                {
                    Id = song.Id,
                    Title = song.Title,
                    Duration = song.Duration,
                    Genres = songGenres.Value,
                    AlbumId = song.AlbumId,
                    AlbumName = song.Album.Title,
                    ArtistId = song.ArtistId,
                    ArtistName = album.Value.Artist.Name,
                    CoverUrl = album.Value.CoverUrl
                });

            }

            return new Response<AlbumQueryDto>()
            {
                Data = AlbumMapper.MapToAlbumQueryDto(album.Value,songsDto),
                Success = true
            };
        }
        public async Task<Response<List<AlbumQueryDto>>> GetLikedUserAlbums(string userId)
        {
            if (!await CheckUserId(userId))
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var likedAlbums = await _albumRepository.GetLikedAlbumsByUserId(userId);
            if (!likedAlbums.IsSuccess)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = likedAlbums.ErrorMsg
                };
            }
            var albumsQuery = new List<AlbumQueryDto>();

            var songsDto = new List<SongQueryDto>();


            foreach (var album in likedAlbums.Value)
            {
                foreach (var song in album.Songs)
                {
                    var songGenres = await _genreRepository.GetBySongId(song.Id);
                    songsDto.Add(SongMapper.MapToSongQueryDto(song, album.Artist, songGenres.Value));
                }
                albumsQuery.Add(AlbumMapper.MapToAlbumQueryDto(album, songsDto));
            }

            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumsQuery,
                Success = true
            };
        }
        public async Task<bool> CheckUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            return true;

        }

        public async Task<Response<List<AlbumQueryDto>>> GetRandomAlbums(int count)
        {
            if (count <= 0)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = "Count must be greater than 0"
                };
            }

            var albums = _albumRepository.GetRandomAlbums(count).Result;

            if (!albums.IsSuccess)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = albums.ErrorMsg
                };
            }
            var albumQuery = new List<AlbumQueryDto>();
            foreach (var album in albums.Value)
            {
                var songsDto = new List<SongQueryDto>();
                foreach (var song in album.Songs)
                {
                    var songGenres = await _genreRepository.GetBySongId(song.Id);
                    songsDto.Add(SongMapper.MapToSongQueryDto(song, album.Artist, songGenres.Value));
                }


                albumQuery.Add(AlbumMapper.MapToAlbumQueryDto(album, songsDto));
            }

            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };

        }

        public async Task<Response<List<AlbumQueryDto>>> GetRecommendedAlbums(int count)
        {
            if (count <= 0)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = "Count must be greater than 0"
                };
            }

            var currentUser = _currentUserService.GetUserId;
            var genres = await _recommendationService.GetRecommendationGenres(currentUser, 5);
            var albums = new List<Album>();
            foreach (var genre in genres)
            {
                var responseArtist = await _albumRepository.GetRecommendedByGenreFromArtists(genre.Id, 5);
                if (responseArtist.IsSuccess)
                {
                    albums.AddRange(responseArtist.Value);
                }

                var responseSongs = await _albumRepository.GetRecommendedByGenreFromSongs(genre.Id, 5);
                if (responseSongs.IsSuccess)
                {
                    albums.AddRange(responseSongs.Value);
                }
            }

            var result = albums.Distinct().Take(count).ToList();

            var albumQuery = new List<AlbumQueryDto>();

            foreach (var album in result)
            {
                var songsDto  = new List<SongQueryDto>();   

                foreach (var song in album.Songs)
                {
                    var songGenres = await _genreRepository.GetBySongId(song.Id);
                    songsDto.Add(SongMapper.MapToSongQueryDto(song, album.Artist, songGenres.Value));
                }
                albumQuery.Add(AlbumMapper.MapToAlbumQueryDto(album,songsDto));
            }
            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };
        }

        public async Task<Response<List<AlbumQueryDto>>> GetByGenre(Guid genre, int count)
        {
            if (count <= 0)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = "Count must be greater than 0"
                };
            }

            var genreEntity = await _genreRepository.GetByIdAsync(genre);
            if(!genreEntity.IsSuccess)
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = genreEntity.ErrorMsg
                };


            var albums = new List<Album>();
            var responseArtist = await _albumRepository.GetRecommendedByGenreFromArtists(genreEntity.Value.Id, count*2);
            if (responseArtist.IsSuccess)
            {
                albums.AddRange(responseArtist.Value);
            }

            var responseSongs = await _albumRepository.GetRecommendedByGenreFromSongs(genreEntity.Value.Id, count*2);
            if (responseSongs.IsSuccess)
            {
                albums.AddRange(responseSongs.Value);
            }

            var result = albums.Distinct().Take(count).ToList();

            var albumQuery = new List<AlbumQueryDto>();

            foreach (var album in result)
            {
                var genres = await _genreRepository.GetByAlbumId(album.Id);
                var songs = new List<SongQueryDto>();
                foreach (var song in album.Songs)
                {
                    var songGenres = await _genreRepository.GetBySongId(song.Id);
                    songs.Add(SongMapper.MapToSongQueryDto(song, album.Artist, songGenres.Value));
                }
                albumQuery.Add(AlbumMapper.MapToAlbumQueryDto(album,songs));
            }
            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };
        }

        public async Task<Response<List<AlbumQueryDto>>> GetByGenres(GenresQueryDto genres, int count)
        {
            if (genres.GenresIds.Count <= 0 || count <= 0)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = "Genres list must not be empty or count must be greater than 0"
                };
            }

            var albums = new List<Album>();

            foreach (var genre in genres.GenresIds)
            {
                var genreEntity = await _genreRepository.GetByIdAsync(genre);

                if (!genreEntity.IsSuccess)
                {
                    return new Response<List<AlbumQueryDto>>()
                    {
                        Success = false,
                        Message = genreEntity.ErrorMsg
                    };
                }

                var responseArtist = await _albumRepository.GetRecommendedByGenreFromArtists(genreEntity.Value.Id, count*2);
                if (responseArtist.IsSuccess)
                {
                    albums.AddRange(responseArtist.Value);
                }

                var responseSongs = await _albumRepository.GetRecommendedByGenreFromSongs(genreEntity.Value.Id, count*2);
                if (responseSongs.IsSuccess)
                {
                    albums.AddRange(responseSongs.Value);
                }
            }

            var result = albums.Distinct().Take(count).ToList();
            
            var albumQuery = new List<AlbumQueryDto>();

            foreach (var album in result)
            {
                var songsDto = new List<SongQueryDto>();
                foreach (var song in album.Songs)
                {
                    var songGenres = await _genreRepository.GetBySongId(song.Id);
                    songsDto.Add(SongMapper.MapToSongQueryDto(song, album.Artist, songGenres.Value));
                }
                albumQuery.Add( AlbumMapper.MapToAlbumQueryDto(album, songsDto));
            }

            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };

        }

        public async Task<Response<List<AlbumQueryDto>>> GetByArtistId(Guid artistId)
        {
            var result = await _albumRepository.GetByArtistId(artistId);
            if (!result.IsSuccess)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            var albumQuery = new List<AlbumQueryDto>();
            foreach (var album in result.Value)
            {
                var songsDto = new List<SongQueryDto>();
                foreach (var song in album.Songs)
                {
                    var songGenres = await _genreRepository.GetBySongId(song.Id);
                    songsDto.Add(SongMapper.MapToSongQueryDto(song,album.Artist,songGenres.Value));
                }
                albumQuery.Add(AlbumMapper.MapToAlbumQueryDto(album, songsDto));
            }

            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };
        }
    }
}
