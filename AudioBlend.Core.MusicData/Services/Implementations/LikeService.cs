using AudioBlend.Core.MusicData.Models.Likes;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly ILikeAlbumRepository _likeAlbumRepository;
        private readonly ILikePlaylistRepository _likePlaylistRepository;
        private readonly ILikeSongRepository _likeSongRepository;

        private readonly IAlbumRepository _albumRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public LikeService(IAlbumRepository albumRepository, IPlaylistRepository playlistRepository, ISongRepository songRepository, ILikeAlbumRepository likeAlbumRepository, ILikePlaylistRepository likePlaylistRepository, ILikeSongRepository likeSongRepository, UserManager<IdentityUser> userManager)
        {
            _albumRepository = albumRepository;
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
            _likeAlbumRepository = likeAlbumRepository;
            _likePlaylistRepository = likePlaylistRepository;
            _likeSongRepository = likeSongRepository;
            _userManager = userManager;
        }

        public Task<Response<string>> GetCountLikesAlbum(Guid albumId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> GetCountLikesPlaylist(Guid playlistId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> GetCountLikesSong(Guid songId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<LikeAlbum>> LikeAlbum(string userId, Guid albumId)
        {
            var validatePostInputs = await ValidatePostInputs(userId, albumId);
            if (validatePostInputs != null)
            {
                return new Response<LikeAlbum>
                {
                    Success = false,
                    Message = validatePostInputs
                };
            }

            var album = await _albumRepository.GetByIdAsync(albumId);

            if (!album.IsSuccess)
            {
                return new Response<LikeAlbum>
                {
                    Success = false,
                    Message = album.ErrorMsg
                };

            }

            var likeIfExist = await _likeAlbumRepository.GetLikeAlbum(userId, albumId);
            if (likeIfExist.IsSuccess)
            {
                return new Response<LikeAlbum>
                {
                    Success = false,
                    Message = "Album already liked"
                };
            }

            var likeAlbum = new LikeAlbum(userId, albumId);
            var result = await _likeAlbumRepository.AddAsync(likeAlbum);

            if (!result.IsSuccess)
            {
                return new Response<LikeAlbum>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<LikeAlbum>() {
                Data = likeAlbum,
                Success = true
            };
        }

        public async Task<Response<LikePlaylist>> LikePlaylist(string userId, Guid playlistId)
        {
            var validatePostInputs = await ValidatePostInputs(userId, playlistId);
            if (validatePostInputs != null)
            {
                return new Response<LikePlaylist>
                {
                    Success = false,
                    Message = validatePostInputs
                };
            }

            var playlist = await _playlistRepository.GetByIdAsync(playlistId);
            if(!playlist.IsSuccess)
            {
                return new Response<LikePlaylist>
                {
                    Success = false,
                    Message = playlist.ErrorMsg
                };
            }

            var likeIfExist = await _likePlaylistRepository.GetLikePlaylist(userId, playlistId);
            if (likeIfExist.IsSuccess)
            {
                return new Response<LikePlaylist>
                {
                    Success = false,
                    Message = "Playlist already liked"
                };
            }

            var likePlaylist = new LikePlaylist(userId, playlistId);

            var result = await _likePlaylistRepository.AddAsync(likePlaylist);

            if (!result.IsSuccess)
            {
                return new Response<LikePlaylist>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<LikePlaylist>()
            {
                Data = likePlaylist,
                Success = true
            };
        }

        public async Task<Response<LikeSong>> LikeSong(string userId, Guid songId)
        {
            var validatePostInputs = await ValidatePostInputs(userId, songId);
            if (validatePostInputs != null)
            {
                return new Response<LikeSong>
                {
                    Success = false,
                    Message = validatePostInputs
                };
            }

            var song = await _songRepository.GetByIdAsync(songId);
            if (!song.IsSuccess)
            {
                return new Response<LikeSong>
                {
                    Success = false,
                    Message = song.ErrorMsg
                };
            }

            var likeIfExist = await _likeSongRepository.GetLikeSong(userId, songId);
            if (likeIfExist.IsSuccess)
            {
                return new Response<LikeSong>
                {
                    Success = false,
                    Message = "Song already liked"
                };
            }

            var likeSong = new LikeSong(userId, songId);

            var result = await _likeSongRepository.AddAsync(likeSong);

            if (!result.IsSuccess)
            {
                return new Response<LikeSong>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<LikeSong>()
            {
                Data = likeSong,
                Success = true
            };
        }
        
        public async Task<Response<LikeAlbum>> UnlikeAlbum(string userId, Guid albumId)
        {
            var validatePostInputs = await ValidatePostInputs(userId, albumId);
            if (validatePostInputs != null)
            {
                return new Response<LikeAlbum>
                {
                    Success = false,
                    Message = validatePostInputs
                };
            }

            var result = await _likeAlbumRepository.GetLikeAlbum(userId, albumId);
            if (!result.IsSuccess)
            {
                return new Response<LikeAlbum>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            var deleteResult = await _likeAlbumRepository.DeleteLike(result.Value.UserId, result.Value.AlbumId);

            if (!result.IsSuccess)
            {
                return new Response<LikeAlbum>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<LikeAlbum>()
            {
                Data = result.Value,
                Success = true
            };
        }

        public async Task<Response<LikePlaylist>> UnlikePlaylist(string userId, Guid playlistId)
        {
            var validatePostInputs = await ValidatePostInputs(userId, playlistId);
            if (validatePostInputs != null)
            {
                return new Response<LikePlaylist>
                {
                    Success = false,
                    Message = validatePostInputs
                };
            }

            var result = await _likePlaylistRepository.GetLikePlaylist(userId, playlistId);
            if (!result.IsSuccess)
            {
                return new Response<LikePlaylist>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            var deleteResult = await _likePlaylistRepository.DeleteLike(result.Value.UserId, result.Value.PlaylistId);

            if (!result.IsSuccess)
            {
                return new Response<LikePlaylist>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<LikePlaylist>()
            {
                Data = result.Value,
                Success = true
            };
        }

        public async Task<Response<LikeSong>> UnlikeSong(string userId, Guid songId)
        {
            var validatePostInputs = await ValidatePostInputs(userId, songId);
            if (validatePostInputs != null)
            {
                return new Response<LikeSong>
                {
                    Success = false,
                    Message = validatePostInputs
                };
            }

            var result = await _likeSongRepository.GetLikeSong(userId, songId);
            if (!result.IsSuccess)
            {
                return new Response<LikeSong>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            var deleteResult = await _likeSongRepository.DeleteLike(result.Value.UserId, result.Value.SongId);

            if (!result.IsSuccess)
            {
                return new Response<LikeSong>
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }

            return new Response<LikeSong>()
            {
                Data = result.Value,
                Success = true
            };

        }

        private async Task<string?> ValidatePostInputs(string userId, Guid id)
        {
            if (string.IsNullOrEmpty(userId) || id == null)
            {
                return "Invalid input";
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found";
            }
            return null;
        }


    }
}
