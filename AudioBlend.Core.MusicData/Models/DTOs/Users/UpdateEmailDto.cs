namespace AudioBlend.Core.MusicData.Models.DTOs.Users
{
    public class UpdateEmailDto
    {
        public string Email { get; set; }
        public UpdateEmailDto() { }
        public UpdateEmailDto(string email)
        {
            Email = email;
        }
    }
}
