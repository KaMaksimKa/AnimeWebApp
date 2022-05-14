namespace AnimeWebApp.Models
{
    public class SortingAnimeDescHandler: ISortingAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        public IEnumerable<Anime>? Invoke(IAnimeRepository context, List<int> animeId)
        {
            animeId.Reverse();
            return Next?.Invoke(context, animeId);
        }
    }
}
