namespace AnimeWebApp.Infrastructure
{
    public class SearchAnimeRoute : IRouter
    {
        private IRouter _mvcRouter;
        public string Controller { get; set; }
        public string Action { get; set; }
        private string? _urlStartWith;
        public SortAndNumberPageForRoute SortAndNumberPage { get; set; } = new SortAndNumberPageForRoute();
        public string UrlStartWith
        {
            get => _urlStartWith ?? $"{Controller}/{Action}".ToLower();
            set => _urlStartWith = value;
        }

        public SearchAnimeRoute(IRouter mvcRouter)
        {
            _mvcRouter = mvcRouter;
        }
        public async Task RouteAsync(RouteContext context)
        {

            var path = context.HttpContext.Request.Path.Value?.Trim('/').ToLower();

            if (path?.StartsWith(UrlStartWith) == true ||
                (context.RouteData.Values["controller"]?.ToString() == Controller && context.RouteData.Values["action"]?.ToString() == Action))
            {

                path = SortAndNumberPage.GetSortAndPage(path, out string? pathSort, out string? pathPage);

                context.RouteData.Values["numberPage"] = pathPage;
                context.RouteData.Values["sort"] = pathSort;

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
                string? query = "search="+context.Values["search"]?.ToString()?.Replace(" ","+");

                string pathSortAndNumberPage = SortAndNumberPage.GetPathFromContext(context);
                path += pathSortAndNumberPage;

                var url = query != null ? path + "?" + query : path;
                return new VirtualPathData(this, url);
            }

            return null;
        }
    }
}
