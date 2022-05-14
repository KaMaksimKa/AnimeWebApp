namespace AnimeWebApp.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public virtual List<Anime> Anime { get; set; }
    }
}
