using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;
        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        public IActionResult Index()
        {
            return Content($"Hello => {_Configuration["Hello"]}");
        }

        public IActionResult Second()
        {
            return Content("Second");
        }

        public IActionResult Third()
        {
            return Content($"Third + {_Configuration["Hello"]}");
        }
    }
}
