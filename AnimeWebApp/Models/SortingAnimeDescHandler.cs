namespace AnimeWebApp.Models
{
    public class SortingAnimeDescHandler: ISortingAnimeHandler
    {
        public string Path { get; } = "desc";
        public IAnimeHandler? Next { get; set; }
        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            animeId = animeId.Reverse();
            return Next?.Invoke(anime, animeId);
        }
    }
}
