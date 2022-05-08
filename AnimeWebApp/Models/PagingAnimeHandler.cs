namespace AnimeWebApp.Models
{
    public class PagingAnimeHandler:IPagingAnimeHandler
    {
        public string Path => $"{NumberPage}";
        public int AnimePerPage { get; set; } = 10;
        public int NumberPage { get; set; } = 1;
        public IAnimeHandler? Next { get; set; } = new StandartAnimeHandler();

        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            if (NumberPage > 0 && (NumberPage - 1) * AnimePerPage < animeId.Count())
            {
                animeId = animeId.Skip(AnimePerPage * (NumberPage - 1)).Take(AnimePerPage);
                return Next?.Invoke(anime, animeId);
            }
            else
            {
                return null;
            }
            
        }
    }
}
