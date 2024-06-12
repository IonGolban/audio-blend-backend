using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.MusicData.Models.Genres;
using System.Transactions;

namespace AudioBlend.Core.MusicData.Mappers
{
    public static class SongMapper
    {
        public static SongQueryDto MapToSongQueryDto(Song song, Artist artist, List<Genre> genres)
        {

           return new SongQueryDto
            {
                Id = song.Id,
                Title = song.Title,
                ArtistId = artist.Id,
                ArtistName = artist.Name,
                AlbumId = song.AlbumId,
                AlbumName = song.Album.Title,
                Duration = song.Duration,
                Genres = genres,
                CoverUrl = song.Album.CoverUrl,
                AudioUrl = song.AudioUrl

            };
        }

    }
}
