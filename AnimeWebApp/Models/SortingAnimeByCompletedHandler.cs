namespace AnimeWebApp.Models
{
    public class SortingAnimeByCompletedHandler : ISortingAnimeHandler
    {
        public string Path { get; } = "completed";
        public IAnimeHandler? Next { get; set; }

        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            var id = animeId;
            animeId = anime.Where(a => id.Contains(a.AnimeId))
                .Select(a => new { Id = a.AnimeId, Completed = a.Completed}).OrderBy(a => a.Completed)
                .Select(a => a.Id);
            return Next?.Invoke(anime, animeId);

        }
    }
}