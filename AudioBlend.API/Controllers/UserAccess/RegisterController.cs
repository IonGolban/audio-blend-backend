using AudioBlend.Core.UserAccess.Models.Registration;
using AudioBlend.Core.UserAccess.Models.Roles;
using AudioBlend.Core.UserAccess.Services.Interfaces.Registration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AudioBlend.API.Controllers.UserAccess
{
    [Route("api/v1/register")]
    public class RegisterController(IRegistrationService registrationService) : ControllerBase
    {
        private readonly IRegistrationService _registrationService = registrationService;

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]RegistrationModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _registrationService.RegisterUser(request, UserRoles.User);

            if (!response.Success)
            {
                return BadRequest(response.ValidationErrors);
            }

            return Ok(response);
        }
    }
}
