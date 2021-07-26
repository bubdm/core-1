using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.TagHelpers
{
    [HtmlTargetElement(Attributes = attributeName)]
    public class ActiveRoute : TagHelper
    {
        private const string attributeName = "my-active-route";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll(attributeName);
        }
    }
}
