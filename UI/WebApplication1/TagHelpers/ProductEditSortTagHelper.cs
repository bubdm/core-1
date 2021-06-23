using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApplication1.Areas.Admin.Models;

namespace WebApplication1.TagHelpers
{
    public class ProductEditSortTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _UrlHelperFactory;

        public ProductEditSortState Property { get; set; }
        public ProductEditSortState Current { get; set; }
        public string Action { get; set; }
        public bool Up { get; set; }

        public ProductEditSortTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _UrlHelperFactory = urlHelperFactory;
        }
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _UrlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";
            string url = urlHelper.Action(Action, new {SortOrder = Property});
            output.Attributes.SetAttribute("href", url);

            if (Current == Property)
            {
                TagBuilder tag = new TagBuilder("i");
                tag.AddCssClass("glyphicon");
                if (Up == true)
                    tag.AddCssClass("glyphicon-chevron-up");
                else
                    tag.AddCssClass("glyphicon-chevron-down");

                output.PreContent.AppendHtml(tag);
            }
        }
    }
}
