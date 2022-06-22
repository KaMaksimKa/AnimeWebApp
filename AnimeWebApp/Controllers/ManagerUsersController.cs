using AnimeWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnimeWebApp.Controllers
{
    public class ManagerUsersController : Controller
    {
        private UserManager<AppUser> _userManager;
        public ManagerUsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
