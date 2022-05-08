using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AnimeWebApp.Infrastructure.TagsHelper
{
    [HtmlTargetElement("div",Attributes = "sorting-model")]
    public class SortMenuTagHelper:TagHelper
    {
        public PagingInfo PageModel { get; set; } = null!;
        private IUrlHelperFactory _factory;
        public SortMenuTagHelper(IUrlHelperFactory factory)
        {
            _factory = factory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext Context { get; set; } = null!;
    }
}
