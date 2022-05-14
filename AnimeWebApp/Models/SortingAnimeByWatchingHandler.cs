namespace AnimeWebApp.Models
{
    public class SortingAnimeByWatchingHandler: ISortingAnimeHandler
    {
        public string Path { get; } = "watching";
        public IAnimeHandler? Next { get; set; }

    public IEnumerable<Anime>? Invoke(IAnimeRepository context, List<int> animeId)
    {
        var id = animeId;
        animeId = context.Anime.Where(a => id.Contains(a.AnimeId))
            .Select(a => new { Id = a.AnimeId, Watching = a.Watching }).OrderBy(a => a.Watching)
            .Select(a => a.Id).ToList();
        return Next?.Invoke(context, animeId);

    }
    }
}