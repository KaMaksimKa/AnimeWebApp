using Microsoft.AspNetCore.Mvc;

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
