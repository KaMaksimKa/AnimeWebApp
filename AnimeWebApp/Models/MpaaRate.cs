namespace AnimeWebApp.Models
{
    public class MpaaRate: IHavingTitleAndFriendlyUrl
    {
        public int MpaaRateId { get; set; }
        public string Title { get; set; }
        public string FriendlyUrl { get; set; } = String.Empty;
        public List<Anime> Animes { get; set; }
    }
}
