using Microsoft.AspNetCore.Http;

namespace AudioBlend.Core.MusicData.Models.DTOs.Users
{
    public class UpdateImgDto
    {
        public IFormFile Image { get; set; }
        public UpdateImgDto() { }
        public UpdateImgDto(IFormFile image)
        {
            Image = image;
        }
    }
}
