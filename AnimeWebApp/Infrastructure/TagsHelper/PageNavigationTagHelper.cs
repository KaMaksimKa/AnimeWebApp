using AnimeWebApp.Models;
using AnimeWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AnimeWebApp.Infrastructure.TagsHelper
{
    [HtmlTargetElement("div",Attributes = "page-model")]
    public class PageNavigationTagHelper:TagHelper
    {
        public PagingInfo PageModel { get; set; } = null!;
        public int PageCount { get; set; } = 5;
        private IUrlHelperFactory _factory;
        public PageNavigationTagHelper(IUrlHelperFactory factory)
        {
            _factory = factory;
        }

        [ViewContext] 
        [HtmlAttributeNotBound] 
        public ViewContext Context { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _factory.GetUrlHelper(Context);
            var routeData = new RouteValueDictionary(Context.RouteData.Values);
            foreach (var (key, value) in Context.HttpContext.Request.Query)
            {
                routeData[key] = value.ToString();
            }


            string classForSelectedButton = "btn btn-primary btn-sm m-1";
            string classForOrdinaryButton = "btn btn-secondary btn-sm m-1";

            TagBuilder elementA;

            routeData["numberPage"] = 1;
            elementA = new TagBuilder("a");
            elementA.Attributes.Add("href", urlHelper.RouteUrl(routeData));
            elementA.Attributes.Add($"class", classForOrdinaryButton);
            elementA.InnerHtml.Append($"<<");
            output.Content.AppendHtml(elementA);


            routeData["numberPage"] = Math.Max(1, PageModel.CurrentPage - PageCount);
            elementA = new TagBuilder("a");
            elementA.Attributes.Add("href", urlHelper.RouteUrl(routeData));
            elementA.Attributes.Add($"class", classForOrdinaryButton);
            elementA.InnerHtml.Append($"<");
            output.Content.AppendHtml(elementA);


            int startPage = Math.Max(PageModel.CurrentPage - PageCount / 2, 1);
            int endPage = Math.Min(startPage + PageCount, PageModel.TotalPages + 1);
            for (int i = startPage; i < endPage; i++)
            {
                routeData["numberPage"] = i;
                elementA = new TagBuilder("a");
                elementA.Attributes.Add("href", urlHelper.RouteUrl(routeData));
                elementA.Attributes.Add($"class", PageModel.CurrentPage == i ? classForSelectedButton : classForOrdinaryButton);
                elementA.InnerHtml.Append($"{i}");
                output.Content.AppendHtml(elementA);
            }
            routeData["numberPage"] = Math.Min(PageModel.TotalPages, PageModel.CurrentPage + PageCount);
            elementA = new TagBuilder("a");
            elementA.Attributes.Add("href", urlHelper.RouteUrl(routeData));
            elementA.Attributes.Add($"class", classForOrdinaryButton);
            elementA.InnerHtml.Append($">");
            output.Content.AppendHtml(elementA);


            routeData["numberPage"] = PageModel.TotalPages;
            elementA = new TagBuilder("a");
            elementA.Attributes.Add("href", urlHelper.RouteUrl(routeData));
            elementA.Attributes.Add($"class", classForOrdinaryButton);
            elementA.InnerHtml.Append($">>");
            output.Content.AppendHtml(elementA);

        }
    }
}
