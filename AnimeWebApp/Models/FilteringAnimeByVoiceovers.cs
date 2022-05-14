namespace AnimeWebApp.Models
{
    public class FilteringAnimeByVoiceovers : IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        public List<string>? IncludeData { get; set; }
        public List<string>? ExcludeData { get; set; }

        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            animes = animes?.Where(a => a.Voiceovers.Any(v => IncludeData == null || IncludeData.Contains(v.NameRu)));
            animes = animes?.Where(a => !a.Voiceovers.Any(v => ExcludeData == null || ExcludeData.Contains(v.NameRu)));
            return Next?.Invoke(animes);
        }
    }
}
