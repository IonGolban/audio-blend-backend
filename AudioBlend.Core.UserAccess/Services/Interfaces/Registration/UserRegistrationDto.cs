namespace AudioBlend.Core.UserAccess.Services.Interfaces.Registration
{
    public class UserRegistrationDto
    {
        public required string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
