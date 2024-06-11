using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.MusicData.Models.Genres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBlend.Core.MusicData.Mappers
{
    public static class AlbumMapper
    {

        public static AlbumQueryDto MapToAlbumQueryDto(Album album,List<SongQueryDto> songs)
        {
            return new AlbumQueryDto
            {
                Id = album.Id,
                Title = album.Title,
                ArtistId = album.Artist.Id,
                ArtistName = album.Artist.Name,

                CoverUrl = album.CoverUrl,
                ReleaseDate = album.ReleaseDate,
                Type = album.Type,
                Songs = songs
            };
        }
    }
}
