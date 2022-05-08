namespace AnimeWebApp.Models
{
    public class SortingAnimeHandlersFactory
    {
        private ISortingAnimeHandler? GetBaseSortingAnime(string nameInUrl)
        {
            return nameInUrl switch
            {
                "rating" => new SortingAnimeByRatingHandler(),
                "date-add" => new SortingAnimeByDateAddHandler(),
                "completed" => new SortingAnimeByCompletedHandler(),
                "watching" => new SortingAnimeByWatchingHandler(),
                _ => null

            };
        }
        public ISortingAnimeHandler? GetHandler(string nameInUrl)
        {
            nameInUrl = nameInUrl.ToLower();
            if (nameInUrl.Contains("-desc"))
            {
                nameInUrl = nameInUrl.Replace("-desc", String.Empty);
                if (GetBaseSortingAnime(nameInUrl) is { } sortingAnime)
                {
                    return new SortingAnimeDescDecoratorHandler(sortingAnime);
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return GetBaseSortingAnime(nameInUrl);
            }
        }
    }
}
