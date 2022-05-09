namespace AnimeWebApp.Models
{
    public class GetingTotalPagesAnimeHandler:IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            TotalPages =(int)Math.Ceiling((decimal) animeId.Count() / PageSize);
            return Next?.Invoke(anime, animeId);
        }
    }
}
