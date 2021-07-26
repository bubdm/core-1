﻿using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData)
        {
            _productData = productData;
        }

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbsWebModel();
            if (int.TryParse(Request.Query["SectionId"], out var sectionId))
            {
                model.Section = _productData.GetSection(sectionId);
                if (model.Section?.ParentId is { } parentSectionId)
                    model.Section.Parent = _productData.GetSection(parentSectionId);
            }
            if (int.TryParse(Request.Query["BrandId"], out var brandId))
                model.Brand = _productData.GetBrand(brandId);
            if (int.TryParse(ViewContext.RouteData.Values["Id"]?.ToString(), out var productId))
                model.Product = _productData.GetProductById(productId)?.Name;
            return View(model);
        }
    }
}
