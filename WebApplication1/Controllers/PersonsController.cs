using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Infrastructure.Interfaces;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsData _PersonsData;
        public PersonsController(IPersonsData personsData)
        {
            _PersonsData = personsData;
        }
        
        public IActionResult Index()
        {
            return View(_PersonsData.GetAll());
        }
        
        public IActionResult Details(int id)
        {
            var person = _PersonsData.Get(id);

            if (person is null)
                return NotFound();

            return View(person);
        }

        public IActionResult Create() => View("Edit", new PersonViewModel());

        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();
            var person = _PersonsData.Get(id);
            if (person is null)
                return NotFound();
            var model = new PersonViewModel
            {
                Id = person.Id,
                LastName = person.LastName,
                FirstName = person.FirstName,
                Patronymic = person.Patronymic,
                Age = person.Age,
                Birthday = person.Birthday,
                CountChildren = person.CountChildren,
            };
            return View(model);
        }
        
        [HttpPost]
        public IActionResult Edit(PersonViewModel model)
        {
            if (model is null)
                return BadRequest();
            var person = new Person
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Birthday = model.Birthday,
                CountChildren = model.CountChildren,
            };

            if (person.Id == 0)
                _PersonsData.Add(person);
            else
                _PersonsData.Update(person);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            var person = _PersonsData.Get(id);
            if (person is null)
                return NotFound();
            var model = new PersonViewModel
            {
                Id = person.Id,
                LastName = person.LastName,
                FirstName = person.FirstName,
                Patronymic = person.Patronymic,
                Age = person.Age,
                Birthday = person.Birthday,
                CountChildren = person.CountChildren,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();
            _PersonsData.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
