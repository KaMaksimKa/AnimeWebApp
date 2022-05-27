using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AnimeWebApp.Components
{
    public class HeaderViewComponent:ViewComponent
    {
        [Authorize]
        public ViewViewComponentResult Invoke()
        {
            return View(new HeaderViewModel(){UserIsIsAuthenticated = User.Identity?.IsAuthenticated });
        }
    }
}
