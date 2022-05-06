namespace AnimeWebApp.Models
{
    public class Voiceover
    {
        public int VoiceoverId { get; set; }
        public string Name { get; set; }
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }
    }
}
