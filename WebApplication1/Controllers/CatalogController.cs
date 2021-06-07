using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        public CatalogController(IProductData productData)
        {
            _productData = productData;
        }
        public IActionResult Index(int? brandId, int? catalogId)
        {
            return View();
        }
    }
}
