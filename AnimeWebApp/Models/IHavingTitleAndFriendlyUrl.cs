namespace AnimeWebApp.Models
{
    public interface IHavingTitleAndFriendlyUrl
    {
        public string Title { get; set; }
        public string FriendlyUrl { get; set; }
    }
}
