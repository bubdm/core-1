using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;

        private readonly IEnumerable<Person> _Persons = Person.GetPersons;
        
        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Second()
        {
            return Content("Second");
        }

        public IActionResult Persons()
        {
            return View(_Persons);
        }

        public IActionResult Details(int id)
        {
            return View(_Persons.First(p => p.Id == id));
        }

        public IActionResult Blog() => View();
    }
}
