
using AnimeWebApp.Models;

namespace AnimeWebApp.Infrastructure
{
    public class FilterAnimeRoute:IRouter
    {
        private IRouter _mvcRouter;
        private List<string> AllFilters = new List<string>
        {
            nameof(FilteringData.Dubbing),
            nameof(FilteringData.Genres),
            nameof(FilteringData.Types)
        };
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
            AllFilters = AllFilters.Select(s => s.ToLower()).ToList();
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


                foreach (var filter in AllFilters)
                {
                    var filterList = _filterDataForRoute.GetDataFromContex(filter, context);
                    for (int i = 0; i < filterList.Count; i++)
                    {
                        context.RouteData.Values[$"{filter}[{i}]"] = filterList[i];
                    }
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


                foreach (var filter in AllFilters)
                {
                    var filterPath = _filterDataForRoute.GetPathFromContext(filter, context);
                    path += filterPath;
                }

                string pathSortAndNumberPage = SortAndNumberPage.GetPathFromContext(context);
                path += pathSortAndNumberPage;

                var url = query != String.Empty ? path + "?" + query : path;
                return new VirtualPathData(this, url);
            }

            return null;
        }
    }
}
