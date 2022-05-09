using AnimeWebApp.Models;

namespace AnimeWebApp.Infrastructure
{
    public class AnimesRoute:IRouter
    {
        private IRouter _mvcRouter;
        public string DefaultPage { get; set; } = "1";
        public string DefaultSort { get; set; } = "date-add";
        public string DefaultFilter { get; set; } = "all";


        public AnimesRoute(IRouter mvcRouter)
        {
            _mvcRouter = mvcRouter;
        }
        public async Task RouteAsync(RouteContext context)
        {

            var path = context.HttpContext.Request.Path.Value?.Trim('/').ToLower();

            if (path?.StartsWith("anime")==true || path ==String.Empty)
            {
                List<IAnimeHandler?> animeHandlers = new List<IAnimeHandler?>();
                
                var pathList = path?.Split("page").Select(s=>s.Trim('/')).ToList();
                string pathPage;
                if (pathList?.Count() == 1)
                {
                    pathPage = DefaultPage;
                }
                else if (pathList?.Count() == 2)
                {
                    pathPage = pathList[1];
                    path = pathList[0];
                }
                else
                {
                    return;
                }
                context.RouteData.Values["numberPage"] = pathPage;


                pathList = path?.Split("sort").Select(s => s.Trim('/')).ToList();
                string pathSort;
                if (pathList?.Count() == 1)
                {
                    pathSort = DefaultSort;
                }
                else if (pathList?.Count() == 2)
                {
                    pathSort = pathList[1];
                    path = pathList[0];
                }
                else
                {
                    return;
                }
                context.RouteData.Values["sort"] = pathSort;


                pathList = path?.Split("filters").Select(s => s.Trim('/')).ToList();
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
                }

                context.RouteData.Routers.Add(_mvcRouter);
                context.RouteData.Values["controller"] = "Anime";
                context.RouteData.Values["action"] = "Index";


                await _mvcRouter.RouteAsync(context);
            }

        }

        public VirtualPathData? GetVirtualPath(VirtualPathContext context)
        {
            if (context.Values["controller"]?.ToString() == "Anime")
            {
                string url = $"/{context.Values["controller"]}";

                var filters = new List<object>();
                var i = 1;
                while (context.Values[$"filters[{i}]"] is {} value)
                {
                    filters.Add(value);
                    i++;
                }

                var pathFilters = string.Join('/', filters);
                url += pathFilters != $"{DefaultFilter}" ? $"/filters/{pathFilters}" : "";


                var sort = context.Values["sort"];
                url += sort?.ToString() != $"{DefaultSort}" ? $"/sort/{sort}" : "";

                var numberPage = context.Values["numberPage"];
                url += numberPage?.ToString() != $"{DefaultPage}"? $"/page/{numberPage}":"";

                return new VirtualPathData(this, url);
            }

            return null;
        }
    }
}
