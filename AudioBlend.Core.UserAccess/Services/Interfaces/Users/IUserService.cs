using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.MusicData.Models.DTOs.Users;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.UserAccess.Models.Users;

namespace AudioBlend.Core.UserAccess.Services.Interfaces.Users
{
    public interface IUserService
    {
        Response<UserDto> GetCurrentUserInfo();
        Task<Response<UserDto>> GetUserInfo(string userId);
        Task<Response<UserDto>> UpdateUsername(UpdateUsernameDto updateUsernameDto);
        Task<Response<UserDto>> UpdatePassword(UpdatePasswordDto updatePasswordDto);
        Task<Response<UserDto>> UpdateEmail(UpdateEmailDto updateEmailDto);
        Task<Response<UserDto>> UpdateImage(UpdateImgDto updateImg);
    }
}
