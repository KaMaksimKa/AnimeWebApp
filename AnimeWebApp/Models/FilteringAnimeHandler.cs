namespace AnimeWebApp.Models
{
    public class FilteringAnimeHandler:IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        private FilteringData _filter;
        public FilteringAnimeHandler(FilteringData filter)
        {
            _filter = filter;
        }
        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            if (_filter.Genres.Count > 0)
            {
                animes = animes?.Where(a => a.Genres.Any(g => _filter.Genres.Contains(g.FriendlyUrl)));
            }
            if (_filter.Dubbing.Count > 0)
            {
                animes = animes?.Where(a => a.Dubbing.Any(d => _filter.Dubbing.Contains(d.FriendlyUrl)));
            }
            if (_filter.Types.Count > 0)
            {
                animes = animes?.Where(a => a.Type==null || _filter.Types.Contains(a.Type.FriendlyUrl));
            }
            return Next?.Invoke(animes);
        }
    }
}
