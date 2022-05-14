
namespace AnimeWebApp.Infrastructure
{
    public class FilterAnimeRoute:IRouter
    {
        private IRouter _mvcRouter;
        public string DefaultFilter { get; set; } = "all";
        public string Controller { get; set; } = "Anime";
        public string Action { get; set; }
        private string? _urlStartWith;
        public SortAndNumberPage SortAndNumberPage { get; set; } = new SortAndNumberPage();
        public string UrlStartWith
        {
            get => _urlStartWith ?? $"{Controller}/{Action}".ToLower();
            set => _urlStartWith = value;
        }

        public FilterAnimeRoute(IRouter mvcRouter)
        {
            _mvcRouter = mvcRouter;
        }
        public async Task RouteAsync(RouteContext context)
        {

            var path = context.HttpContext.Request.Path.Value?.Trim('/').ToLower();

            if (path?.StartsWith(UrlStartWith)==true ||
                (context.RouteData.Values["controller"]?.ToString() == Controller && context.RouteData.Values["action"]?.ToString() == Action))
            {


                path = SortAndNumberPage.GetSortAndPage(path, out string? pathSort, out string? pathPage);

                context.RouteData.Values["numberPage"] = pathPage;
                context.RouteData.Values["sort"] = pathSort;


                /*pathList = path?.Split("filters").Select(s => s.Trim('/')).ToList();
                string pathFilter;
                if (pathList?.Count() == 1)
                {
                    pathFilter = DefaultFilter;
                }
                else if (pathList?.Count() == 2)
                {
                    pathFilter = pathList[1];
                    path = pathList[0];
                }
                else
                {
                    return;
                }
                var filters = pathFilter.Split("/");
                for (int i = 0; i < filters.Length ; i ++)
                {
                    context.RouteData.Values[$"filters[{i}]"] = filters[i];
                }*/

                context.RouteData.Routers.Add(_mvcRouter);
                context.RouteData.Values["controller"] = Controller;
                context.RouteData.Values["action"] = Action;


                await _mvcRouter.RouteAsync(context);
            }

        }

        public VirtualPathData? GetVirtualPath(VirtualPathContext context)
        {
            if (context.Values["controller"]?.ToString() == Controller &&
                context.Values["action"]?.ToString() == Action)
            {
                string path = $"/{UrlStartWith}";
                string query = String.Empty;
                string? valueSort = null;
                string? valueNumberPage = null;

                foreach (var (key,value) in context.Values)
                {
                    if (key == "numberPage")
                    {
                        valueNumberPage = value?.ToString();
                    }
                    else if (key == "sort")
                    {
                        valueSort = value?.ToString();
                    }
                    else
                    {
                        if (key != "controller" && key != "action" && key != "area" && key != "page")
                        {
                            if (query != String.Empty)
                            {
                                query += "&";
                            }
                            query += $"{key}={value?.ToString()?.Replace(" ", "+")}";
                        }
                    }
                    
                }

                string pathSortAndNumberPage = SortAndNumberPage.GetPath(valueSort, valueNumberPage);
                path += pathSortAndNumberPage;


                /*var filters = new List<object>();
                var i = 0;
                while (context.Values[$"filters[{i}]"] is {} value)
                {
                    filters.Add(value);
                    i++;
                }

                var pathFilters = string.Join('/', filters);
                url += pathFilters != $"{DefaultFilter}" && pathFilters!=String.Empty ? $"/filters/{pathFilters}" : "";
*/

                var url = query != String.Empty ? path + "?" + query : path;
                return new VirtualPathData(this, url);
            }

            return null;
        }
    }
}
