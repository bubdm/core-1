using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApplication1.Domain.WebModel.Admin;

namespace WebApplication1.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _UrlHelperFactory;

        public PageWebModel PageModel { get; set; }

        public string PageAction { get; set; }

        public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _UrlHelperFactory = urlHelperFactory;
        }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new (); 

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _UrlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            var tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            var currentItem = CreateTag(PageModel.Page, urlHelper);

            if (PageModel.HasPreviousPage)
            {
                var prevItem = CreateTag(PageModel.Page - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            tag.InnerHtml.AppendHtml(currentItem);

            if (PageModel.HasNextPage)
            {
                var nextItem = CreateTag(PageModel.Page + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }

            output.Content.AppendHtml(tag);
        }
        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");

            if (pageNumber == PageModel.Page)
            {
                item.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }

            foreach (var (key, value) in PageUrlValues.Where(v => v.Value is not null))
                link.MergeAttribute($"data-{key}", value.ToString());

            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
