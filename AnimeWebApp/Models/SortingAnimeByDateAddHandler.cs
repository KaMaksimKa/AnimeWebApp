namespace AnimeWebApp.Models
{
    public class SortingAnimeByDateAddHandler : ISortingAnimeHandler
    {
        public string Path { get; } = "date-add";
        public IAnimeHandler? Next { get; set; }

        public IEnumerable<Anime>? Invoke(IAnimeRepository context, List<int> animeId)
        {
            var id = animeId;
            animeId = context.Anime.Where(a => id.Contains(a.AnimeId))
                .Select(a => new { Id = a.AnimeId, IdFromAnimeGo = a.IdFromAnimeGo }).OrderBy(a => a.IdFromAnimeGo)
                .Select(a => a.Id).ToList();
            return Next?.Invoke(context, animeId);

        }
    }
}