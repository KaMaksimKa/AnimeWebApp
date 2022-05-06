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

        public IQueryable<Anime> Anime => _context.Anime.Include(a=>a.Genres).Include(a=>a.Voiceovers);
        public IQueryable<Genre> Genre => _context.Genre;
        public IQueryable<Voiceover> Voiceover => _context.Voiceover;
    }
}
