using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.DTO;
using WebApplication1.Domain.DTO.Mappers;
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
        public IActionResult GetSections()
        {
            return Ok(_ProductData.GetSections().ToDTO());
        }

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSection(int id)
        {
            return Ok(_ProductData.GetSection(id).ToDTO());
        }

        [HttpGet("brands")]
        public IActionResult GetBrands() => Ok(_ProductData.GetBrands().ToDTO());

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrand(int id) => Ok(_ProductData.GetBrand(id).ToDTO());

        [HttpPost("get")]
        public IActionResult GetProducts(ProductFilter filter = null) => Ok(_ProductData.GetProducts(filter).ToDTO());

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id) => Ok(_ProductData.GetProductById(id).ToDTO());

        [HttpPost]
        public IActionResult Add(ProductDTO product)
        {
            var id = _ProductData.Add(product.FromDTO());
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Update(ProductDTO product)
        {
            _ProductData.Update(product.FromDTO());
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _ProductData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
