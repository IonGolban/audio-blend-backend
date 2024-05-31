using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.UserAccess.Models.Users;

namespace AudioBlend.Core.UserAccess.Services.Interfaces.Users
{
    public interface IUserService
    {
        Response<UserDto> GetCurrentUserInfo();

    }
}
