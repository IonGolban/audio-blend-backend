using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.UserAccess.Models.Registration;
using AudioBlend.Core.UserAccess.Models.Users;
using AudioBlend.Core.UserAccess.Services.Interfaces.Registration;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace AudioBlend.Core.UserAccess.Services.Implementations.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public RegistrationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<Response<UserRegistrationDto>> RegisterUser(RegistrationModel request, string role)
        {
            var errors = ValidateRegisterModel(request);
            if (errors.Count > 0)
            {
                return new Response<UserRegistrationDto>
                {
                    ValidationErrors = errors.Select(err => err.Value).ToList(),
                    Success = false
                };
            }
            var user = new User
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email 
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new Response<UserRegistrationDto>
                {
                    ValidationErrors = result.Errors.Select(err => err.Description).ToList(),
                    Success = false
                };
            }
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            await _userManager.AddToRoleAsync(user, role);

            return new Response<UserRegistrationDto>() { 
                Success = true,
                Data = new UserRegistrationDto()
                {
                    Id = user.Id,
                    Email = request.Email,
                    UserName = request.UserName
                }  
            };
        }
        private Dictionary<string,string> ValidateRegisterModel(RegistrationModel registrationModel)
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(registrationModel.UserName))
            {
                errors.Add("UserName", "UserName is required");
            }
            if (string.IsNullOrEmpty(registrationModel.Email))
            {
                errors.Add("Email", "Email is required");
            }
            if (string.IsNullOrEmpty(registrationModel.Password))
            {
                errors.Add("Password", "Password is required");
            }
            var user_mail = _userManager.FindByEmailAsync(registrationModel.Email).Result;
            if (user_mail != null)
            {
                errors.Add("Email", "Email is already taken");
            }
            var user_name = _userManager.FindByNameAsync(registrationModel.UserName).Result;
            if (user_name != null)
            {
                errors.Add("UserName", "UserName is already taken");
            }
            

            return errors;
            
        }
    }
}
