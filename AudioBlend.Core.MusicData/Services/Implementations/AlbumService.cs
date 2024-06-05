using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Mappers;
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

        public AlbumService(ICurrentUserService currentUserService, IAlbumRepository albumRepository, UserManager<IdentityUser> userManager, IRecommendationService recommendationService)
        {
            _albumRepository = albumRepository;
            _userManager = userManager;
            _recommendationService = recommendationService;
            _currentUserService = currentUserService;
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
                albumQuery.Add(AlbumMapper.MapToAlbumQueryDto(album));

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

            return new Response<AlbumQueryDto>()
            {
                Data = AlbumMapper.MapToAlbumQueryDto(album.Value),
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
            foreach (var album in likedAlbums.Value)
            {
                albumsQuery.Add(AlbumMapper.MapToAlbumQueryDto(album));
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
                albumQuery.Add(AlbumMapper.MapToAlbumQueryDto(album));
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
                var responseArtist = await _albumRepository.GetRecommendedByGenreFromArtists(genre, 5);
                if (responseArtist.IsSuccess)
                {
                    albums.AddRange(responseArtist.Value);
                }

                var responseSongs = await _albumRepository.GetRecommendedByGenreFromSongs(genre, 5);
                if (responseSongs.IsSuccess)
                {
                    albums.AddRange(responseSongs.Value);
                }
            }

            var result = albums.Distinct().Take(count).ToList();

            var albumQuery = new List<AlbumQueryDto>();

            foreach (var album in result)
            {
                albumQuery.Add(new AlbumQueryDto()
                {
                    Id = album.Id,
                    Title = album.Title,
                    ArtistId = album.Artist.Id,
                    ArtistName = album.Artist.Name,

                    CoverUrl = album.CoverUrl,
                    ReleaseDate = album.ReleaseDate,
                    Type = album.Type,
                    Songs = album.Songs.Select(s => new SongQueryDto()
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Duration = s.Duration,
                        Genres = s.Genres,
                        AlbumId = s.AlbumId,
                        AlbumName = s.Album.Title,
                        ArtistId = s.ArtistId,
                        ArtistName = album.Artist.Name,
                        CoverUrl = album.CoverUrl
                    }).ToList()

                });
            }
            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };
        }

        public async Task<Response<List<AlbumQueryDto>>> GetByGenre(string genre, int count)
        {
            if (count <= 0)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = "Count must be greater than 0"
                };
            }

            var albums = new List<Album>();
            var responseArtist = await _albumRepository.GetRecommendedByGenreFromArtists(genre, count*2);
            if (responseArtist.IsSuccess)
            {
                albums.AddRange(responseArtist.Value);
            }

            var responseSongs = await _albumRepository.GetRecommendedByGenreFromSongs(genre, count*2);
            if (responseSongs.IsSuccess)
            {
                albums.AddRange(responseSongs.Value);
            }

            var result = albums.Distinct().Take(count).ToList();

            var albumQuery = new List<AlbumQueryDto>();

            foreach (var album in result)
            {
                albumQuery.Add(new AlbumQueryDto()
                {
                    Id = album.Id,
                    Title = album.Title,
                    ArtistId = album.Artist.Id,
                    ArtistName = album.Artist.Name,

                    CoverUrl = album.CoverUrl,
                    ReleaseDate = album.ReleaseDate,
                    Type = album.Type,
                    Songs = album.Songs.Select(s => new SongQueryDto()
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Duration = s.Duration,
                        Genres = s.Genres,
                        AlbumId = s.AlbumId,
                        AlbumName = s.Album.Title,
                        ArtistId = s.ArtistId,
                        ArtistName = album.Artist.Name,
                        CoverUrl = album.CoverUrl
                    }).ToList()

                });
            }
            return new Response<List<AlbumQueryDto>>()
            {
                Data = albumQuery,
                Success = true
            };
        }

        public async Task<Response<List<AlbumQueryDto>>> GetByGenres(List<string> genres, int count)
        {
            if (genres.Count <= 0 || count <= 0)
            {
                return new Response<List<AlbumQueryDto>>()
                {
                    Success = false,
                    Message = "Genres list must not be empty or count must be greater than 0"
                };
            }

            var albums = new List<Album>();

            foreach (var genre in genres)
            {
                var responseArtist = await _albumRepository.GetRecommendedByGenreFromArtists(genre, count*2);
                if (responseArtist.IsSuccess)
                {
                    albums.AddRange(responseArtist.Value);
                }

                var responseSongs = await _albumRepository.GetRecommendedByGenreFromSongs(genre, count*2);
                if (responseSongs.IsSuccess)
                {
                    albums.AddRange(responseSongs.Value);
                }
            }

            var result = albums.Distinct().Take(count).ToList();

            return new Response<List<AlbumQueryDto>>()
            {
                Data = result.Select(AlbumMapper.MapToAlbumQueryDto).ToList(),
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
            return new Response<List<AlbumQueryDto>>()
            {
                Data = result.Value.Select(AlbumMapper.MapToAlbumQueryDto).ToList(),
                Success = true
            };
        }
    }
}
