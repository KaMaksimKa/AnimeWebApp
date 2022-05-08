using AnimeWebApp.Models;
using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AnimeWebApp.Controllers
{
    public class AnimeController : Controller
    {
        private readonly IAnimeRepository _repository;
        public int PageSize { get; set; } = 10;
        public AnimeController(IAnimeRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            

            int totalPage = (int)Math.Ceiling((decimal)_repository.Anime.Count() / PageSize);

            var pagingAnimeHandler = ControllerContext.RouteData.Values["pagingAnimeHandler"] as IPagingAnimeHandler;
            if (pagingAnimeHandler != null)
            {
                pagingAnimeHandler.AnimePerPage = PageSize;
            }

            var soringAnimeHandler = ControllerContext.RouteData.Values["sortingAnimeHandler"] as ISortingAnimeHandler;
            if (soringAnimeHandler != null)
            {
                soringAnimeHandler.Next = pagingAnimeHandler;
            }
            


            if (soringAnimeHandler?.Invoke(_repository.Anime, _repository.Anime.Select(a => a.AnimeId)) is { } anime)
            {
                return View(new AnimeIndexViewModel
                {
                    Anime = anime,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = pagingAnimeHandler?.NumberPage??-1,
                        TotalPages = totalPage
                    }
                });
            }
            
            
            return NotFound();

        }


        [Route("[controller]/[action]/{search}/page/{numberPage}")]
        [Route("[controller]/[action]/{search}")]
        public IActionResult Search(string search)
        {
            return View("Index", new AnimeIndexViewModel
            {
                Anime = _repository.Anime.Where(a=>a.NameRu == null || a.NameRu.Contains(search)).Take(10),
                PagingInfo = new PagingInfo()
                
            });
        }
        [HttpPost]
        public RedirectToActionResult GetSearch(string search)
        {
            return RedirectToAction("Search", new {search = search, numberPage =2});
        }
    }
}
