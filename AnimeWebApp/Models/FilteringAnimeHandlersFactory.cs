namespace AnimeWebApp.Models
{
    public class FilteringAnimeHandlersFactory
    {
        public IAnimeHandler? GetHandler(string filterInUrl)
        {
            var filterList = filterInUrl.Split("-is-");

            if (filterList.Length == 2)
            {
                var typeFilter = filterList[0];
                string[] filterArgs;

                if (filterList[0].Contains("-or-"))
                {
                    filterArgs = filterList[1].Split("-or-");
                }
                else if (filterList.Contains("-and-"))
                {
                    filterArgs = filterList[1].Split("-and-");
                }
                else
                {
                    filterArgs = new[] {filterList[1]};
                }

            }
            throw new Exception("Доделать");
            return null;
        }
        public IAnimeHandler? GetHandler(List<string> listFiltersInUrl)
        {
            throw new Exception("Доделать");
        }
    }
}
