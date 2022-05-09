namespace AnimeWebApp.Models
{
    public class PagingAnimeHandlersFactory
    {
        public IPagingAnimeHandler? GetHandler(int numberPage)
        {
            return new PagingAnimeHandler {NumberPage = numberPage};
        }
        public IPagingAnimeHandler? GetHandler(int numberPage,int animePerPage)
        {
            return new PagingAnimeHandler { NumberPage = numberPage,AnimePerPage = animePerPage };
        }

    }
}
