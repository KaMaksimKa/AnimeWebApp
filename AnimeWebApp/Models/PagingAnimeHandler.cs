namespace AnimeWebApp.Models
{
    public class PagingAnimeHandler:IAnimeHandler
    {
        public int PageSize { get; set; }
        public int NumberPage { get; set; }

        public PagingAnimeHandler(int numberPage,int pageSize)
        {
            PageSize = pageSize;
            NumberPage = numberPage;
        }
        public IAnimeHandler? Next { get; set; }

        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            if (NumberPage > 0)
            {
                return animes?.Skip(PageSize * (NumberPage - 1)).Take(PageSize);
            }
            else
            {
                return null;
            }
        }
    }
}
