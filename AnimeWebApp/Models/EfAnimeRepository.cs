using Microsoft.EntityFrameworkCore;

namespace AnimeWebApp.Models
{
    public class EfAnimeRepository:IAnimeRepository
    {
        private ApplicationContext _context;

        public EfAnimeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<Anime> Anime
        {
            get
            {
                return _context.Animes.Include(a => a.Dubbing)
                    .Include(a => a.Genres).AsNoTracking();
            }
        }
        public IQueryable<Genre> Genres => _context.Genres.AsNoTracking();
        public IQueryable<Dubbing> Dubbing => _context.Dubbing.AsNoTracking();
        public IQueryable<TypeAnime> TypeAnimes => _context.Types.AsNoTracking();
        public IQueryable<Status> Statuses => _context.Statuses.AsNoTracking();
        public IQueryable<MpaaRate> MpaaRates => _context.MpaaRates.AsNoTracking();
        public IQueryable<Studio> Studios => _context.Studios.AsNoTracking();
    }
}
