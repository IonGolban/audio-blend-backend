using AudioBlend.Core.MusicData.Domain.Artists;
using AudioBlend.Core.MusicData.Domain.Songs;

namespace AudioBlend.Core.MusicData.Models.Genres
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Genre() { }
        public Genre(string name)
        {
            Name = name;
        }
        public Genre(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
