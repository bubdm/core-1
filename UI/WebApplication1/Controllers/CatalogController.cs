using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using WebApplication1.Domain.DTO.Mappers;
using WebApplication1.Domain.Model;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.WebModel;
using WebApplication1.Domain.WebModel.Admin;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;

        private readonly Mapper _mapperProductToView = new (new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));
        
        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _productData = productData;
            _configuration = configuration;
        }

        public IActionResult Index(int? brandId, int? sectionId, int page = 1, int? PageSize = null)
        {
            var pageSize = PageSize ?? (int.TryParse(_configuration["CatalogPageSize"], out var value) ? value : null);

            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = pageSize,
            };
            var (products, productCount) = _productData.GetProducts(filter);

            return View(new CatalogWebModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                PageWebModel = new PageWebModel(productCount, page, pageSize ?? 0),
                Products = _mapperProductToView
                    .Map<IEnumerable<ProductWebModel>>(products.OrderBy(p => p.Order)),
            });
        }

        public IActionResult Details(int id)
        {
            var product = _productData.GetProductById(id);
            if (product is null)
                return NotFound();

            return View(_mapperProductToView
                .Map<ProductWebModel>(product));
        }

        public IActionResult GetFeaturedItems(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            return PartialView("Partial/_ProductsPartial", GetProducts(BrandId, SectionId, Page, PageSize));
        }

        private IEnumerable<ProductWebModel> GetProducts(int? BrandId, int? SectionId, int Page, int? PageSize)
        {
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = PageSize ?? _configuration.GetValue("", 6),
            };
            var result = _productData.GetProducts(filter).Products.OrderBy(p => p.Order);
            return _mapperProductToView.Map<IEnumerable<ProductWebModel>>(result);
        }
    }
}
