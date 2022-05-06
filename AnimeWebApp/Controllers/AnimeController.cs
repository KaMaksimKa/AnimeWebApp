using AnimeWebApp.Models;
using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AnimeWebApp.Controllers
{
    public class AnimeController : Controller
    {
        private IAnimeRepository _repository;
        public int PageSize { get; set; } = 10;
        public AnimeController(IAnimeRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index(int? page)
        {
            return View(new AnimeIndexViewModel
            {
                Anime = _repository.Anime.Skip(PageSize*((page ?? 1 )- 1)).Take(10),
                PagingInfo = new PagingInfo()
            } );
        }
        
        public IActionResult Search(string searchString)
        {
            return View("Index", new AnimeIndexViewModel
            {
                Anime = _repository.Anime.Where(a=>a.NameRu != null && a.NameRu.Contains(searchString)).Take(10),
                PagingInfo = new PagingInfo()
                
            });
        }
    }
}
