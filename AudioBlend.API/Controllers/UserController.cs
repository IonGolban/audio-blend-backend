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

    }
}
