using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly Mapper _mapperProductToView = new (new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));
        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        
        public IActionResult Index([FromServices] IProductData productData)
        {
            var products = _mapperProductToView
                .Map<IEnumerable<ProductWebModel>>(productData.GetProducts().Products.Take(6));
            ViewBag.Products = products;
            return View();
        }

        public IActionResult Second()
        {
            return Content("Second");
        }

        public IActionResult Blog() => View();

        public IActionResult Throw(string message)
        {
            throw new ApplicationException(message ?? "Test exception in home controller");
        }
    }
}
