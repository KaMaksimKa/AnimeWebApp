namespace AnimeWebApp.Models
{
    public interface IPagingAnimeHandler:IAnimeHandler
    {
        public int AnimePerPage { get; set; }
        public int NumberPage { get; set; }
    }
}
