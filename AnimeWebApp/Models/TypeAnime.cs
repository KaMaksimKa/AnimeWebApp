namespace AnimeWebApp.Models
{
    public class TypeAnime: IHavingTitleAndFriendlyUrl
    {
        public int TypeAnimeId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = String.Empty;
        public List<Anime> Animes { get; set; }
    }
}
