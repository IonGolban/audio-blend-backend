using AudioBlend.Core.MusicData.Models.DTOs.Users;
using AudioBlend.Core.UserAccess.Services.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers
{
    [ApiController]
    [Route("api/v1/music-data/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCurrentUser()
        {
            var res = _userService.GetCurrentUserInfo();
            Console.WriteLine(res);
            if(!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }
        [HttpGet("isAuth")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult IsAuth()
        {
            var res = _userService.GetCurrentUserInfo();
            Console.WriteLine(res);
            if (!res.Success)
            {
                return Unauthorized(res.Message);
            }
            return Ok(res.Data);

        }

        [HttpGet("{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var res = await _userService.GetUserInfo(userId);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("update/username")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto updateUsernameDto)
        {
            var res = await _userService.UpdateUsername(updateUsernameDto);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("update/email")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto updateEmailDto)
        {
            var res = await _userService.UpdateEmail(updateEmailDto);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("update/password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var res = await _userService.UpdatePassword(updatePasswordDto);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }

        [HttpPut("update/image")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateImg([FromForm] UpdateImgDto updateImgDto)
        {
            var res = await _userService.UpdateImage(updateImgDto);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Data);
        }


    }
}
