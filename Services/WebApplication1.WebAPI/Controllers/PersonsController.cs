using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.WebAPI.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsData _PersonsData;
        public PersonsController(IPersonsData personsData)
        {
            _PersonsData = personsData;
        }


    }
}
