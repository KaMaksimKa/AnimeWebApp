namespace AnimeWebApp.Models
{
    public class SortingAnimeDescDecoratorHandler:ISortingAnimeHandler
    {
        public IAnimeHandler? Next { get; set; } 
        private ISortingAnimeHandler _sortingAnimeHandler;
        public SortingAnimeDescDecoratorHandler(ISortingAnimeHandler sortingAnimeHandler)
        {
            _sortingAnimeHandler = sortingAnimeHandler;
        }
        public IEnumerable<Anime>? Invoke(IAnimeRepository context, List<int> animeId)
        {
            _sortingAnimeHandler.Next = new SortingAnimeDescHandler() { Next = this.Next };
            return _sortingAnimeHandler.Invoke(context, animeId);
        }
    }
}
