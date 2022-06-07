namespace AnimeWebApp.Models.ViewModels
{
    public class AnimeFilterViewModel
    {
        public IEnumerable<Anime> Anime { get; set; } = null!;
        public PagingInfo PagingInfo { get; set; } = null!;
        public SortingInfo SortingInfo { get; set; } = null!;
        public List<DropdownsFilerAnimeViewModel> FilteringInfo { get; set; } = new List<DropdownsFilerAnimeViewModel>();
    }
}
