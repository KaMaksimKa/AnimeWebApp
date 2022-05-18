﻿namespace AnimeWebApp.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = String.Empty;
        public List<Anime> Animes { get; set; }
    }
}
