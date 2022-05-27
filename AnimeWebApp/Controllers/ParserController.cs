using AnimeWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimeWebApp.Controllers
{
    public class ParserController : Controller
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAnimeRepository _repository;
        public ParserController(IAnimeRepository repository, IServiceScopeFactory serviceScopeFactory)
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
                                var path = Path.Combine(env.WebRootPath, "img/anime", anime.AnimeId + ".jpg");
                                if (!System.IO.File.Exists(path))
                                {
                                    var stream = await parser.GetSteamPhotoFromAnimeHref(anime.Href);
                                    if (stream != null)
                                    {
                                        (new SaverPhoto()).SaveFhotoFromStream(stream, path);
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
            });
            return RedirectToAction("Filter","Anime");
        }
        public IActionResult UpdateMainDataAllAnime()
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

                    var animeFromParser = await parser.GetPartialAnimeFromDefaultUrlAsync();
                    var anime = ConverterAnimeFromParser.ToAnime(animeFromParser.Where(a => idsFromAnimego.Contains(a.IdFromAnimeGo)));
                    writer.AddOrUpDateAnimeRange(anime);
                }
            });
            return RedirectToAction("Filter","Anime");
        }
        public IActionResult UpdateAnime(int idFromAnimeGo)
        {
            var href = _repository.Anime.Where(a => a.IdFromAnimeGo == idFromAnimeGo).Select(a => a.Href).First();
            Task.Run(async () =>
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    await using var context = scope.ServiceProvider.GetService<ApplicationContext>();
                    var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                    if (context != null)
                    {
                        using var parser = new ParserAnimeGo();
                        using var writer = new WriterAnimeToDb(context);

                        var animeFromParser = await parser.GetAllDataAnime(href, idFromAnimeGo);
                        var anime = ConverterAnimeFromParser.ToAnime(animeFromParser);
                        writer.AddOrUpDateAnime(anime);
                        if (env != null)
                        {
                            var path = Path.Combine(env.WebRootPath, "img/anime", idFromAnimeGo + ".jpg");
                            if (!System.IO.File.Exists(path))
                            {
                                var stream = await parser.GetSteamPhotoFromAnimeHref(href);
                                if (stream != null)
                                {
                                    (new SaverPhoto()).SaveFhotoFromStream(stream, path);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });


            return RedirectToAction("Filter","Anime");
        }
    }
}
