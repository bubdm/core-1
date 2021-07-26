using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        private const string attributeName = "active-route";
        
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> RouteValues { get; set; } = new (StringComparer.OrdinalIgnoreCase);

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsActive())
                MakeActive(output);


            output.Attributes.RemoveAll(attributeName);
        }

        private bool IsActive()
        {
            var routeValues = ViewContext.RouteData.Values;

            var currentController = routeValues["controller"]?.ToString();
            var currentAction = routeValues["action"]?.ToString();

            if (!string.IsNullOrEmpty(Controller) && !string.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase))
                return false;
            if (!string.IsNullOrEmpty(Action) && !string.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase))
                return false;

            foreach (var (key, value) in RouteValues)
                if (!routeValues.ContainsKey(key) || routeValues[key]?.ToString() != value)
                    return false;

            return true;
        }

        private static void MakeActive(TagHelperOutput output)
        {
            var classAttribute = output.Attributes.FirstOrDefault(a => a.Name == "class");

            if (classAttribute is null)
                output.Attributes.Add("class", "active");
            else
            {
                if (classAttribute.Value.ToString()?.Contains("active") ?? false)
                    return;
                output.Attributes.SetAttribute("class", classAttribute.Value + " active");
            }
        }
    }
}
