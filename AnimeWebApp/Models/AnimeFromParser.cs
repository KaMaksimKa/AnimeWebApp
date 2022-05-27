namespace AnimeWebApp.Models
{
    public class AnimeFromParser
    {
        public int AnimeId { get; set; }
        public string? TitleEn { get; set; }
        public string? TitleRu { get; set; }
        public string? Type { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
        public double? Rate { get; set; }
        public string? Status { get; set; }
        public int? CountEpisode { get; set; }
        public string? MpaaRate { get; set; }
        public int? Planned { get; set; }
        public int? Completed { get; set; }
        public int? Watching { get; set; }
        public int? Dropped { get; set; }
        public int? OnHold { get; set; }
        public string? Href { get; set; }
        public string? NextEpisode { get; set; }
        public int? IdFromAnimeGo { get; set; }
        public string? Duration { get; set; }
        public List<string> Studios { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>();
        public List<string> Dubbing { get; set; } = new List<string>();
    }
}
