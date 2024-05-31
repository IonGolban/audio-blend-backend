
namespace AudioBlend.Core.MusicData.Models.Likes
{
    public class Like
    {
        public string UserId { get; private set; }
        public Like() { }

        public Like(string userId)
        {
            UserId = userId;
        }


    }
}
