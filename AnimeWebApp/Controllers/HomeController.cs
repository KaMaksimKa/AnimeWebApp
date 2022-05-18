using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AnimeWebApp.Components
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
