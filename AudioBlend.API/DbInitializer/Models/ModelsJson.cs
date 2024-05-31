namespace AudioBlend.API.DbInitializer.Models
{
    public class SongJsonModel
    {
        public TrackJsonModel Track { get; set; }
        public List<ArtistJsonModel> Artists { get; set; }
    }

    public class TrackJsonModel
    {
        public string Name { get; set; }
        public List<string> Artists { get; set; }
        public string Album { get; set; }
        public string ReleaseDate { get; set; }
        public int Duration { get; set; }
        public int Popularity { get; set; }
    }

    public class ArtistJsonModel
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public List<string> Genres { get; set; }
        public int Popularity { get; set; }
        public int Followers { get; set; }
        public List<AlbumJsonModel> Albums { get; set; }
    }

    public class AlbumJsonModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ReleaseDate { get; set; }
        public int TotalTracks { get; set; }
        public int Followers { get; set; }
        public string img_url { get; set; } 
        public List<TrackJsonModel> Tracks { get; set; }
    }

    public class RootJsonModel
    {
        public List<SongJsonModel> Songs { get; set; }
    }

}
