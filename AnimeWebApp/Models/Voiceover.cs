namespace AnimeWebApp.Models
{
    public class Voiceover
    {
        public int VoiceoverId { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public virtual List<Anime> Anime { get; set; }
    }
}
