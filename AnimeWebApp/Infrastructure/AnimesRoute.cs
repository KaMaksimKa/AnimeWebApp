using AnimeWebApp.Models;

namespace AnimeWebApp.Infrastructure
{
    public class AnimesRoute:IRouter
    {
        private IRouter _mvcRouter;
        public string DefaultPage { get; set; } = "1";
        private PagingAnimeHandlersFactory _pagingFactory;

        public string DefaultSort { get; set; } = "date-add";
        private SortingAnimeHandlersFactory _sortingFactory;
        public AnimesRoute(IRouter mvcRouter)
        {
            _mvcRouter = mvcRouter;
            _pagingFactory = new PagingAnimeHandlersFactory();
            _sortingFactory = new SortingAnimeHandlersFactory();
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

                context.RouteData.Values["pagingAnimeHandler"] = _pagingFactory.GetHandler(pathPage);


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

                context.RouteData.Values["sortingAnimeHandler"] = _sortingFactory.GetHandler(pathSort);


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

                var sortingAnimeHalder = context.Values["sortingAnimeHandler"] as ISortingAnimeHandler;
                url += sortingAnimeHalder?.Path != $"{DefaultSort}" ? $"/sort/{sortingAnimeHalder?.Path}" : "";

                var pagingAnimeHalder = context.Values["pagingAnimeHandler"] as IPagingAnimeHandler;
                url += pagingAnimeHalder?.Path != $"{DefaultPage}"? $"/page/{pagingAnimeHalder?.Path}":"";

                return new VirtualPathData(this, url);
            }

            return null;
        }
    }
}
