namespace AnimeWebApp.Models
{
    public class SortingAnimeByRatingHandler: ISortingAnimeHandler
    {
        public string Path { get; } = "rating";
        public IAnimeHandler? Next { get; set; }

        public IEnumerable<Anime>? Invoke(IAnimeRepository context, List<int> animeId)
        {
            var id = animeId;
            animeId = context.Anime.Where(a => id.Contains(a.AnimeId))
                .Select(a => new {Id = a.AnimeId, Rate = a.Rate}).OrderBy(a => a.Rate)
                .Select(a => a.Id).ToList();
            return Next?.Invoke(context,animeId);

        }
    }
}
