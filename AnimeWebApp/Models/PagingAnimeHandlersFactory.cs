namespace AnimeWebApp.Models
{
    public class PagingAnimeHandlersFactory
    {
        public IPagingAnimeHandler? GetHandler(string pathHandler)
        {
            if (int.TryParse(pathHandler, out int numberPage))
            {
                return new PagingAnimeHandler { NumberPage = numberPage};
            }
            else
            {
                return null;
            }
        }
    }
}
