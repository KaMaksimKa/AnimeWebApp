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
                    FilteringInfo = new List<DropdownsFilerAnimeViewModel>
                    {
                        new DropdownsFilerAnimeViewModel(filteringData.Genres)
                        {
                            Title = "Жанры",
                            FriendlyUrl = nameof(filteringData.Genres),
                            AllowedItems = _repository.Genres.Where(g => g.FriendlyUrl != String.Empty)
                            .OrderBy(g => g.Title).OfType<IHavingTitleAndFriendlyUrl>().ToList()
                        },
                         new DropdownsFilerAnimeViewModel(filteringData.Dubbing)
                        {
                            Title = "Озвучка",
                            FriendlyUrl = nameof(filteringData.Dubbing),
                            AllowedItems = _repository.Dubbing.Where(d => d.FriendlyUrl != String.Empty)
                            .OrderBy(d => d.Title).OfType<IHavingTitleAndFriendlyUrl>().ToList()
                        },
                         new DropdownsFilerAnimeViewModel(filteringData.Statuses)
                        {
                            Title = "Статус",
                            FriendlyUrl = nameof(filteringData.Statuses),
                            AllowedItems = _repository.Statuses.Where(s => s.FriendlyUrl != String.Empty)
                            .OrderBy(s => s.Title).OfType<IHavingTitleAndFriendlyUrl>().ToList()
                        },
                         new DropdownsFilerAnimeViewModel(filteringData.Types)
                        {
                            Title = "Тип",
                            FriendlyUrl = nameof(filteringData.Types),
                            AllowedItems = _repository.TypeAnimes.Where(t => t.FriendlyUrl != String.Empty)
                            .OrderBy(t => t.Title).OfType<IHavingTitleAndFriendlyUrl>().ToList()
                        },

                    }
                }); ;
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
