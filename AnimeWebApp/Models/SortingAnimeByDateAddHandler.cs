namespace AnimeWebApp.Models
{
    public class SortingAnimeByDateAddHandler : ISortingAnimeHandler
    {
        public string Path { get; } = "date-add";
        public IAnimeHandler? Next { get; set; }

        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            var id = animeId;
            animeId = anime.Where(a => id.Contains(a.AnimeId))
                .Select(a => new { Id = a.AnimeId, IdFromAnimeGo = a.IdFromAnimeGo }).OrderBy(a => a.IdFromAnimeGo)
                .Select(a => a.Id);
            return Next?.Invoke(anime, animeId);

        }
    }
}