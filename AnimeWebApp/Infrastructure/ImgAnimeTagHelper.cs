using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AnimeWebApp.Infrastructure
{
    [HtmlTargetElement("img",Attributes = "anime-id")]
    public class ImgAnimeTagHelper : TagHelper
    {
        public int? AnimeId { get; set; }
        public string PathAnime { get; set; } = "/img/anime/";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.StartTagOnly;
            if (AnimeId != null)
            {
                output.Attributes.Add(new TagHelperAttribute("src", PathAnime + AnimeId + ".jpg"));
            }
            

        }
    }
}
