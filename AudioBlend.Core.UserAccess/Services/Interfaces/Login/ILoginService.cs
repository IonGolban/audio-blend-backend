using AudioBlend.Core.Shared.Responses;
using AudioBlend.Core.UserAccess.Models.Login;

namespace AudioBlend.Core.UserAccess.Services.Interfaces.Login
{
    public interface ILoginService
    {
        Task<Response<string>> LoginAsync(LoginModel loginModel);
    }
}
