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

        [Route("api/animes")]
        public List<Anime>? GetAnimes(int? numberPage, string? sort, List<string>? filters)
        {
            var pagingAnimeHandler = new PagingAnimeHandler(numberPage??1, PageSize);
            var sortingAnimeHandler = new SortingAnimeHandler(sort??"date-add-desc");
            var filteringAnimeHandler =
                new FilteringAnimeByGenres
                {
                    IncludeData = new List<string> { "Экшен" },
                    ExcludeData = new List<string> { "Фэнтези" },
                };
            var combiningAnimeHandler = new CombiningAnimeHandler(new List<IAnimeHandler>
            {
                sortingAnimeHandler,
                pagingAnimeHandler
            });
            return combiningAnimeHandler.Invoke(_repository.Anime)?.ToList();
        }
        public IActionResult Filter(int numberPage, string sort,List<string> filters)
        {

            var pagingAnimeHandler = new PagingAnimeHandler(numberPage,PageSize);
            var sortingAnimeHandler = new SortingAnimeHandler(sort);
            var countingAnimeHandler = new CountingAnimeHandler();
            var filteringAnimeHandler =
                new FilteringAnimeByGenres
                {
                    IncludeData = new List<string>{"Экшен"},
                    ExcludeData = new List<string>{ "Фэнтези" },
                };

            var combiningAnimeHandler = new CombiningAnimeHandler(new List<IAnimeHandler>
            {
                countingAnimeHandler,
                sortingAnimeHandler,
                pagingAnimeHandler
            });
            
            if (combiningAnimeHandler.Invoke(_repository.Anime)?.ToList() is { } anime)
            {
                var totalPages = (int)Math.Ceiling((decimal)countingAnimeHandler.NumberOfAnime / PageSize) ;
                return View(new AnimeIndexViewModel
                {
                    Anime = anime,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = numberPage,
                        PageSize = PageSize,
                        TotalPages = totalPages,
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

            return NotFound();
        }


        public IActionResult Search(string search,int numberPage,string sort)
        {
            var pagingAnimeHandler = new PagingAnimeHandler(numberPage, PageSize);
            var sortingAnimeHandler = new SortingAnimeHandler(sort);
            var countingAnimeHandler = new CountingAnimeHandler();
            var searchingAnimeHandler = new SearchingAnimeHandler(search);

            var combiningAnimeHandler = new CombiningAnimeHandler(new List<IAnimeHandler>
            {
                searchingAnimeHandler,
                countingAnimeHandler,
                sortingAnimeHandler,
                pagingAnimeHandler
            });
            if (combiningAnimeHandler.Invoke(_repository.Anime)?.ToList() is { } anime)
            {
                var totalPages = (int)Math.Ceiling((decimal)countingAnimeHandler.NumberOfAnime / PageSize);
                return View("Filter", new AnimeIndexViewModel
                {
                    Anime = anime,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = numberPage,
                        PageSize = PageSize,
                        TotalPages = totalPages,
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

            return NotFound();
        }
        
    }
}
