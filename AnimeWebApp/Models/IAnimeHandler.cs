namespace AnimeWebApp.Models
{
    public interface IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes);
    }
}
