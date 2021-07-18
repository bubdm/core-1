using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces.WebAPI;

namespace WebApplication1.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValuesService _ValuesService;
        public WebAPIController(IValuesService valuesService)
        {
            _ValuesService = valuesService;
        }
        public IActionResult Index()
        {
            var values = _ValuesService.GetAll();
            return View(values);
        }
    }
}
