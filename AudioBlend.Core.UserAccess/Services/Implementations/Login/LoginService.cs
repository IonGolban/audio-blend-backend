using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.UserAccess.Models.Login;
using AudioBlend.Core.UserAccess.Models.Users;
using AudioBlend.Core.UserAccess.Security;
using AudioBlend.Core.UserAccess.Services.Interfaces.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AudioBlend.Core.UserAccess.Services.Implementations.Login
{
    public class LoginService(UserManager<User> userManager, IConfiguration configuration) : ILoginService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly JwtProvider _jwtProvider = new JwtProvider(configuration);

        public async Task<Response<string>> LoginAsync(LoginModel loginModel)
        {
            var errors = await ValidateLoginModel(loginModel);
            if (errors.Count > 0)
            {
                return new Response<string>
                {
                    Success = false,
                    ErrorsKeyMessage = errors
                };
            }

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var token = _jwtProvider.GenerateJwtToken(authClaims);
            return new Response<string>
            {
                Success = true,
                Data = token
            };
        }

        private async Task<Dictionary<string, string>> ValidateLoginModel(LoginModel loginModel)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(loginModel.Email))
            {
                errors.Add("email", "Email is required");
            }
            if (string.IsNullOrEmpty(loginModel.Email))
            {
                errors.Add("password", "Password is required");
            }

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                errors.Add("email", "User not found");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!passwordValid)
            {
                errors.Add("password", "Invalid password");
            }

            return errors;



        }
    }
}
