namespace AnimeWebApp.Models
{
    public class Status: IHavingTitleAndFriendlyUrl
    {
        public int StatusId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = String.Empty;
        public List<Anime> Animes { get; set; }
    }
}
