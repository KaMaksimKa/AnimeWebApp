namespace AnimeWebApp.Models
{
    public class Dubbing: IHavingTitleAndFriendlyUrl
    {
        public int DubbingId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = String.Empty;
        public List<Anime> Animes { get; set; }
    }
}
