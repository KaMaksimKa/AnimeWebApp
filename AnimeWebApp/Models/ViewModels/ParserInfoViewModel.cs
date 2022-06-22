namespace AnimeWebApp.Models.ViewModels
{
    public class ParserInfoViewModel
    {
        public int NeedToDo { get;  set; }
        public int Done { get;  set; }
        public string Description { get; set; }
        public bool IsCookiesGood { get; set; }
        public string CurrentParsingUrl { get; set; }
    }
}
