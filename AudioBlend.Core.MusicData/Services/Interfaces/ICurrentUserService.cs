using System.Security.Claims;

namespace AudioBlend.Core.MusicData.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string? GetUserRole { get; }
        string? GetUserId { get; }
        ClaimsPrincipal GetCurrentClaimsPrincipal();
    }
}
