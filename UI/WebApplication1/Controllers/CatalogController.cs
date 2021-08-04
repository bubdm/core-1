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
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))
            .ForMember("Keywords", o => o.MapFrom(p => p.Keywords.Select(k => k.Word)))
        ));

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _productData = productData;
            _configuration = configuration;
        }

        public IActionResult Index(int? brandId, int? sectionId, int page = 1)
        {
            var model = GetCatalogWebModel(brandId, sectionId, page);
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var product = _productData.GetProductById(id);
            if (product is null)
                return NotFound();
            return View(_mapperProductToView
                .Map<ProductWebModel>(product));
        }

        #region Web-API

        public IActionResult GetFeaturedItems(int? BrandId, int? SectionId, int Page = 1)
        {
            var products = GetProducts(BrandId, SectionId, Page);
            return PartialView("Partial/_ProductsPartial", products);
        }

        public IActionResult GetCatalogPagination(int? BrandId, int? SectionId, int Page = 1)
        {
            var model = GetCatalogWebModel(BrandId, SectionId, Page);
            return PartialView("Partial/_CatalogPaginationPartial", model);
        }

        #endregion

        #region Вспомогательные

        private CatalogWebModel GetCatalogWebModel(int? brandId, int? sectionId, int page)
        {
            int? pageSize = (int.TryParse(_configuration["CatalogPageSize"], out var value) ? value : null);

            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = pageSize,
            };
            var (products, productCount) = _productData.GetProducts(filter);

            var model = new CatalogWebModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                PageWebModel = new PageWebModel(productCount, page, pageSize ?? 0),
                Products = _mapperProductToView
                    .Map<IEnumerable<ProductWebModel>>(products.OrderBy(p => p.Order)),
            };
            return model;
        }

        private IEnumerable<ProductWebModel> GetProducts(int? BrandId, int? SectionId, int Page)
        {
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = _configuration.GetValue("CatalogPageSize", 6),
            };
            var result = _productData.GetProducts(filter).Products.OrderBy(p => p.Order);
            return _mapperProductToView.Map<IEnumerable<ProductWebModel>>(result);
        }

        #endregion
    }
}
