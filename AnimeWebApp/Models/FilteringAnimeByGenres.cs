namespace AnimeWebApp.Models
{
    public class FilteringAnimeByGenres: IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        public List<string>? IncludeData { get; set; }
        public List<string>? ExcludeData { get; set; }
        
        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            animes = animes?.Where(a => a.Genres.Any(g=> IncludeData == null || IncludeData.Contains(g.NameRu)));
            animes = animes?.Where(a => !a.Genres.Any(g => ExcludeData == null || ExcludeData.Contains(g.NameRu)));
            return Next?.Invoke(animes);
        }
    }
}
