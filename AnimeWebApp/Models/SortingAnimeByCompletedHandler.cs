namespace AnimeWebApp.Models
{
    public class SortingAnimeByCompletedHandler : ISortingAnimeHandler
    {
        public string Path { get; } = "completed";
        public IAnimeHandler? Next { get; set; }

        public IEnumerable<Anime>? Invoke(IAnimeRepository context, List<int> animeId)
        {
            var id = animeId;
            animeId = context.Anime.Where(a => id.Contains(a.AnimeId))
                .Select(a => new { Id = a.AnimeId, Completed = a.Completed}).OrderBy(a => a.Completed)
                .Select(a => a.Id).ToList();
            return Next?.Invoke(context, animeId);

        }
    }
}