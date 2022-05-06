namespace AnimeWebApp.Models
{
    public interface IAnimeRepository
    {
        public IQueryable<Anime> Anime { get; }
        public IQueryable<Genre> Genre { get; } 
        public IQueryable<Voiceover> Voiceover { get; }
    }
}
