﻿namespace AnimeWebApp.Models
{
    public class StandartAnimeHandler:IAnimeHandler
    {
        public string Path { get; } = String.Empty;
        public IAnimeHandler? Next { get; set; }
        public IQueryable<Anime>? Invoke(IQueryable<Anime> anime, IQueryable<int> animeId)
        {
            return  from id in animeId
                     join
                    a in anime on id equals a.AnimeId
                select new Anime
                {
                    IdFromAnimeGo = a.IdFromAnimeGo,
                    Voiceovers = a.Voiceovers,
                    Href = a.Href,
                    Watching = a.Watching,
                    NameRu = a.NameRu,
                    Completed = a.Completed,
                    Status = a.Status,
                    NameEn = a.NameEn,
                    Type = a.Type,
                    Description = a.Description,
                    CountEpisode = a.CountEpisode,
                    AnimeId = a.AnimeId,
                    Rate = a.Rate,
                    Genres = a.Genres,
                    Planned = a.Planned,
                    Dropped = a.Dropped,
                    OnHold = a.OnHold,
                    Studio = a.Studio,
                    NextEpisode = a.NextEpisode,
                    Duration = a.Duration,
                    MpaaRate = a.MpaaRate,
                    Year = a.Year
                };
        }
    }
}
