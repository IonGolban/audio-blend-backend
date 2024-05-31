namespace AudioBlend.Core.MusicData.Domain.Artists
{
    public class Artist
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ImgUrl { get; private set; }
        public List<string> Genres { get; private set; }
        public int Followers { get; set; }
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
        public Artist(Guid id, string name, string imgUrl, List<string> genres)
        {
            Name = name;
            ImgUrl = imgUrl;
            Genres = genres;
            Id = id;
        }
        public Artist(Guid id, string name, string imgUrl, List<string> genres,int followers)
        {
            Name = name;
            ImgUrl = imgUrl;
            Genres = genres;
            Id = id;
            Followers = followers;
        }

    }
}
