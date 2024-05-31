using AudioBlend.Core.MusicData.Models.DTOs;
using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.UserAccess.Models.Registration;
using Microsoft.AspNetCore.Identity;

namespace AudioBlend.Core.UserAccess.Services.Interfaces.Registration
{
    public interface IRegistrationService
    {
        Task<Response<UserRegistrationDto>> RegisterUser(RegistrationModel request, string role);
    }
}
