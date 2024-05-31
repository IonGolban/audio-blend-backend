using AudioBlend.Core.UserAccess.Models.Login;
using AudioBlend.Core.UserAccess.Services.Interfaces.Login;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.UserAccess
{
    [Route("api/v1/login")]
    public class LoginController(ILoginService loginService) : ControllerBase
    {
        private readonly ILoginService _loginService = loginService;

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody]LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _loginService.LoginAsync(request);

            if (!response.Success)
            {
                return Unauthorized(response.ErrorsKeyMessage);
            }

            return Ok(response.Data);
        }



    }
}
