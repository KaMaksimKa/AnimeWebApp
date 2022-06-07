namespace AnimeWebApp.Models.ViewModels
{
    public class FilteringInfo
    {
        private readonly FilteringData _filteringData;
        public List<Genre> AllGenres { get; init; } = new List<Genre>();
        public List<Genre> CurrentGenres => AllGenres.Where(a => _filteringData.Genres.Contains(a.FriendlyUrl)).ToList();
        public List<Dubbing> AllDubbing{ get; init; } = new List<Dubbing>();
        public List<Dubbing> CurrentDubbing => AllDubbing.Where(d => _filteringData.Dubbing.Contains(d.FriendlyUrl)).ToList();
        public List<TypeAnime> AllTypes { get; init; } = new List<TypeAnime>();
        public List<TypeAnime> CurrentTypes => AllTypes.Where(t => _filteringData.Types.Contains(t.FriendlyUrl)).ToList();
        public List<Status> AllStatuses { get; init; } = new List<Status>();
        public List<Status> CurrentStatuses => AllStatuses.Where(s => _filteringData.Statuses.Contains(s.FriendlyUrl)).ToList();

        public FilteringInfo(FilteringData filteringData)
        {
            _filteringData = filteringData;
        }
    }
}
