﻿using AudioBlend.Core.MusicData.Models.Genres;

namespace AudioBlend.Core.MusicData.Models.DTOs.Artists
{
    public class ArtistQueryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<Genre> Genres { get;  set; }
        public int Followers { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
