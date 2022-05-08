namespace AnimeWebApp.Models
{
    public interface IAnimeHandler
    {
        public string Path { get; }
        public IAnimeHandler? Next { get; set; }
        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId);
    }
}
