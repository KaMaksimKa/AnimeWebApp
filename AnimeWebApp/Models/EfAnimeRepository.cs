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
                return _context.Anime.Include(a=>a.Voiceovers).Include(a=>a.Genres).AsNoTracking()/*.Select(a=>new Anime()
                {
                    AnimeId = a.AnimeId,
                    Rate = a.Rate,
                    Completed = a.Completed,
                    IdFromAnimeGo = a.IdFromAnimeGo,
                    NameRu = a.NameRu,
                    Watching = a.Watching,
                    Voiceovers = a.Voiceovers,
                    Description = a.Description,
                    Planned = a.Planned,
                    Genres = a.Genres,
                    Dropped = a.Dropped,
                    OnHold = a.OnHold,
                    CountEpisode = a.CountEpisode,
                    Duration = a.Duration,
                    Href = a.Href,
                    MpaaRate = a.MpaaRate,
                    Type = a.Type,
                    Year = a.Year,
                    NameEn = a.NameEn,
                    Status = a.Status,
                    NextEpisode = a.NextEpisode,
                    Studio = a.Studio,
                })*/;
            }
        }

        public IQueryable<Genre> Genre => _context.Genre.Include(g=>g.Anime);
        public IQueryable<Voiceover> Voiceover => _context.Voiceover.Include(v=>v.Anime);
    }
}
