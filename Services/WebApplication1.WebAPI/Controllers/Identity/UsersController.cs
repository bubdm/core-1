using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dal.Context;
using WebApplication1.Domain.Identity;
using WebApplication1.Interfaces.Adresses;

namespace WebApplication1.WebAPI.Controllers.Identity
{
    [Route(WebAPIInfo.Identity.Users), ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserStore<User, Role, Application1Context> _userStore;
        public UsersController(Application1Context context)
        {
            _userStore = new UserStore<User, Role, Application1Context>(context);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAllUsers() => 
            await _userStore.Users.ToArrayAsync();



    }
}
