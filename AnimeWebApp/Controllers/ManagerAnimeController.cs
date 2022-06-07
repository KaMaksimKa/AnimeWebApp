using AnimeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnimeWebApp.Models.ViewModels;

namespace AnimeWebApp.Controllers
{
    [Authorize(Roles = "Admins")]
    public class ManagerAnimeController : Controller
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAnimeRepository _repository;
        public ManagerAnimeController(IAnimeRepository repository, IServiceScopeFactory serviceScopeFactory)
        {
            _repository = repository;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public IActionResult AddNewFullAnime()
        {
            var idsFromAnimego = _repository.Anime.Select(a => a.IdFromAnimeGo).ToList();
            Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetService<ApplicationContext>();
                var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                
                if (context != null)
                {
                    using var parser = new ParserAnimeGo();
                    using var writer = new WriterAnimeToDb(context);
                    var animeFromParser = await parser.GetPartialAnimeFromDefaultUrlAsync();
                    animeFromParser = animeFromParser.Where(a => !idsFromAnimego.Contains(a.IdFromAnimeGo)).ToList();
                    var step = 10;
                    var i = 0;
                    while (animeFromParser.Skip(i * step).Take(step).Any())
                    {
                        var fullAnimeFromAnimego = animeFromParser.Skip(i * step).Take(step).ToList();
                        foreach (var anime in fullAnimeFromAnimego)
                        {
                            await parser.UpdateAllDataAnime(anime);
                        }

                        var fullAnime = ConverterAnimeFromParser.ToAnime(fullAnimeFromAnimego);
                        writer.AddAnimeRange(fullAnime);
                        foreach (var anime in fullAnime)
                        {
                            if (env != null)
                            {
                                var path = Path.Combine(env.WebRootPath, "img/anime", anime.IdFromAnimeGo + ".jpg");
                                var stream = await parser.GetSteamPhotoFromAnimeHref(anime.Href);
                                if (stream != null)
                                {
                                    (new SaverPhoto()).SaveFhotoFromStream(stream, path);
                                }
                            }
                        }
                        i++;
                    }
                }
            });
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UpdatePhotosAnime()
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                using var contex = scope.ServiceProvider.GetService<ApplicationContext>();
                var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                if (contex != null && env != null)
                {
                    using var parser = new ParserAnimeGo();
                    var anime = contex.Animes.Where(a => a.IdFromAnimeGo > 2030).Select(an => new { an.IdFromAnimeGo, an.Href }).ToList();
                    foreach (var a in contex.Animes.Select(an => new { an.IdFromAnimeGo,an.Href }))
                    {
                        if (env != null)
                        {
                            if (a.IdFromAnimeGo > 2030)
                            {

                            }
                            var path = Path.Combine(env.WebRootPath, "img/anime", a.IdFromAnimeGo + ".jpg");
                            var stream = await parser.GetSteamPhotoFromAnimeHref(a.Href);
                            if (stream != null)
                            {
                                (new SaverPhoto()).SaveFhotoFromStream(stream, path);
                            }
                        }
                    }
                }
            });
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UpdateDataAllAnime()
        {
            var idsFromAnimego = _repository.Anime.Select(a => a.IdFromAnimeGo).ToList();
            Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                await using var context = scope.ServiceProvider.GetService<ApplicationContext>();
                var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                if (context != null)
                {
                    using var parser = new ParserAnimeGo();
                    using var writer = new WriterAnimeToDb(context);

                    var animeFromParser = await parser.GetFullAnimeFromDefaultUrlAsync();

                    var anime = ConverterAnimeFromParser.ToAnime(animeFromParser.Where(a => idsFromAnimego.Contains(a.IdFromAnimeGo)));
                    writer.AddOrUpDateAnimeRange(anime);
                }
            });
            return RedirectToAction(nameof(Index));
        }

        private IQueryable<IHavingTitleAndFriendlyUrl> GetAnimeAttributes(AnimeAttributes animeAttribute)
        {
            return animeAttribute switch
            {
                AnimeAttributes.Genres => _repository.Genres,
                AnimeAttributes.Dubbing => _repository.Dubbing,
                AnimeAttributes.Types => _repository.TypeAnimes,
                AnimeAttributes.Statuses => _repository.Statuses,
                _ => throw new NotImplementedException()
            };
        }
        [HttpGet]
        public IActionResult ChangeFriendlyUrl(AnimeAttributes animeAttribute)
        {
            ChangeFriendlyUrlViewModel viewModel = new ChangeFriendlyUrlViewModel();
            viewModel.AnimeAttribute = animeAttribute;
            viewModel.Attributes = GetAnimeAttributes(animeAttribute)
                .OrderBy(g => g.Title).ToDictionary(g => g.Title, g => g.FriendlyUrl);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeFriendlyUrl(ChangeFriendlyUrlViewModel viewModel)
        {
            foreach (var item in GetAnimeAttributes(viewModel.AnimeAttribute))
            {
                if (viewModel.Attributes.ContainsKey(item.Title))
                {
                    item.FriendlyUrl = viewModel.Attributes[item.Title] ?? item.FriendlyUrl;
                }
            }
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(ChangeFriendlyUrl),new { animeAttribute = viewModel.AnimeAttribute });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
