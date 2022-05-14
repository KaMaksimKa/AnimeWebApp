using Microsoft.AspNetCore.Mvc.Filters;

namespace AnimeWebApp.Models
{
    public class MyActionFilterAttribute: ActionFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await next.Invoke();
        }
    }
}
