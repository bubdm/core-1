using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("Staff")]
    public class PersonsController : Controller
    {
        private readonly IEnumerable<Person> _Persons = Person.GetPersons;

        [Route("All")]
        public IActionResult Index()
        {
            return View(_Persons);
        }

        [Route("Info/{id}")]
        public IActionResult Details(int id)
        {
            var person = _Persons.FirstOrDefault(p => p.Id == id);

            if (person is null)
                return NotFound();

            return View(person);
        }
    }
}
