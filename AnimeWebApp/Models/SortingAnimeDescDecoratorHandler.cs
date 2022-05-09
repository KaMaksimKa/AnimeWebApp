namespace AnimeWebApp.Models
{
    public class SortingAnimeDescDecoratorHandler:ISortingAnimeHandler
    {
        public string Path { get; }
        public IAnimeHandler? Next { get; set; } 
        private ISortingAnimeHandler _sortingAnimeHandler;
        public SortingAnimeDescDecoratorHandler(ISortingAnimeHandler sortingAnimeHandler)
        {
            _sortingAnimeHandler = sortingAnimeHandler;
            /*Path = $"{_sortingAnimeHandler.Path}-desc";*/
        }
        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            _sortingAnimeHandler.Next = new SortingAnimeDescHandler() { Next = this.Next };
            return _sortingAnimeHandler.Invoke(anime,animeId);
        }
    }
}
