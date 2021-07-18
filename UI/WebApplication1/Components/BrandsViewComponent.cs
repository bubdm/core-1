using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;
        public BrandsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandWebModel> GetBrands() => _productData
            .GetBrands().OrderBy(b => b.Order)
            .Select(b =>
            new BrandWebModel
            {
                Id = b.Id,
                Name = b.Name,
            });
    }
}
