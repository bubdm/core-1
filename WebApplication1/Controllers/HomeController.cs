using System;
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

        public IActionResult Throw(string id) => throw new ApplicationException(id);

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
            var person = _Persons.FirstOrDefault(p => p.Id == id);

            if (person is null)
                return NotFound();

            return View(person);
        }

        public IActionResult Blog() => View();
    }
}
