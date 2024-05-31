using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBlend.Core.MusicData.Mappers
{
    public static class AlbumMapper
    {

        public static AlbumQueryDto MapToAlbumQueryDto(Album album)
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
                Songs = album.Songs.Select(s => SongMapper.MapToSongQueryDto(s,album.Artist)).ToList()
            };
        }
    }
}
