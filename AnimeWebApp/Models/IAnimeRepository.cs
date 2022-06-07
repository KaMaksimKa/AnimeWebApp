namespace AnimeWebApp.Models
{
    public interface IAnimeRepository
    {
        public IQueryable<Anime> Anime { get; }
        public IQueryable<Genre> Genres { get; } 
        public IQueryable<Dubbing> Dubbing { get; }
        public IQueryable<TypeAnime> TypeAnimes { get; }
        public IQueryable<Status> Statuses { get; }
        public IQueryable<MpaaRate> MpaaRates { get; }
        public IQueryable<Studio> Studios { get; }
        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
