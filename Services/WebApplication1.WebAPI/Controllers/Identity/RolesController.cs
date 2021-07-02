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
    [Route(WebAPIInfo.Identity.Roles), ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleStore<Role> _roleStore;
        public RolesController(Application1Context context)
        {
            _roleStore = new RoleStore<Role>(context);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAllRoles() => 
            await _roleStore.Roles.ToArrayAsync();



    }
}
