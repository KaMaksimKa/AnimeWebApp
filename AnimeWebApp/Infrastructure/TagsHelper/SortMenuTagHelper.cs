using AnimeWebApp.Models;
using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AnimeWebApp.Infrastructure.TagsHelper
{
    [HtmlTargetElement("div",Attributes = "sorting-model")]
    public class SortMenuTagHelper:TagHelper
    {
        public SortingInfo SortingModel { get; set; } = null!;
        private IUrlHelperFactory _factory;
        public SortMenuTagHelper(IUrlHelperFactory factory)
        {
            _factory = factory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext Context { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _factory.GetUrlHelper(Context);
            RouteData newRouteData = new RouteData(Context.RouteData);

            foreach (var (key,value) in Context.HttpContext.Request.Query)
            {
                newRouteData.Values[key] = value.ToString();
            }
            output.Attributes.SetAttribute("class", "btn-group");
            var button = new TagBuilder("button");
            button.Attributes["type"] = "button";
            button.Attributes["class"] = "btn btn-dark dropdown-toggle";
            button.Attributes["data-bs-toggle"] = "dropdown";
            button.Attributes["aria-expanded"] = "false";

            
            button.InnerHtml.Append(SortingModel.AllSorts[SortingModel.CurrentSort]);
            output.Content.AppendHtml(button);

            var ul = new TagBuilder("ul");
            ul.Attributes["class"] = "dropdown-menu dropdown-menu-dark dropdown-menu-start dropdown-menu-lg-start";


            foreach (var (key,value) in SortingModel.AllSorts)
            {
                newRouteData.Values["sort"] = key;
                var li = new TagBuilder("li");
                var a = new TagBuilder("a");
                a.Attributes["class"] = "dropdown-item";
                a.Attributes["href"] = urlHelper.RouteUrl(newRouteData.Values);
                a.InnerHtml.Append(value);
                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }

            output.Content.AppendHtml(ul);
        }
    }
}
