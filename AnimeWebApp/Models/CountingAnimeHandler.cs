namespace AnimeWebApp.Models
{
    public class CountingAnimeHandler:IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        public int NumberOfAnime { get; set; }

        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            NumberOfAnime = animes?.Count()??0;
            return Next?.Invoke(animes);
        }
    }
}
