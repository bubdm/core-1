using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.WebAPI.Controllers
{
    [Route(WebAPIInfo.Persons), ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsData _PersonsData;
        public PersonsController(IPersonsData personsData)
        {
            _PersonsData = personsData;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_PersonsData.GetAll());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok(_PersonsData.Get(id));
        }

        [HttpPost]
        public IActionResult Add(Person person)
        {
            var id = _PersonsData.Add(person);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Update(Person person)
        {
            _PersonsData.Update(person);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _PersonsData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
