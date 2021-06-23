using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using WebApplication1.Domain;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.ViewModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly Mapper _mapperProductToView = new (new MapperConfiguration(c => c.CreateMap<Product, ProductViewModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));
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
                Products = _mapperProductToView
                    .Map<IEnumerable<ProductViewModel>>(products.OrderBy(p => p.Order)),
            });
        }

        public IActionResult Details(int id)
        {
            var product = _productData.GetProductById(id);
            if (product is null)
                return NotFound();

            return View(_mapperProductToView
                .Map<ProductViewModel>(product));
        }
    }
}
