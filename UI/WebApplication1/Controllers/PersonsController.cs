using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Identity;
using WebApplication1.Domain.ViewModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class PersonsController : Controller
    {
        private readonly IPersonsData _PersonsData;
        private readonly Mapper _mapperPersonToView = 
            new (new MapperConfiguration(c => c.CreateMap<Person, PersonViewModel>()));
        private readonly Mapper _mapperPersonFromView =
            new(new MapperConfiguration(c => c.CreateMap<PersonViewModel, Person>()));
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

            return View(_mapperPersonToView.Map<PersonViewModel>(person));
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

            var person = _mapperPersonFromView.Map<Person>(model);

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

            return View(_mapperPersonToView.Map<PersonViewModel>(person));
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
