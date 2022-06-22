using Microsoft.EntityFrameworkCore;

namespace AnimeWebApp.Models
{
    public class WriterAnimeToDb:IDisposable
    {
        private ApplicationContext _context;
        public WriterAnimeToDb(ApplicationContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        private List<Anime> PrepareAnimes(List<Anime> animes)
        {
            var genres = _context.Genres.ToList();
            var dubbings = _context.Dubbing.ToList();
            var statuses = _context.Statuses.ToList();
            var mpaaRates = _context.MpaaRates.ToList();
            var types = _context.Types.ToList();
            var studios = _context.Studios.ToList();


            var prepareAnimes = animes.ToList();
            foreach (var anime in prepareAnimes)
            {
                var newAnimeGenres = new List<Genre>();
                foreach (var genreOld in anime.Genres)
                {
                    if (!genres.Select(g => g.Title).Contains(genreOld.Title))
                    {
                        genres.Add(genreOld);
                    }
                    newAnimeGenres.Add(genres.First(g => g.Title == genreOld.Title));
                }
                anime.Genres = newAnimeGenres;


                var newDubbings = new List<Dubbing>();
                foreach (var dubbingsOld in anime.Dubbing)
                {
                    if (!dubbings.Select(d => d.Title).Contains(dubbingsOld.Title))
                    {
                        dubbings.Add(dubbingsOld);
                    }
                    newDubbings.Add(dubbings.First(d => d.Title == dubbingsOld.Title));
                }
                anime.Dubbing = newDubbings;


                var newStudios = new List<Studio>();
                foreach (var studiosOld in anime.Studios)
                {
                    if (!studios.Select(d => d.Title).Contains(studiosOld.Title))
                    {
                        studios.Add(studiosOld);
                    }
                    newStudios.Add(studios.First(d => d.Title == studiosOld.Title));
                }
                anime.Studios = newStudios;

                if (anime.Status is {} status && !statuses.Select(s => s.Title).Contains(status.Title))
                {
                    statuses.Add(status);
                }
                if (anime.Status is { } )
                {
                    anime.Status = statuses.First(s => s.Title == anime.Status.Title);
                }


                if (anime.MpaaRate is { } mpaaRate && !mpaaRates.Select(m => m.Title).Contains(mpaaRate.Title))
                {
                    mpaaRates.Add(mpaaRate);
                }
                if (anime.MpaaRate is { })
                {
                    anime.MpaaRate = mpaaRates.First(m => m.Title == anime.MpaaRate.Title);
                }


                if (anime.Type is { } type && !types.Select(t => t.Title).Contains(type.Title))
                {
                    types.Add(type);
                }
                if (anime.Type is { })
                {
                    anime.Type = types.First(t => t.Title == anime.Type.Title);
                }

            }
            return prepareAnimes;
        }
        public void AddAnime(Anime anime)
        {
            AddAnimeRange(new List<Anime>{anime});
        }

        public void AddAnimeRange(List<Anime> animes)
        {
            animes = PrepareAnimes(animes);
            _context.AddRange(animes);
            _context.SaveChanges();
        }
        public void AddOrUpDateAnime(Anime anime, bool isUpdatingNull = false)
        {
            AddOrUpDateAnimeRange(new List<Anime>{anime}, isUpdatingNull);
        }
        public void AddOrUpDateAnimeRange(List<Anime> animes,bool isUpdatingNull = false)
        {
            animes = PrepareAnimes(animes);
            _context.Animes.Include(a=>a.Studios).Load();
            _context.Animes.Include(a => a.Genres).Load();
            _context.Animes.Include(a => a.Status).Load();
            _context.Animes.Include(a => a.Type).Load();
            _context.Animes.Include(a => a.MpaaRate).Load();
            _context.Animes.Include(a => a.Dubbing).Load();
            var addingAnime = animes.Where(a => !(_context.Animes.Select(an => an.IdFromAnimeGo)).Contains(a.IdFromAnimeGo)).ToList();
            
            
            var updatingAnime = animes.Where(a => (_context.Animes.Select(an => an.IdFromAnimeGo)).Contains(a.IdFromAnimeGo)).ToList();
            var preUpdateAnime = _context.Animes.Where(a => updatingAnime.Select(an => an.IdFromAnimeGo).Contains(a.IdFromAnimeGo)).ToList();
            foreach (var anime in preUpdateAnime)
            {
                var upAnime = updatingAnime.First(a => a.IdFromAnimeGo == anime.IdFromAnimeGo);

                anime.Studios = upAnime.Studios.Count != 0 || isUpdatingNull ? upAnime.Studios : anime.Studios;
                anime.Genres = upAnime.Genres.Count != 0 || isUpdatingNull ? upAnime.Genres : anime.Genres;
                anime.Dubbing = upAnime.Dubbing.Count != 0 || isUpdatingNull ? upAnime.Dubbing : anime.Dubbing;
                anime.Status = upAnime.Status!=null || isUpdatingNull ? upAnime.Status: anime.Status;
                anime.Type = upAnime.Type != null || isUpdatingNull ? upAnime.Type : anime.Type;
                anime.MpaaRate = upAnime.MpaaRate != null || isUpdatingNull ? upAnime.MpaaRate : anime.MpaaRate;
                anime.TitleRu = upAnime.TitleRu != null || isUpdatingNull ? upAnime.TitleRu : anime.TitleRu;
                anime.TitleEn = upAnime.TitleEn != null || isUpdatingNull ? upAnime.TitleEn : anime.TitleEn;
                anime.CountEpisode = upAnime.CountEpisode != null || isUpdatingNull ? upAnime.CountEpisode : anime.CountEpisode;
                anime.Year = upAnime.Year != null || isUpdatingNull ? upAnime.Year : anime.Year;
                anime.Description = upAnime.Description != null || isUpdatingNull ? upAnime.Description : anime.Description;
                anime.Rate = upAnime.Rate != null || isUpdatingNull ? upAnime.Rate : anime.Rate;
                anime.Planned = upAnime.Planned != null || isUpdatingNull ? upAnime.Planned : anime.Planned;
                anime.Completed = upAnime.Completed != null || isUpdatingNull ? upAnime.Completed : anime.Completed;
                anime.Watching = upAnime.Watching != null || isUpdatingNull ? upAnime.Watching : anime.Watching;
                anime.Dropped = upAnime.Dropped != null || isUpdatingNull ? upAnime.Dropped : anime.Dropped;
                anime.OnHold = upAnime.OnHold != null || isUpdatingNull ? upAnime.OnHold : anime.OnHold;
                anime.Href = upAnime.Href != null || isUpdatingNull ? upAnime.Href : anime.Href;
                anime.NextEpisode = upAnime.NextEpisode != null || isUpdatingNull ? upAnime.NextEpisode : anime.NextEpisode;
                anime.Duration = upAnime.Duration != null || isUpdatingNull ? upAnime.Duration : anime.Duration;
            }
            _context.AddRange(addingAnime);
            _context.SaveChanges();
        }

        
    }
}
