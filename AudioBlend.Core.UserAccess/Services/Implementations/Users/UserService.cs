using AudioBlend.Core.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using AudioBlend.Core.UserAccess.Models.Users;
using AudioBlend.Core.UserAccess.Services.Interfaces.Users;
using AudioBlend.Core.MusicData.Services.Interfaces;
using AudioBlend.Core.MusicData.Models.DTOs.Users;

namespace AudioBlend.Core.UserAccess.Services.Implementations.Users
{
    public class UserService(
        IAzureBlobStorageService azureBlobStorageService, UserManager<User> userManger, UserManager<IdentityUser> appUserManger, ICurrentUserService currentUserService) : IUserService
    {
        private readonly UserManager<User> _userManger = userManger;
        private readonly UserManager<IdentityUser> _appUserManger = appUserManger;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IAzureBlobStorageService _azureBlobStorageService = azureBlobStorageService;

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
        public async Task<Response<UserDto>> GetUserInfo(string userId) {
            var user = await userManger.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response<UserDto>()
                {
                    Message = "User not found",
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
        public async Task<Response<UserDto>> UpdateUsername(UpdateUsernameDto updateUsernameDto)
        {
            var userId = _currentUserService.GetUserId;
            var user = await userManger.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = new List<string>() { "User not found" },
                    Success = false
                };
            }
            user.UserName = updateUsernameDto.Username;
            var result = await userManger.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = result.Errors.Select(e => e.Description).ToList(),
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

        public async Task<Response<UserDto>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var userId = _currentUserService.GetUserId;
            var user = await userManger.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return new Response<UserDto>()
                {
                    Message = "User not found",
                    Success = false
                };
            }
            if(updatePasswordDto.NewPassword != updatePasswordDto.ConfirmNewPassword)
            {
                return new Response<UserDto>()
                {
                    Message = "Passwords do not match",
                    Success = false
                };
            }

            var isCorrectCurrentPassword = await userManger.CheckPasswordAsync(user, updatePasswordDto.CurrentPassword);
            if (!isCorrectCurrentPassword)
            {
                return new Response<UserDto>()
                {
                    Message = "Current password is incorrect",
                    Success = false
                };
            }

            var result = await userManger.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = result.Errors.Select(e => e.Description).ToList(),
                    Success = false,
                    Message = "Password could not be updated"
                };
            }

            return new Response<UserDto>()
            {
                Success = true,
                Message = "Password updated successfully",
                Data = new UserDto()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    ImgUrl = user.ImgUrl
                }
            };

        }

        public async Task<Response<UserDto>> UpdateEmail(UpdateEmailDto updateEmailDto)
        {
            var userId = _currentUserService.GetUserId;
            var user = await userManger.FindByIdAsync(userId.ToString());
            if (user == null)
            { 
                return new Response<UserDto>()
                {
                    ValidationErrors = new List<string>() { "User not found" },
                    Success = false
                };
            }
            user.Email = updateEmailDto.Email;
            var result = await userManger.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = result.Errors.Select(e => e.Description).ToList(),
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

        public async Task<Response<UserDto>> UpdateImage(UpdateImgDto updateImgDto)
        {
            var userId = _currentUserService.GetUserId;
            var user = await userManger.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = new List<string>() { "User not found" },
                    Success = false
                };
            }
            var ImgUrl = await _azureBlobStorageService.UploadFileToBlobAsync(updateImgDto.Image);
            
            if(!ImgUrl.IsSuccess)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = new List<string>() { ImgUrl.ErrorMsg },
                    Success = false,
                    Message = "Image could not be uploaded"
                };
            }
            
            user.ImgUrl = ImgUrl.Value;
            var result = await userManger.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Response<UserDto>()
                {
                    ValidationErrors = result.Errors.Select(e => e.Description).ToList(),
                    Success = false,
                    Message = "Cannot udpate image profile for user"
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
