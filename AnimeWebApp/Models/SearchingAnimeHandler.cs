namespace AnimeWebApp.Models
{
    public class SearchingAnimeHandler:IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        private string _search;
        public SearchingAnimeHandler(string search)
        {
            _search = search;
        }
        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            animes = animes?.Where(a => a.NameRu == null || a.NameRu.ToLower().Contains(_search));
            return Next?.Invoke(animes);
        }
    }
}
