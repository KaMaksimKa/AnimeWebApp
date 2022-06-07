namespace AnimeWebApp.Models
{
    public class SortingAnimeHandler:IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        public string TypeSort { get;}
        public SortingAnimeHandler(string typeSort)
        {
            TypeSort = typeSort.ToLower();
        }

        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            var typeSort = TypeSort.Replace("-desc", String.Empty);
            animes = typeSort switch
            {
                "rating" => animes?.OrderBy(a=>a.Rate??0),
                "date-add" => animes?.OrderBy(a => a.IdFromAnimeGo??0),
                "completed" => animes?.OrderBy(a => a.Completed??0),
                "watching" => animes?.OrderBy(a => a.Watching??0),
                "planned" => animes?.OrderBy(a=>a.Planned??0),
                _ => null
            };
            if (TypeSort.Contains("-desc"))
            {
                animes = animes?.Reverse();
            }

            return Next?.Invoke(animes);
        }
    }
}
