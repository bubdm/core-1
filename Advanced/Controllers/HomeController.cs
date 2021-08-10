using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advanced.Models;
using Microsoft.EntityFrameworkCore;

namespace Advanced.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index([FromQuery] string selectedCity)
        {
            var model = new PersonsWebModel
            {
                Persons = _context.Persons.Include(p => p.Department).Include(p => p.Location),
                Cities = _context.Locations.Select(l => l.City).Distinct(),
                SelectedCity = selectedCity,
            };
            return View(model);
        }
    }

    public class PersonsWebModel
    {
        public IEnumerable<Person> Persons { get; set; }
        public IEnumerable<string> Cities { get; set; }
        public string SelectedCity { get; set; }
        public string GetClass(string city) => 
            SelectedCity == city ? "bg-info text-white" : "";
    }
}
