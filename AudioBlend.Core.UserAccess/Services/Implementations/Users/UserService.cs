using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using AudioBlend.Core.UserAccess.Models.Users;
using AudioBlend.Core.UserAccess.Services.Interfaces.Users;
using AudioBlend.Core.MusicData.Services.Interfaces;

namespace AudioBlend.Core.UserAccess.Services.Implementations.Users
{
    public class UserService(UserManager<User> userManger, UserManager<IdentityUser> appUserManger, ICurrentUserService currentUserService) : IUserService
    {
        private readonly UserManager<User> _userManger = userManger;
        private readonly UserManager<IdentityUser> _appUserManger = appUserManger;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public Response<UserDto> GetCurrentUserInfo()
        {
            var userId = _currentUserService.GetUserId;
            var user = userManger.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = new List<string>() { "User not found" },
                    Success = false
                };
            }
            return new Response<UserDto>()
            {
                Success = true,
                Data = new UserDto()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    ImgUrl = user.ImgUrl
                    
                }
            };
        }

        
    }
}
