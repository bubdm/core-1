using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductData _ProductData;
        public ProductsController(IProductData productData)
        {
            _ProductData = productData;
        }


    }
}
