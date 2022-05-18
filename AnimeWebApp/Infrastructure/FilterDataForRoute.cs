namespace AnimeWebApp.Infrastructure
{
    public class FilterDataForRoute
    {
        public string GetPath(string key, List<string> values)
        {
            if (values.Count!=0)
            {
                return "/"+key + "-is-" + String.Join("-or-", values);
            }
            else
            {
                return String.Empty;
            }
        }
        public string GetPathFromContext(string key,VirtualPathContext context)
        {
            if (context.Values.ContainsKey($"{key}[0]"))
            {
                List<string> values = new List<string>();
                int i = 0;
                while (context.Values[$"{key}[{i}]"] is string value)
                {
                    values.Add(value);
                    i++;
                }
                return GetPath(key, values);
            }
            else if(context.HttpContext.Request.Query.ContainsKey(key))
            {
                return GetPath(key, context.HttpContext.Request.Query[key].ToList());
            }
            else
            {
                return String.Empty;
            }
        }

        public List<string> GetDataFromContex(string key, RouteContext context)
        {
            var pathList = context.HttpContext.Request.Path.Value?.Split("/");
            foreach (var path in pathList)
            {
                if (path.Contains(key + "-is-"))
                {
                    return path.Replace(key + "-is-", "").Split("-or-").ToList();
                }
            }
            return new List<string>();
        }
    }
}
