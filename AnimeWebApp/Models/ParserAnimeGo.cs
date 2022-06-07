using System.Net;
using System.Net.Http.Headers;
using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json.Linq;

namespace AnimeWebApp.Models
{
    public class ParserAnimeGo: IDisposable
    {
        public string DefaultUrl { get; set; } = "https://animego.org/anime";
        private readonly IBrowsingContext _context;
        private HttpClient _clientJson;
        private HttpClient _clientImg;
        public ParserAnimeGo()
        {
            _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            _clientImg = new HttpClient();
            _clientJson = new HttpClient();
            _clientJson.DefaultRequestHeaders.Accept.Clear();
            _clientJson.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _clientJson.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/javascript"));
            _clientJson.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(@"*/*"));

            _clientJson.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru"));
            _clientJson.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
            _clientJson.DefaultRequestHeaders.Add("User-Agent",
                @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.141 YaBrowser/22.3.3.852 Yowser/2.5 Safari/537.36");
            _clientJson.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
        }
        public async Task<List<AnimeFromParser>> GetFullAnimeFromDefaultUrlAsync()
        {
            return await GetFullAnimeFromUrlAsync(DefaultUrl);
        }
        public async Task<List<AnimeFromParser>> GetFullAnimeFromUrlAsync(string url)
        {
            var animeList =await GetPartialAnimeFromUrlAsync(url);
            int i = 0;
            foreach (var anime in animeList)
            {
                await UpdateAllDataAnime(anime);
            }
            return animeList;
        }
        public async Task<List<AnimeFromParser>> GetPartialAnimeFromDefaultUrlAsync()
        {
            return await GetPartialAnimeFromUrlAsync(DefaultUrl);
        }
        public async Task<List<AnimeFromParser>> GetPartialAnimeFromUrlAsync(string url)
        {
            if (!url.Contains("?"))
            {
                url += "?";
            }
            List<AnimeFromParser> animeList = new List<AnimeFromParser>();
            IDocument? page = null;

            int numberPage = 1;
            do
            {
                try
                {
                    string newUrl = url + $"&page={numberPage}";
                    page = await GetDocumentFromHtmlAsync(newUrl);

                    foreach (var e in page.QuerySelectorAll(".animes-list-item"))
                    {
                        string? href = e.QuerySelector(".h5")?.QuerySelector("a")?.GetAttribute("href")?.Trim();
                        int.TryParse(href?.Split("-")[^1], out int idFromAnimeGoResult);
                        int? idFromAnimeGo = idFromAnimeGoResult == 0 ? null : idFromAnimeGoResult;
                        var nameRu = e.QuerySelector(".h5")?.Text().Trim();
                        var nameEn = e.QuerySelector(".text-gray-dark-6 ")?.Text().Trim();
                        var type = e.QuerySelector("span")?.QuerySelector("a")?.Text().Trim();
                        int.TryParse(e.QuerySelector(".anime-year")?.QuerySelector("a")?.Text().Trim(), out int yearResult);
                        int? year = yearResult == 0 ? null : yearResult;
                        var description = e.QuerySelector(".description")?.Text().Trim();
                        animeList.Add(new AnimeFromParser
                        {
                            Href = href,
                            IdFromAnimeGo = idFromAnimeGo,
                            TitleEn = nameEn,
                            TitleRu = nameRu,
                            Description = description,
                            Type = type,
                            Year = year,
                        });
                    }
                }
                finally
                {
                    page?.Close();
                }
                numberPage++;
            } while (page.StatusCode != HttpStatusCode.NotFound);

            return animeList;
        }
        public async Task<AnimeFromParser> GetMainDataAnimeAsync(string? hrefAnime, int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() { Href = hrefAnime, IdFromAnimeGo = idFromAnimeGo };
            await UpdateMainDataAnimeAsync(anime); 
            return anime;
        }
        public async Task<AnimeFromParser> GetShowDataAnimeAsync(int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() {IdFromAnimeGo = idFromAnimeGo };
            await UpdateShowDataAnimeAsync(anime);
            return anime;
        }
        public async Task<AnimeFromParser> GetVoiceoverDataAnimeFromFirstEpisodeAsync(int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() { IdFromAnimeGo = idFromAnimeGo };
            await UpdateVoiceoverDataAnimeFromFirstEpisodeAsync(anime);
            return anime;
        }
        public async Task<AnimeFromParser> GetAllDataAnime(string? hrefAnime, int idFromAnimeGo)
        {
            var anime = new AnimeFromParser() { Href = hrefAnime, IdFromAnimeGo = idFromAnimeGo };
            await UpdateAllDataAnime(anime);
            return anime;
        }

        public async Task UpdateMainDataAnimeAsync(AnimeFromParser anime)
        {
            if (anime.Href == null)
            {
                return;
            }
            
            using var page =await GetDocumentFromHtmlAsync(anime.Href);
            if (page.StatusCode == HttpStatusCode.OK)
            {
                double.TryParse(page.QuerySelector(".rating-value")?.Text().Trim(), out double rateResult);
                double? rate = rateResult == 0 ? null : rateResult;

                Dictionary<string, string?> dictionary = new Dictionary<string, string?>();
                if (page.QuerySelector(".anime-info")?.QuerySelectorAll("dt") is {} elements)
                {
                    foreach (var e in elements)
                    {
                        dictionary.Add(e.Text().Trim(), e.NextElementSibling?.Text().Trim());
                    }
                }
               

                dictionary.TryGetValue("Эпизоды", out string? countEpisodeValue);
                int.TryParse(countEpisodeValue?.Split("/")[^1], out int countEpisodeResult);
                int? countEpisode = countEpisodeResult == 0 ? null : countEpisodeResult;
                
                dictionary.TryGetValue("Статус", out string? status);

                dictionary.TryGetValue("Жанр", out string? genresValue);
                List<string> genres = genresValue?.Split(",").Select(g => g.Trim()).ToList() ??
                                     new List<string>();

                dictionary.TryGetValue("Рейтинг MPAA", out string? mpaaRate);
                dictionary.TryGetValue("Студия", out string? studioValue);
                List<string> studios = studioValue?.Split(",").Select(s => s.Trim() ).ToList() ??
                                       new List<string>();
                dictionary.TryGetValue("Длительность", out string? duration);
                dictionary.TryGetValue("Следующий эпизод", out string? nextEpisode);

                dictionary.TryGetValue("Озвучка ", out string? voiceoverValue);
                List<string> voiceovers = voiceoverValue?.Split(",").Select(v => v.Trim()).ToList() ??
                                          new List<string>();

                anime.Rate = rate;
                anime.CountEpisode = countEpisode;
                anime.Status = status;
                anime.Genres = genres;
                anime.MpaaRate = mpaaRate;
                anime.Studios = studios;
                anime.Duration = duration;
                anime.NextEpisode = nextEpisode;
                anime.Dubbing = anime.Dubbing.Union(voiceovers).ToList();

            }
            
        }
        public async Task UpdateShowDataAnimeAsync(AnimeFromParser anime)
        {
            if (anime.IdFromAnimeGo == null)
            {
                return;
            }
            Dictionary<string, string?> dictionary = new Dictionary<string, string?>();

            using var doc = await GetDocumentFromJsonAsync($"https://animego.org/animelist/{anime.IdFromAnimeGo}/show");
            if (doc.StatusCode == HttpStatusCode.OK)
            {
                foreach (var e in doc.QuerySelectorAll("tr").Skip(1))
                {
                    dictionary.Add(e.QuerySelectorAll("td")[2].Text().Trim(), e.QuerySelectorAll("td")[0].Text().Trim());
                }
            }
            

            dictionary.TryGetValue("Смотрю", out string? watchingValue);
            int.TryParse(watchingValue, out int watchingResult);
            int? watching = watchingResult == 0 ? null : watchingResult;

            dictionary.TryGetValue("Просмотрено", out string? completedValue);
            int.TryParse(completedValue, out int completedResult);
            int? completed = completedResult == 0 ? null : completedResult;

            dictionary.TryGetValue("Брошено", out string? droppedValue);
            int.TryParse(droppedValue, out int droppedResult);
            int? dropped = droppedResult == 0 ? null : droppedResult;

            dictionary.TryGetValue("Отложено", out string? onHoldValue);
            int.TryParse(onHoldValue, out int onHoldResult);
            int? onHold = onHoldResult == 0 ? null : onHoldResult;

            dictionary.TryGetValue("Запланировано", out string? plannedValue);
            int.TryParse(plannedValue, out int plannedResult);
            int? planned = plannedResult == 0 ? null : plannedResult;

            anime.Completed = completed;
            anime.Planned = planned;
            anime.Dropped = dropped;
            anime.OnHold = onHold;
            anime.Watching = watching;

        }
        public async Task UpdateVoiceoverDataAnimeFromFirstEpisodeAsync(AnimeFromParser anime)
        {
            if (anime.IdFromAnimeGo == null)
            {
                return;
            }
            List<string> list = new List<string>();
            using var doc = await GetDocumentFromJsonAsync($"https://animego.org/anime/{anime.IdFromAnimeGo}/player?_allow=true");
            if (doc.StatusCode == HttpStatusCode.OK)
            {
                if (doc.QuerySelector("#video-dubbing") is { } selector)
                {
                    foreach (var e in selector.QuerySelectorAll(".video-player-toggle-item"))
                    {
                        list.Add(e.Text().Trim() );
                    }
                }
            }
            anime.Dubbing = anime.Dubbing.Union(list).ToList();


        }
        public async Task UpdateAllDataAnime(AnimeFromParser anime)
        {
            try
            {
                await UpdateMainDataAnimeAsync(anime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                await UpdateShowDataAnimeAsync(anime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                await UpdateVoiceoverDataAnimeFromFirstEpisodeAsync(anime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private async Task<IDocument> GetDocumentFromHtmlAsync(string url)
        {
            await Task.Delay(300);
            IDocument document = await _context.OpenAsync(url);
            while (document.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(5000);
                document = await _context.OpenAsync(url);
            }
            return document;
        }
        private async Task<IDocument> GetDocumentFromJsonAsync(string url)
        {
            await Task.Delay(300);
            HttpResponseMessage message = await _clientJson.GetAsync(url);
            while (message.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(5000);
                message = await _clientJson.GetAsync(url);
            }

            var text = await message.Content.ReadAsStringAsync();
            JToken jToken = JToken.Parse(text);
            var html = jToken.Last?.Last?.ToString();
            

            return await _context.OpenAsync(req =>
            {
                req.Content(html);
                req.Status(message.StatusCode);
            });
        }
        public async Task<Stream?> GetStreamFromUrl(string? url)
        {
            if (url == null)
            {
                return null;
            }
            await Task.Delay(300);
            var response = await _clientImg.GetAsync(url);
            while (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(5000);
                response = await _clientImg.GetAsync(url);
            }
            return await response.Content.ReadAsStreamAsync();
        }
        public async Task<Stream?> GetSteamPhotoFromAnimeHref(string? hrefAnime)
        {
            if (hrefAnime == null)
            {
                return null;
            }
            using var page = await GetDocumentFromHtmlAsync(hrefAnime);
            if (page.StatusCode == HttpStatusCode.OK)
            {
                var url = page.QuerySelector(".anime-poster")?.QuerySelector("img")?.GetAttribute("src")?.Trim();
                return await GetStreamFromUrl(url);
            }
            else
            {
                return null;
            }
        }
        public void Dispose()
        {
            _context.Dispose();
            _clientJson.Dispose();
            _clientImg.Dispose();
        }
    }
}
