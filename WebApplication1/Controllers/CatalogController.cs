﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication.Domain;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        public CatalogController(IProductData productData)
        {
            _productData = productData;
        }
        public IActionResult Index(int? brandId, int? sectionId)
        {
            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
            };
            var products = _productData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products
                    .OrderBy(p => p.Order)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                    })
            });
        }
    }
}