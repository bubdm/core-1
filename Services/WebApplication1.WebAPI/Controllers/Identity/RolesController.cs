using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Dal.Context;
using WebApplication1.Domain.Identity;
using WebApplication1.Interfaces.Adresses;

namespace WebApplication1.WebAPI.Controllers.Identity
{
    [Route(WebAPIInfo.Identity.Roles), ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly RoleStore<Role> _roleStore;
        public RolesController(Application1Context context, ILogger<RolesController> logger)
        {
            _logger = logger;
            _roleStore = new RoleStore<Role>(context);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Role>> GetAllRolesAsync() => 
            await _roleStore.Roles.ToArrayAsync();

        [HttpPost]
        public async Task<bool> CreateAsync(Role role)
        {
            var result = await _roleStore.CreateAsync(role).ConfigureAwait(false);
            if (result.Succeeded == false) _logger.LogError($"Создание роли пользователей {role.Name} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync(Role role)
        {
            var result = await _roleStore.UpdateAsync(role).ConfigureAwait(false);
            if (result.Succeeded == false) _logger.LogError($"Обновление роли пользователей {role.Name} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpDelete("delete")]
        public async Task<bool> DeleteAsync(Role role)
        {
            var result = await _roleStore.DeleteAsync(role).ConfigureAwait(false);
            if (result.Succeeded == false) _logger.LogError($"Удаление роли пользователей {role.Name} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpPost("getroleid")]
        public async Task<string> GetRoleIdAsync([FromBody] Role role)
        {
            return await _roleStore.GetRoleIdAsync(role).ConfigureAwait(false);
        }

        [HttpPost("getrolename")]
        public async Task<string> GetRoleNameAsync([FromBody] Role role)
        {
            return await _roleStore.GetRoleNameAsync(role).ConfigureAwait(false);
        }

        [HttpPost("setrolename/{name}")]
        public async Task<string> SetRoleNameAsync(Role role, string name)
        {
            await _roleStore.SetRoleNameAsync(role, name).ConfigureAwait(false);
            await _roleStore.UpdateAsync(role);
            if (!string.Equals(role.Name, name)) _logger.LogError($"Изменение имени роли {role.Name} на {name} завершилось неудачей");
            return role.Name;
        }

        [HttpPost("getnormalizedrolename")]
        public async Task<string> GetNormalizedRoleNameAsync([FromBody] Role role)
        {
            return await _roleStore.GetNormalizedRoleNameAsync(role).ConfigureAwait(false);
        }

        [HttpPost("setnormalizedrolename")]
        public async Task<string> SetNormalizedRoleNameAsync(Role role, string name)
        {
            await _roleStore.SetNormalizedRoleNameAsync(role, name).ConfigureAwait(false);
            await _roleStore.UpdateAsync(role);
            if (!string.Equals(role.NormalizedName, name)) _logger.LogError($"Изменение имени роли {role.NormalizedName} на {name} завершилось неудачей");
            return role.NormalizedName;
        }

        [HttpGet("findbyid/{id}")]
        public async Task<Role> FindByIdAsync(string id)
        {
            return await _roleStore.FindByIdAsync(id).ConfigureAwait(false);
        }

        [HttpGet("findbyname/{name}")]
        public async Task<Role> FindByNameAsync(string name)
        {
            return await _roleStore.FindByNameAsync(name).ConfigureAwait(false);
        }

    }
}
