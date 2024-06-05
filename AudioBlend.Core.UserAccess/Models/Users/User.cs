using Microsoft.AspNetCore.Identity;

namespace AudioBlend.Core.UserAccess.Models.Users
{
    public class User : IdentityUser
    {
        public string? ImgUrl { get; set; }
    }
}
