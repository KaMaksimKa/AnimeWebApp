using AnimeWebApp.Models;
using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        /*[Route("api/animes")]
        public List<Anime>? GetAnimes(int? numberPage, string? sort)
        {
            var pagingAnimeHandler = new PagingAnimeHandler(numberPage??1, PageSize);
            var sortingAnimeHandler = new SortingAnimeHandler(sort??"date-add-desc");
            var combiningAnimeHandler = new CombiningAnimeHandler(new List<IAnimeHandler>
            {
                sortingAnimeHandler,
                pagingAnimeHandler
            });
            return combiningAnimeHandler.Invoke(_repository.Anime)?.ToList();
        }*/
        public IActionResult Filter(int numberPage, string sort,FilteringData filteringData)
        {
            var paging = new PagingAnimeHandler(numberPage,PageSize);
            var sorting = new SortingAnimeHandler(sort);
            var counting = new CountingAnimeHandler();
            var filtering = new FilteringAnimeHandler(filteringData);
            var combining = new CombiningAnimeHandler(new List<IAnimeHandler>
            {
                filtering,
                counting,
                sorting,
                paging
            });
            
            if (combining.Invoke(_repository.Anime)?.ToList() is { } anime)
            {
                var totalPages = (int)Math.Ceiling((decimal)counting.NumberOfAnime / PageSize) ;
                return View(new AnimeFilterViewModel
                {
                    Anime = anime,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = numberPage,
                        PageSize = PageSize,
                        TotalPages = totalPages,
                    },
                    SortingInfo = new SortingInfo(sort),
                    FilteringInfo = new FilteringInfo(filteringData)
                    {
                        AllGenres = _repository.Genres.Where(g=>g.FriendlyUrl!=String.Empty).OrderBy(g=>g.Title).ToList(),
                        AllDubbing = _repository.Dubbing.Where(d => d.FriendlyUrl != String.Empty).OrderByDescending(d=>d.Animes.Count).ToList(),
                        AllTypes = _repository.TypeAnimes.Where(t => t.FriendlyUrl != String.Empty).OrderByDescending(t => t.Title).ToList(),
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
                return View("Search", new AnimeFilterViewModel
                {
                    Anime = anime,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = numberPage,
                        PageSize = PageSize,
                        TotalPages = totalPages,
                    },
                    SortingInfo = new SortingInfo(sort)
                });
            }

            return NotFound();
        }
        
    }
}
