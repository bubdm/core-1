using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Domain.Identity;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
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

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Create() => View("Edit", new PersonViewModel());


        [Authorize(Roles = Role.Administrators)]
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
        
        [HttpPost, Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(PersonViewModel model)
        {
            if (model is null)
                return BadRequest();

            if (model.LastName == "Иванов")
                ModelState.AddModelError(nameof(model.LastName), "Запрещено иметь имя Иванов");

            if (!ModelState.IsValid)
                return View(model);

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

        [Authorize(Roles = Role.Administrators)]
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

        [HttpPost, Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();
            _PersonsData.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
