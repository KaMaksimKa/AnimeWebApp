using AnimeWebApp.Models;
using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AnimeWebApp.Controllers
{
    public class AnimeController : Controller
    {
        private readonly IAnimeRepository _repository;
        private readonly PagingAnimeHandlersFactory _pagerFactory;
        private readonly SortingAnimeHandlersFactory _sorterFactory;
        public int PageSize { get; set; } = 10;
        public AnimeController(IAnimeRepository repository)
        {
            _repository = repository;
            _pagerFactory = new PagingAnimeHandlersFactory();
            _sorterFactory = new SortingAnimeHandlersFactory();
        }
        public IActionResult Index(int numberPage, string sort,List<string> filters)
        {

            var pagingAnimeHandler = _pagerFactory.GetHandler(numberPage,PageSize);
            var soringAnimeHandler = _sorterFactory.GetHandler(sort);
            if ( pagingAnimeHandler is {} && soringAnimeHandler is {})
            {

                var totalPagesAnimeHandler = new GetingTotalPagesAnimeHandler{Next = pagingAnimeHandler,PageSize = PageSize};
                soringAnimeHandler.Next = totalPagesAnimeHandler;

                if (soringAnimeHandler?.Invoke(_repository.Anime, _repository.Anime.Select(a => a.AnimeId)) is { } anime)
                {
                    return View(new AnimeIndexViewModel
                    {
                        Anime = anime,
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = numberPage,
                            PageSize = PageSize,
                            TotalPages = totalPagesAnimeHandler.TotalPages
                        },
                        SortingInfo = new SortingInfo
                        {
                            CurrentSort = sort,
                            AllSorts = new Dictionary<string, string>
                            {
                                ["date-add-desc"] = "Дате добавления",
                                ["rating-desc"] = "Рейтингу",
                                ["completed-desc"] = "Уже посмотрели",
                                ["watching-desc"] = "Смотрят сейчас"
                            }
                        }
                    });
                }
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
