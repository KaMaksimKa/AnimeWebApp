namespace AnimeWebApp.Models
{
    public class CombiningAnimeHandler:IAnimeHandler
    {
        public IAnimeHandler? Next { get; set; }
        private List<IAnimeHandler> _handlers;
        public CombiningAnimeHandler(List<IAnimeHandler> handlers)
        {
            _handlers = handlers;
        }
        public IQueryable<Anime>? Invoke(IQueryable<Anime>? animes)
        {
            if (_handlers.Count >1)
            {
                for (int i = 0; i < _handlers.Count-1; i++)
                {
                    _handlers[i].Next = _handlers[i+1];
                }
                _handlers.Last().Next = Next;
            }
            else if (_handlers.Count == 1)
            {
                _handlers.First().Next = Next;
                
            }
            return _handlers.FirstOrDefault()?.Invoke(animes);
        }
    }
}
