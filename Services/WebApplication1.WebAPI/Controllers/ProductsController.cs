using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.DTO;
using WebApplication1.Domain.Entities;
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
        public IActionResult GetSections() => Ok(DTOMappers.MapperSectionToDTO
            .Map<IEnumerable<SectionDTO>>(_ProductData.GetSections()));

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSection(int id)
        {
            var source = _ProductData.GetSection(id);
            var section = DTOMappers.MapperSectionToDTO
                .Map<SectionDTO>(source);
            return Ok(section);
        }

        [HttpGet("brands")]
        public IActionResult GetBrands() => Ok(DTOMappers.MapperBrandToDTO
            .Map<IEnumerable<BrandDTO>>(_ProductData.GetBrands()));

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrand(int id) => Ok(DTOMappers.MapperBrandToDTO
            .Map<BrandDTO>(_ProductData.GetBrand(id)));

        [HttpPost("get")]
        public IActionResult GetProducts(ProductFilter filter = null) => Ok(DTOMappers.MapperProductToDTO
            .Map<IEnumerable<ProductDTO>>(_ProductData.GetProducts(filter)));

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id) => Ok(DTOMappers.MapperProductToDTO
            .Map<ProductDTO>(_ProductData.GetProductById(id)));

        [HttpPost]
        public IActionResult Add(ProductDTO product)
        {
            var id = _ProductData.Add(DTOMappers.MapperProductFromDTO
                .Map<Product>(product));
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Update(ProductDTO product)
        {
            _ProductData.Update(DTOMappers.MapperProductFromDTO
                .Map<Product>(product));
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
