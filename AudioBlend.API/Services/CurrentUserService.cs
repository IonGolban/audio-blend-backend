using AudioBlend.Core.MusicData.Services.Interfaces;
using System.Security.Claims;

namespace AudioBlend.API.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public string? GetUserRole => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        public string? GetUserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public ClaimsPrincipal GetCurrentClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext is { User: not null }
                ? _httpContextAccessor.HttpContext.User
                : null!;
        }
    }
}