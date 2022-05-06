namespace AnimeWebApp.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }
    }
}
