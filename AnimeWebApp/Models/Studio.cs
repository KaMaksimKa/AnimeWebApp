namespace AnimeWebApp.Models
{
    public class Studio: IHavingTitleAndFriendlyUrl
    {
        public int StudioId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = String.Empty;
        public List<Anime> Animes { get; set; }
    }
}
