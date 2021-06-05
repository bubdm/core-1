using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Infrastructure.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("Staff")]
    public class PersonsController : Controller
    {
        private readonly IPersonsData _PersonsData;
        public PersonsController(IPersonsData personsData)
        {
            _PersonsData = personsData;
        }

        [Route("All")]
        public IActionResult Index()
        {
            return View(_PersonsData.GetAll());
        }

        [Route("Info/{id}")]
        public IActionResult Details(int id)
        {
            var person = _PersonsData.Get(id);

            if (person is null)
                return NotFound();

            return View(person);
        }
    }
}
