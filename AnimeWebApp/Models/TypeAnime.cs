namespace AnimeWebApp.Models
{
    public class TypeAnime
    {
        public int TypeAnimeId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = String.Empty;
        public List<Anime> Animes { get; set; }
    }
}
