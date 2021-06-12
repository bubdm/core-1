using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication.Domain;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;


        
        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        public IActionResult Throw(string id) => throw new ApplicationException(id);

        public IActionResult Index([FromServices] IProductData productData)
        {
            var products = productData
                .GetProducts()
                .Take(6)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                });
            ViewBag.Products = products;
            return View();
        }

        public IActionResult Second()
        {
            return Content("Second");
        }

        public IActionResult Blog() => View();
    }
}
