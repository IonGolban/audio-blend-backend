using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class AlbumCommandService(IGenreRepository genreRepository, IAzureBlobStorageService azureBlobStorageService, IArtistService artistService, IAlbumRepository albumRepository, ICurrentUserService currentUserService, IArtistRepository artistRepository) : IAlbumCommandService
    {
        private readonly IAlbumRepository _albumRepository = albumRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IArtistRepository _artistRepository = artistRepository;
        private readonly IArtistService _artistService = artistService;
        private readonly IAzureBlobStorageService _fileService = azureBlobStorageService;
        private readonly IGenreRepository _genreRepository = genreRepository;


        public async Task<Response<Album>> AddAlbum(AddAlbumDto albumDto)
        {
            Console.WriteLine(albumDto.Songs.Count);
            Console.WriteLine(albumDto.Songs[0].FileName);
            
            
            if (!ValidateAddAlbumDto(albumDto))
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Invalid data"
                };
            }
            var userId = _currentUserService.GetUserId;
            var artist = await GetArtistByUserId(userId);
            if (artist == null)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "User cannot be artist ???"
                };
            }
            if(albumDto.CoverImage.ContentType != "image/jpeg" && albumDto.CoverImage.ContentType != "image/png" && albumDto.CoverImage.ContentType != "image/jpg")
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Cover image must be in jpeg or png format"
                };
            }

            var uploadedCoverUrl = await _fileService.UploadFileToBlobAsync(albumDto.CoverImage);



            if (!uploadedCoverUrl.IsSuccess)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Error while uploading cover image"
                };
            }

            var songs = new List<Song>();
            var album = new Album(new Guid(), albumDto.Title, albumDto.Description, "album", uploadedCoverUrl.Value, artist.Id, DateTime.UtcNow);

            var genres = new List<Guid>();
            foreach (var genreName in albumDto.Genres)
            {
                Guid genreId;
                var genreEntity = await _genreRepository.GetByName(genreName);
                if (!genreEntity.IsSuccess)
                {
                    var newGenre = new Genre(Guid.NewGuid(), genreName);
                    var resultGenre = await _genreRepository.AddAsync(newGenre);
                    if (!resultGenre.IsSuccess)
                    {
                        return new Response<Album>()
                        {
                            Success = false,
                            Message = "Error while adding genre"
                        };
                    }
                    genreId = newGenre.Id;
                }
                else
                {
                    genreId = genreEntity.Value.Id;
                }
                genres.Add(genreId);
            }


            foreach (var songAlbum in albumDto.Songs)
            {

                if (songAlbum.ContentType != "audio/mp3" && songAlbum.ContentType != "audio/wav" && songAlbum.ContentType != "audio/wave")
                {
                    return new Response<Album>()
                    {
                        Success = false,
                        Message = "Song must be in mp3 format"
                    };
                }

                var uploadedSongUrl = await _fileService.UploadFileToBlobAsync(songAlbum);
                if (!uploadedSongUrl.IsSuccess)
                {
                    return new Response<Album>()
                    {
                        Success = false,
                        Message = "Error while uploading song"
                    };
                }
                songs.Add(new Song
                {
                    Id = Guid.NewGuid(),
                    Title = songAlbum.FileName,
                    AudioUrl = uploadedSongUrl.Value,
                    ArtistId = artist.Id,
                    Artist = artist,
                    AlbumId = album.Id,
                    GenresIds = genres
                });

            }
        
            album.AddSongRange(songs);



            var result = await _albumRepository.AddAsync(album);
            if (!result.IsSuccess)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Error while adding album"
                };
            }
            return new Response<Album>()
            {
                Data = result.Value,
                Success = true
            };
        }

        public async Task<Response<Album>> AddSingleRelease(AddSingleReleaseDto singleReleaseDto)
        {
            Console.WriteLine(singleReleaseDto.Title);
            Console.WriteLine(singleReleaseDto.Song.ContentType);
            if(string.IsNullOrEmpty(singleReleaseDto.Title) || singleReleaseDto.Genres.Count == 0 || singleReleaseDto.Song == null || singleReleaseDto.CoverImage == null)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Title cannot be empty"
                };
            }

            var userId = _currentUserService.GetUserId;
            var artist = await GetArtistByUserId(userId);

            if (artist == null)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "User cannot be artist ???"
                };
            }

            if (singleReleaseDto.Song.ContentType != "audio/mpeg" && singleReleaseDto.Song.ContentType != "audio/mp3" && singleReleaseDto.Song.ContentType != "audio/wav" && singleReleaseDto.Song.ContentType != "audio/wave")
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Song must be in mp3 format"
                };
            }

            if (singleReleaseDto.CoverImage.ContentType != "image/jpeg" && singleReleaseDto.CoverImage.ContentType != "image/png" && singleReleaseDto.CoverImage.ContentType != "image/jpg")
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Cover image must be in jpeg or png format"
                };
            }

            var uploadedCoverUrl = await _fileService.UploadFileToBlobAsync(singleReleaseDto.CoverImage);

            if (!uploadedCoverUrl.IsSuccess)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Error while uploading cover image"
                };
            }

            var uploadedSongUrl = await _fileService.UploadFileToBlobAsync(singleReleaseDto.Song);

            if (!uploadedSongUrl.IsSuccess)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Error while uploading song"
                };
            }

            var genres = new List<Guid>();

            foreach (var genreName in singleReleaseDto.Genres)
            {
                Guid genreId;
                var genreEntity = await _genreRepository.GetByName(genreName);
                if (!genreEntity.IsSuccess)
                {
                    var newGenre = new Genre(Guid.NewGuid(), genreName);
                    var resultGenre = await _genreRepository.AddAsync(newGenre);
                    if (!resultGenre.IsSuccess)
                    {
                        return new Response<Album>()
                        {
                            Success = false,
                            Message = "Error while adding genre"
                        };
                    }
                    genreId = newGenre.Id;
                }
                else
                {
                    genreId = genreEntity.Value.Id;
                }
                genres.Add(genreId);
            }

            var song = new Song
            {
                Id = Guid.NewGuid(),
                Title = singleReleaseDto.Title,
                AudioUrl = uploadedSongUrl.Value,
                ArtistId = artist.Id,
                Artist = artist,
                GenresIds = genres
            };

            var album = new Album(Guid.NewGuid(), singleReleaseDto.Title, singleReleaseDto.Description, "single", uploadedCoverUrl.Value, artist.Id, DateTime.UtcNow);

            album.AddSong(song);

            var result = await _albumRepository.AddAsync(album);

            if (!result.IsSuccess)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Error while adding album"
                };
            }

            return new Response<Album>()
            {
                Data = result.Value,
                Success = true
            };
        }

        public async Task<Response<Album>> DeleteAlbum(Guid albumId)
        {
            var album = await _albumRepository.GetByIdAsync(albumId);
            if (album == null)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Album not found"
                };
            }

            var result = await _albumRepository.DeleteAsync(albumId);
            
            if (!result.IsSuccess)
            {
                return new Response<Album>()
                {
                    Success = false,
                    Message = "Error while deleting album"
                };
            }

            return new Response<Album>()
            {
                Data = album.Value,
                Success = true
            };
        }

        public async Task<Artist> GetArtistByUserId(string userId)
        {
            var artist = await _artistRepository.getByUserId(userId);
            if (!artist.IsSuccess)
            {
                var newArtist = await _artistService.CreateArtistByUserId(userId);
                return newArtist.Data;
            }
            return artist.Value;
        }

        public bool ValidateAddAlbumDto(AddAlbumDto addAlbumDto)
        {
            if(addAlbumDto == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(addAlbumDto.Title))
            {
                return false;
            }
            if (string.IsNullOrEmpty(addAlbumDto.Description))
            {
                return false;
            }

            foreach (var songAlbum in addAlbumDto.Songs)
            {
                if (songAlbum == null)
                { 
                   return false;
                }
            }
            foreach(var genre in addAlbumDto.Genres)
            {
                if (string.IsNullOrEmpty(genre))
                {
                    return false;
                }
            }
            return true;


        }
    }
}
