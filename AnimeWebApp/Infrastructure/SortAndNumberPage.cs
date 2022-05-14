namespace AnimeWebApp.Infrastructure
{
    public class SortAndNumberPage
    {

        public string DefaultNumberPage { get; set; } = "1";
        public string DefaultSort { get; set; } = "date-add-desc";

        public string GetPath(string? sort, string? numberPage)
        {
            var path = "";
            if (sort is { } && sort != DefaultSort)
            {
                path += $"/sort/{sort}";
            }

            if (numberPage is { } && numberPage != DefaultNumberPage)
            {
                path += $"/page/{numberPage}";
            }

            return path;
        }

        public string? GetSortAndPage(string? path, out string? sort, out string? numberPage)
        {
            path = path?.Trim('/').ToLower();



            var pathList = path?.Split("page").Select(s => s.Trim('/')).ToList();
            string? pathPage;
            if (pathList?.Count() == 1)
            {
                numberPage = DefaultNumberPage;
            }
            else if (pathList?.Count() == 2)
            {
                numberPage = pathList[1];
                path = pathList[0];
            }
            else
            {
                numberPage = null;
                path = pathList?[0];
            }

            pathList = path?.Split("sort").Select(s => s.Trim('/')).ToList();
            if (pathList?.Count() == 1)
            {
                sort = DefaultSort;
            }
            else if (pathList?.Count() == 2)
            {
                sort = pathList[1];
                path = pathList[0];
            }
            else
            {
                sort = null;
                path = pathList?[0];
            }

            return path;
        }
    }
}
