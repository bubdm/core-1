using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleMvcSitemap;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Controllers.API
{
    public class SiteMapController : ControllerBase
    {
        public IActionResult Index([FromServices] IProductData productData)
        {
            var nodes = new List<SitemapNode>()
            {
                new (Url.Action("Index", "Home")),
                new (Url.Action("Second", "Home")),
                new (Url.Action("Blog", "Home")),
                new (Url.Action("Index", "Catalog")),
                new (Url.Action("Index", "WebAPI")),
            };
            nodes.AddRange(productData.GetSections()
                .Select(s => new SitemapNode(Url.Action("Index", "Catalog", new { SectionId = s.Id }))));
            foreach (var brand in productData.GetBrands())
                nodes.Add(new SitemapNode(Url.Action("Index", "Catalog", new {BrandId = brand.Id})));
            foreach (var product in productData.GetProducts())
                nodes.Add(new SitemapNode(Url.Action("Details", "Catalog", new {product.Id})));
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
