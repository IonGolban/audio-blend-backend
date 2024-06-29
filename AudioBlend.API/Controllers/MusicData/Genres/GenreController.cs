using AudioBlend.Core.MusicData.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.MusicData.Genres
{
    [ApiController]
    [Route("api/v1/music-data/genres")]
    public class GenreController(IGenreRepository genreRepository) : ControllerBase
    {
        private readonly IGenreRepository _genreRepository = genreRepository;

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var result = await _genreRepository.GetAll();
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMsg);
            }
            return Ok(result.Value);
        }
    }
}
