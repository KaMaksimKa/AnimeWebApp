namespace AnimeWebApp.Models.ViewModels
{
    public class SortingInfo
    {
        public string CurrentSort { get; }
        public Dictionary<string, string> AllTypesSorts { get; } = new Dictionary<string, string>
        {
            ["date-add-desc"] = "Дате добавления ▽",
            ["date-add"] = "Дате добавления △",
            ["rating-desc"] = "Рейтингу ▽",
            ["rating"] = "Рейтингу △",
            ["completed-desc"] = "Уже посмотрели ▽",
            ["completed"] = "Уже посмотрели △",
            ["watching-desc"] = "Смотрят сейчас ▽",
            ["watching"] = "Смотрят сейчас △",
        };

        public List<string> AllowedTypesSorts { get; set; } = new List<string>();

        public SortingInfo(string currentSort)
        {
            CurrentSort = currentSort;
            foreach (var (key,_) in AllTypesSorts)
            {
                if (key.Contains("-desc") && currentSort.Replace("-desc","")!=key.Replace("-desc", ""))
                {
                    AllowedTypesSorts.Add(key);
                }

                if (currentSort.Replace("-desc", "") == key.Replace("-desc", "") &&
                    currentSort != key)
                {
                    AllowedTypesSorts.Add(key);
                }
            } 
        }
    }
}
