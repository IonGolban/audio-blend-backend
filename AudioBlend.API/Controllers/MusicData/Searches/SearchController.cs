﻿using AudioBlend.Core.MusicData.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Searches
{

    [ApiController]
    [Route("api/v1/music-data/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAll([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }

            var res = await _searchService.SearchAll(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("albums")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAlbums([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }
            var res = await _searchService.SearchAlbums(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("songs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchSongs([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }
            var res = await _searchService.SearchSongs(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpGet("artists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchArtists([FromQuery] string query, [FromQuery] int count)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required");
            }
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }
            var res = await _searchService.SearchArtists(query, count);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

    }
}