using AudioBlend.Core.MusicData.Models.Genres;
using AudioBlend.Core.MusicData.Models.Likes;

namespace AudioBlend.Core.MusicData.Domain.Artists
{
    public class Artist
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ImgUrl { get; private set; }
        public List<Guid> GenresIds { get; private set; } = [];
        public int Followers { get; set; }
        public ICollection<FollowArtist>? FollowedByUsers { get; set; }
        public string UserId { get; private set; } = string.Empty;

        public Artist()
        {
            Id = Guid.NewGuid();
        }
        public Artist(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public Artist(Guid id, string name, string userId)
        {
            Id = id;
            Name = name;
            UserId = userId;
        }

            public Artist(Guid id, string name, string imgUrl, List<Guid> artistGenres)
        {
            Name = name;
            ImgUrl = imgUrl;
            GenresIds = artistGenres;
            Id = id;
        }
        public Artist(Guid id, string name, string imgUrl, List<Guid> artistGenres, int followers)
        {
            Name = name;
            ImgUrl = imgUrl;
            GenresIds = artistGenres;
            Id = id;
            Followers = followers;
        }

        public void setImage(string imgUrl)
        {
            ImgUrl = imgUrl;
        }
        public void setGenres(List<Guid> genres)
        {
            GenresIds = genres;
        }

        public void setFollowers(int followers)
        {
            Followers = followers;
        }
        public void setUserId(string userId)
        {
            UserId = userId;
        }

        public void setName(string name)
        {
            Name = name;
        }
    }
}
