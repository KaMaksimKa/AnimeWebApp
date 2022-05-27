
namespace AnimeWebApp.Models
{
    internal class ConverterAnimeFromParser
    {
        public static Anime ToAnime(AnimeFromParser animeFromParser)
        {
            return  new Anime()
            {
                Completed = animeFromParser.Completed,
                CountEpisode = animeFromParser.CountEpisode,
                Description = animeFromParser.Description,
                Dropped = animeFromParser.Dropped,
                Duration = animeFromParser.Duration,
                Href = animeFromParser.Href,
                IdFromAnimeGo = animeFromParser.IdFromAnimeGo,
                NextEpisode = animeFromParser.NextEpisode,
                Rate = animeFromParser.Rate,
                TitleEn = animeFromParser.TitleEn,
                TitleRu = animeFromParser.TitleRu,
                Studios = animeFromParser.Studios.Select(s=>new Studio(){Title = s}).ToList(),
                Status = animeFromParser.Status!=null?new Status(){Title = animeFromParser.Status}:null,
                Type = animeFromParser.Type != null ? new TypeAnime() { Title = animeFromParser.Type } : null,
                Genres = animeFromParser.Genres.Select(g => new Genre() { Title = g }).ToList(),
                MpaaRate = animeFromParser.MpaaRate != null ? new MpaaRate() { Title = animeFromParser.MpaaRate } : null,
                Dubbing = animeFromParser.Dubbing.Select(d => new Dubbing() { Title = d }).ToList(),
                OnHold = animeFromParser.OnHold,
                Year = animeFromParser.Year,
                Watching = animeFromParser.Watching
            };
        }

        public static List<Anime> ToAnime(IEnumerable<AnimeFromParser> animeFromParsers)
        {
            return animeFromParsers.Select(ToAnime).ToList();
        }
    }
}
