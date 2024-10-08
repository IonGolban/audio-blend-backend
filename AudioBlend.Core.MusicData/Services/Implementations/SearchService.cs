﻿using AudioBlend.Core.MusicData.Domain.Albums;
using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Songs;
using AudioBlend.Core.MusicData.Mappers;
using AudioBlend.Core.MusicData.Models.DTOs.Albums;
using AudioBlend.Core.MusicData.Models.DTOs.Searches;
using AudioBlend.Core.MusicData.Models.DTOs.Songs;
using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Repositories.Interfaces;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.Shared.Responses;

namespace AudioBlend.Core.MusicData.Services.Implementations
{
    public class SearchService(IGenreRepository genreRepository,IAlbumRepository albumRepository, ISongRepository songRepository, IArtistRepository artistRepository) : ISearchService
    {
        private readonly IAlbumRepository _albumRepository = albumRepository;
        private readonly ISongRepository _songRepository = songRepository;
        private readonly IArtistRepository _artistRepository = artistRepository;
        private readonly IGenreRepository _genreRepository = genreRepository;
        private const int Treshold = 10;
        public async Task<Response<MultipleSearchDto>> SearchAll(string query, int count)
        {
            var resultAlbums = await _albumRepository.SearchByAlbumName(query, Treshold, count);
            var resultSongs = await _songRepository.SearchBySongName(query, Treshold, count);
            var resultArtists = await _artistRepository.SearchByArtistName(query, Treshold, count);

            if (resultAlbums.IsSuccess == false || resultSongs.IsSuccess == false || resultArtists.IsSuccess == false)
            {
                return new Response<MultipleSearchDto>()
                {
                    Success = false,
                    Message = "Error while searching or cannot be found by this query"
                };
            }

            var topByScore = new Dictionary<Guid, int>();

            foreach (var album in resultAlbums.Value)
            {
                if (!topByScore.ContainsKey(album.Result.Id))
                {
                    topByScore.Add(album.Result.Id, album.Score);
                }
            }

            foreach (var song in resultSongs.Value)
            {
                if (!topByScore.ContainsKey(song.Result.Id))
                {
                    topByScore.Add(song.Result.Id, song.Score);
                }
            }

            foreach (var artist in resultArtists.Value)
            {
                if (!topByScore.ContainsKey(artist.Result.Id))
                {
                    topByScore.Add(artist.Result.Id, artist.Score);
                }
            }

            var topResults = topByScore.OrderBy(r => r.Value).Take(count).ToList();


            var topAlbums = new List<AlbumQueryDto>();
            var topSongs = new List<SongQueryDto>();
            var topArtists = new List<Artist>();

            foreach (var result in topResults)
            {
                var album = resultAlbums.Value.FirstOrDefault(a => a.Result.Id == result.Key);
                if (album != null)
                {
                    var topSongsAlbums = new List<SongQueryDto>();
                    foreach (var songAlbum in album.Result.Songs)
                    {
                        var genres = await _genreRepository.GetBySongId(songAlbum.Id);
                        topSongsAlbums.Add(SongMapper.MapToSongQueryDto(songAlbum, songAlbum.Artist, genres.Value));
                    }
                    topAlbums.Add(AlbumMapper.MapToAlbumQueryDto(album.Result, topSongsAlbums));
                    continue;
                }

                var song = resultSongs.Value.FirstOrDefault(s => s.Result.Id == result.Key);
                if (song != null)
                {
                    var genres = await _genreRepository.GetBySongId(song.Result.Id);
                    topSongs.Add(SongMapper.MapToSongQueryDto(song.Result, song.Result.Artist, genres.Value));
                    continue;
                }

                var artist = resultArtists.Value.FirstOrDefault(ar => ar.Result.Id == result.Key);
                if (artist != null)
                {
                    topArtists.Add(artist.Result);
                }
            }




            Console.WriteLine(topResults.Count);
            Console.WriteLine(topAlbums.Count);
            Console.WriteLine(topSongs.Count);
            Console.WriteLine(topArtists.Count);
            var response = new MultipleSearchDto()
            {
                Albums = topAlbums,
                Songs = topSongs,
                Artists = topArtists
            };

            return new Response<MultipleSearchDto>()
            {
                Data = response,
                Success = true
            };
        }


        public async Task<Response<List<AlbumQueryDto>>> SearchAlbums(string query, int count)
        {
            var result = await _albumRepository.SearchByAlbumName(query, Treshold, count);
            if(result.IsSuccess == false)
            {
                return new Response<List<AlbumQueryDto>>()  
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            List<AlbumQueryDto> albumQueryDtos = new List<AlbumQueryDto>();
            foreach(var album in result.Value)
            {
                var songs = await _songRepository.GetByAlbumId(album.Result.Id);
                if (songs.IsSuccess == false)
                {
                    return new Response<List<AlbumQueryDto>>
                    {
                        Success = false,
                        Message = songs.ErrorMsg
                    };
                }
                var songsDto = songs.Value.Select(s => SongMapper.MapToSongQueryDto(s, s.Artist, new List<Genre>())).ToList();
                
                albumQueryDtos.Add(AlbumMapper.MapToAlbumQueryDto(album.Result, songsDto));
            }



            var response = albumQueryDtos.Take(count).ToList();
            return new Response<List<AlbumQueryDto>>()
            {
                Data = response,
                Success = true
            };
        }

        public async Task<Response<List<SongQueryDto>>> SearchSongs(string query, int count)
        {
            var result = await _songRepository.SearchBySongName(query, Treshold, count);
            if (result.IsSuccess == false)
            {
                return new Response<List<SongQueryDto>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }


            var response = result.Value.Select((s) => SongMapper.MapToSongQueryDto(s.Result, s.Result.Artist, new List<Genre>())).Take(count).ToList();
            return new Response<List<SongQueryDto>>()
            {
                Data = response,
                Success = true
            };
        }

        public async Task<Response<List<Artist>>> SearchArtists(string query, int count)
        {
            var result = await _artistRepository.SearchByArtistName(query, Treshold, count);
            if (result.IsSuccess == false)
            {
                return new Response<List<Artist>>()
                {
                    Success = false,
                    Message = result.ErrorMsg
                };
            }
            var response = result.Value.Take(count).ToList();
            return new Response<List<Artist>>()
            {
                Data = response.Select(ar => ar.Result).ToList(),
                Success = true
            };
        }

        
    }
}
