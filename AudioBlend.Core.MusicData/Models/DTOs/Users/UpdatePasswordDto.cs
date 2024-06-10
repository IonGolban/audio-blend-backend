namespace AudioBlend.Core.MusicData.Models.DTOs.Users
{
    public class UpdatePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public UpdatePasswordDto() { }


    }
}
