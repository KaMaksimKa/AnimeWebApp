
namespace AnimeWebApp.Infrastructure
{
    public class FilterAnimeRoute:IRouter
    {
        private IRouter _mvcRouter;
        public string DefaultFilter { get; set; } = "all";
        public string Controller { get; set; } = "Anime";
        public string Action { get; set; }
        private string? _urlStartWith;
        public SortAndNumberPageForRoute SortAndNumberPage { get; set; } = new SortAndNumberPageForRoute();
        private readonly FilterDataForRoute _filterDataForRoute = new FilterDataForRoute();
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

                var genres = _filterDataForRoute.GetDataFromContex("genres", context);
                for (int i = 0; i < genres.Count; i++)
                {
                    context.RouteData.Values[$"genres[{i}]"] = genres[i];
                }
                var dubbing = _filterDataForRoute.GetDataFromContex("dubbing", context);
                for (int i = 0; i < dubbing.Count; i++)
                {
                    context.RouteData.Values[$"dubbing[{i}]"] = dubbing[i];
                }
                var types = _filterDataForRoute.GetDataFromContex("types", context);
                for (int i = 0; i < types.Count; i++)
                {
                    context.RouteData.Values[$"types[{i}]"] = types[i];
                }

                context.RouteData.Routers.Add(_mvcRouter);
                context.RouteData.Values["controller"] = Controller;
                context.RouteData.Values["action"] = Action;

                var p = GetVirtualPath(new VirtualPathContext(context.HttpContext, context.RouteData.Values, context.RouteData.Values));

                if (context.HttpContext.Request.Path.Value == p?.VirtualPath)
                {
                    await _mvcRouter.RouteAsync(context);
                }
                else
                {
                    context.Handler = async c => c.Response.Redirect(p.VirtualPath);
                }



                
            }
            
        }

        public VirtualPathData? GetVirtualPath(VirtualPathContext context)
        {
            if (context.Values["controller"]?.ToString() == Controller &&
                context.Values["action"]?.ToString() == Action)
            {
                string path = $"/{UrlStartWith}";
                string query = String.Empty;
                var pathGenres = _filterDataForRoute.GetPathFromContext("genres", context);
                path += pathGenres;
                var pathDubbing = _filterDataForRoute.GetPathFromContext("dubbing", context);
                path += pathDubbing;
                var pathTypes = _filterDataForRoute.GetPathFromContext("types", context);
                path += pathTypes;

                string pathSortAndNumberPage = SortAndNumberPage.GetPathFromContext(context);
                path += pathSortAndNumberPage;

                var url = query != String.Empty ? path + "?" + query : path;
                return new VirtualPathData(this, url);
            }

            return null;
        }
    }
}
