namespace AnimeWebApp.Models
{
    public class SortingAnimeByWatchingHandler: ISortingAnimeHandler
    {
        public string Path { get; } = "watching";
        public IAnimeHandler? Next { get; set; }

    public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
    {
        var id = animeId;
        animeId = anime.Where(a => id.Contains(a.AnimeId))
            .Select(a => new { Id = a.AnimeId, Watching = a.Watching }).OrderBy(a => a.Watching)
            .Select(a => a.Id);
        return Next?.Invoke(anime, animeId);

    }
    }
}