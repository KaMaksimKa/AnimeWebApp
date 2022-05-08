namespace AnimeWebApp.Models
{
    public class SortingAnimeByRatingHandler: ISortingAnimeHandler
    {
        public string Path { get; } = "rating";
        public IAnimeHandler? Next { get; set; }

        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            var id = animeId;
            animeId = anime.Where(a => id.Contains(a.AnimeId))
                .Select(a => new {Id = a.AnimeId, Rate = a.Rate}).OrderBy(a => a.Rate)
                .Select(a => a.Id);
            return Next?.Invoke(anime,animeId);

        }
    }
}
