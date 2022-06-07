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
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<Anime> Anime
        {
            get
            {
                return _context.Animes.Include(a => a.Dubbing)
                    .Include(a => a.Genres);
            }
        }
        public IQueryable<Genre> Genres => _context.Genres;
        public IQueryable<Dubbing> Dubbing => _context.Dubbing;
        public IQueryable<TypeAnime> TypeAnimes => _context.Types;
        public IQueryable<Status> Statuses => _context.Statuses;
        public IQueryable<MpaaRate> MpaaRates => _context.MpaaRates;
        public IQueryable<Studio> Studios => _context.Studios;
    }
}
