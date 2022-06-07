namespace AnimeWebApp.Models.ViewModels
{
    public class DropdownsFilerAnimeViewModel
    {
        public string Title { get; set; }
        public string FriendlyUrl { get; set; }
        private readonly List<string> _titlesItems;
        public List<IHavingTitleAndFriendlyUrl> SelectedItems =>
            AllowedItems.Where(i => _titlesItems.Contains(i.FriendlyUrl)).ToList();
        public List<IHavingTitleAndFriendlyUrl> AllowedItems { get; set; } = new List<IHavingTitleAndFriendlyUrl>();
        public DropdownsFilerAnimeViewModel(List<string> titlesItems)
        {
            _titlesItems = titlesItems;
        }
    }
}
