using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Model;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.WebAPI.Controllers
{
    [Route(WebAPIInfo.Products), ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductData _ProductData;
        public ProductsController(IProductData productData)
        {
            _ProductData = productData;
        }

        [HttpGet("sections")]
        public IActionResult GetSections() => Ok(_ProductData.GetSections());

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSection(int id) => Ok(_ProductData.GetSection(id));

        [HttpGet("brands")]
        public IActionResult GetBrands() => Ok(_ProductData.GetBrands());

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrand(int id) => Ok(_ProductData.GetBrand(id));

        [HttpPost]
        public IActionResult GetProducts(ProductFilter filter = null) => Ok(_ProductData.GetProducts(filter));

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id) => Ok(_ProductData.GetProductById(id));
    }
}
